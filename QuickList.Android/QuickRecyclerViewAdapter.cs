using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Binding;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Extensions;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.DroidX.RecyclerView.Model;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.WeakSubscription;
using Object = Java.Lang.Object;

namespace QuickList
{
    public class QuickRecyclerViewAdapter : AndroidX.RecyclerView.Widget.RecyclerView.Adapter, IMvxRecyclerAdapterBindableHolder
    {
        
        private ICommand _itemClick, _itemLongClick;
        private IEnumerable<SectionRowModel> _itemsSource;
        private IList<IRowModel> _flattenedSource;
        private IDisposable _subscription;

        protected IMvxAndroidBindingContext BindingContext { get; }

        public bool ReloadOnAllItemsSourceSets { get; set; }

        public QuickRecyclerViewAdapter() : this(null)
        {
        }

        public QuickRecyclerViewAdapter(IMvxAndroidBindingContext bindingContext)
        {
            BindingContext = bindingContext ?? MvxAndroidBindingContextHelpers.Current();
        }

        protected QuickRecyclerViewAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        [MvxSetToNullAfterBinding]
        public ICommand ItemClick
        {
            get => _itemClick;
            set
            {
                if (ReferenceEquals(_itemClick, value))
                    return;

                if (_itemClick != null && value != null)
                    Mvx.IoCProvider.Resolve<IMvxLog>().Warn("Changing ItemClick may cause inconsistencies where some items still call the old command.");

                _itemClick = value;
            }
        }

        [MvxSetToNullAfterBinding]
        public ICommand ItemLongClick
        {
            get => _itemLongClick;
            set
            {
                if (ReferenceEquals(_itemLongClick, value))
                    return;

                if (_itemLongClick != null && value != null)
                    Mvx.IoCProvider.Resolve<IMvxLog>().Warn("Changing ItemLongClick may cause inconsistencies where some items still call the old command.");

                _itemLongClick = value;
            }
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable<SectionRowModel> ItemsSource
        {
            get => _itemsSource;
            set => SetItemsSource(value);
        }
        
        public override void OnViewAttachedToWindow(Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Object holder)
        {
            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnDetachedFromWindow();
            base.OnViewDetachedFromWindow(holder);
        }

        public override int GetItemViewType(int position)
        {
            var itemAtPosition = GetItem(position);
            return (int) itemAtPosition.RowModelType();
        }

        public override AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);

            var viewHolder = new MvxRecyclerViewHolder(InflateViewForHolder(parent, viewType, itemBindingContext), itemBindingContext)
            {
                Id = viewType
            };

            return viewHolder;
        }

