using System;
using System.Collections.Generic;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using MvvmCross.Binding.Attributes;
using MvvmCross.DroidX.RecyclerView;

namespace QuickList
{
    [Register("QuickList.QuickRecyclerView")]
    public class QuickRecyclerView : AndroidX.RecyclerView.Widget.RecyclerView
    {
        public QuickRecyclerView(Context context, IAttributeSet attrs) :
            this(context, attrs, 0, new QuickRecyclerViewAdapter())
        {
        }

        public QuickRecyclerView(Context context, IAttributeSet attrs, int defStyle) 
            : this(context, attrs, defStyle, new QuickRecyclerViewAdapter())
        {
        }

        [Android.Runtime.Preserve(Conditional = true)]
        protected QuickRecyclerView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public QuickRecyclerView(Context context, IAttributeSet attrs, int defStyle, QuickRecyclerViewAdapter adapter)
            : base(context, attrs, defStyle)
        {
            if (adapter == null)
                return;

            var currentLayoutManager = GetLayoutManager();
            if (currentLayoutManager == null)
                SetLayoutManager(new MvxGuardedLinearLayoutManager(context));
            
            Adapter = adapter;
        }
        
        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            DetachedFromWindow();
        }

        protected virtual void DetachedFromWindow()
        {
            // Remove all the views that are currently in play.
            // This clears out all of the ViewHolder DataContexts by detaching the ViewHolder.
            // Eventually the GC will come along and clear out the binding contexts.
            // Issue #1405
             //Note: this has a side effect of breaking fragment transitions, as the recyclerview is cleared before the transition starts, which empties the view and displays a "black" screen while transitioning.
            GetLayoutManager()?.RemoveAllViews();
        }

        [MvxSetToNullAfterBinding]
        public new QuickRecyclerViewAdapter Adapter
        {
            get => GetAdapter() as QuickRecyclerViewAdapter;
            set
            {
                var existing = Adapter;
                if (existing == value)
                    return;

                // Support lib doesn't seem to have anything similar to IListAdapter yet
                // hence cast to Adapter.
                if (value != null && existing != null)
                {
                    value.ItemsSource = existing.ItemsSource;
                    value.ItemClick = existing.ItemClick;
                    value.ItemLongClick = existing.ItemLongClick;

                    SwapAdapter((Adapter)value, false);
                }
                else
                {
                    SetAdapter((Adapter)value);
                }

                if (existing != null)
                    existing.ItemsSource = null;
            }
        }

        [MvxSetToNullAfterBinding]
        public IEnumerable<SectionRowModel> ItemsSource
        {
            get => Adapter.ItemsSource;
            set
            {
                var adapter = Adapter;
                if (adapter != null)
                    adapter.ItemsSource = value;
            }
        }
        
        [MvxSetToNullAfterBinding]
        public ICommand ItemClick
        {
            get => Adapter.ItemClick;
            set => Adapter.ItemClick = value;
        }

        [MvxSetToNullAfterBinding]
        public ICommand ItemLongClick
        {
            get => Adapter.ItemLongClick;
            set => Adapter.ItemLongClick = value;
        }
    }
}