        protected virtual View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
        {
            // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
            var layoutId = (QuickRowModelType) viewType switch
            {
                QuickRowModelType.Section => Resource.Layout.item_quick_section,
                QuickRowModelType.Command => Resource.Layout.item_quick_command,
                QuickRowModelType.Switch => Resource.Layout.item_quick_switch,
                QuickRowModelType.Text => Resource.Layout.item_quick_text_entry,
                QuickRowModelType.Image => Resource.Layout.item_quick_image,
                QuickRowModelType.Label => Resource.Layout.item_quick_label,
                QuickRowModelType.Option => Resource.Layout.item_quick_option,
                _ => throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null)
            };
            return bindingContext.BindingInflate(layoutId, parent, false);
        }

        public override void OnBindViewHolder(AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder holder, int position)
        {
            var dataContext = GetItem(position);
            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.DataContext = dataContext;

            if (viewHolder.Id == Android.Resource.Layout.SimpleListItem1)
                ((TextView)holder.ItemView).Text = dataContext?.ToString();

            viewHolder.Click -= OnItemViewClick;
            viewHolder.LongClick -= OnItemViewLongClick;
            viewHolder.Click += OnItemViewClick;
            viewHolder.LongClick += OnItemViewLongClick;

            OnMvxViewHolderBound(new MvxViewHolderBoundEventArgs(position, dataContext, holder));
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.Click -= OnItemViewClick;
            viewHolder.LongClick -= OnItemViewLongClick;
            viewHolder.OnViewRecycled();
        }

        public override void OnDetachedFromRecyclerView(AndroidX.RecyclerView.Widget.RecyclerView recyclerView)
        {
            base.OnDetachedFromRecyclerView(recyclerView);
            Clean(false);
        }

        /// <summary>
        /// By default, force recycling a view if it has animations
        /// </summary>
        public override bool OnFailedToRecycleView(Object holder) => true;

        protected virtual void OnItemViewClick(object sender, EventArgs e)
        {
            var holder = (IMvxRecyclerViewHolder)sender;
            ExecuteCommandOnItem(ItemClick, holder.DataContext);
            if (holder.DataContext != null && holder.DataContext is ImageRowModel imageRowModel)
            {
                if (imageRowModel.TapCommand != null && imageRowModel.TapCommand.CanExecute(imageRowModel.ImageUrl))
                {
                    imageRowModel.TapCommand.Execute(imageRowModel.ImageUrl);
                }
            }
        }

        protected virtual void OnItemViewLongClick(object sender, EventArgs e)
        {
            var holder = (IMvxRecyclerViewHolder)sender;
            ExecuteCommandOnItem(ItemLongClick, holder.DataContext);
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, object itemDataContext)
        {
            if (command != null && itemDataContext != null && command.CanExecute(itemDataContext))
                command.Execute(itemDataContext);
        }

        public override int ItemCount => _flattenedSource?.Count ?? 0;

        public virtual object GetItem(int viewPosition)
        {
            if (viewPosition >= 0 && viewPosition < _flattenedSource.Count)
                return _flattenedSource[viewPosition];
            Mvx.IoCProvider.Resolve<IMvxLog>().Error($"MvxRecyclerView GetItem index out of range. viewPosition:{viewPosition} itemCount:{MvxEnumerableExtensions.Count(_itemsSource)}");
            //We should trigger an exception instead of hiding it here, as it means you have bugs in your code.
            return null; 
        }

        private int GetViewPosition(object item)
        {
            return _flattenedSource.GetPosition(item);
        }
        
        protected virtual void SetItemsSource(IEnumerable<SectionRowModel> value)
        {
            if (Looper.MainLooper != Looper.MyLooper())
                Mvx.IoCProvider.Resolve<IMvxLog>().Error("ItemsSource property set on a worker thread. This leads to crash in the RecyclerView. It must be set only from the main thread.");

            if (ReferenceEquals(_itemsSource, value) && !ReloadOnAllItemsSourceSets)
                return;

            _subscription?.Dispose();
            _subscription = null;

            if (value != null && !(value is IList<SectionRowModel>))
            {
                MvxBindingLog.Warning("Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");
            }

            if (value is INotifyCollectionChanged newObservable)
                _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);

            _itemsSource = value;
            if (_itemsSource == null)
            {
                _flattenedSource = null;
            } 
            else 
            {
                _flattenedSource = new List<IRowModel>();
                foreach (var section in _itemsSource)
                {
                    _flattenedSource.Add(section);
                    foreach (var quickRow in section.Rows)
                    {
                        _flattenedSource.Add(quickRow);
                    }
                }
            }
            NotifyDataSetChanged();
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_subscription == null || _itemsSource == null) //Object disposed
                return;

            if (Looper.MainLooper == Looper.MyLooper())
                NotifyDataSetChanged(e);
            else
                Mvx.IoCProvider.Resolve<IMvxLog>().Error("ItemsSource collection content changed on a worker thread. This leads to crash in the RecyclerView as it will not be aware of changes immediatly and may get a deleted item or update an item with a bad item template. All changes must be synchronized on the main thread.");
        }

        public virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    NotifyItemRangeInserted(GetViewPosition(e.NewStartingIndex), e.NewItems.Count);
                    break;
                case NotifyCollectionChangedAction.Move:
                    for (var i = 0; i < e.NewItems.Count; i++)
                        NotifyItemMoved(GetViewPosition(e.OldStartingIndex + i), GetViewPosition(e.NewStartingIndex + i));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    NotifyItemRangeChanged(GetViewPosition(e.NewStartingIndex), e.NewItems.Count);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    NotifyItemRangeRemoved(GetViewPosition(e.OldStartingIndex), e.OldItems.Count);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    NotifyDataSetChanged();
                    break;
            }
        }

        public event Action<MvxViewHolderBoundEventArgs> MvxViewHolderBound;

        protected virtual void OnMvxViewHolderBound(MvxViewHolderBoundEventArgs obj)
        {
            MvxViewHolderBound?.Invoke(obj);
        }

        private void Clean(bool disposing)
        {
            if (disposing)
            {
                _subscription?.Dispose();
                _subscription = null;
                _itemClick = null;
                _itemLongClick = null;
                _itemsSource = null;
                _flattenedSource = null;
            }
        }

        /// <summary>
        /// Always called with disposing = false, as it is only disposed from java
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            Clean(true);
            base.Dispose(disposing);
        }
    }
}