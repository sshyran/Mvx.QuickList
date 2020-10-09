using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Foundation;
using MvvmCross.Binding.Attributes;
using MvvmCross.Commands;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.WeakSubscription;
using QuickList.Views;
using UIKit;

namespace QuickList
{
    public class QuickTableViewSource : MvxTableViewSource
    {
        private IDisposable? _subscription;
        public IMvxCommand? SubmitCommand { get; set; }
        
        private IList<SectionRowModel> _itemsSource = null!;

        [MvxSetToNullAfterBinding]
        protected new virtual IList<SectionRowModel> ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (ReferenceEquals(_itemsSource, value)
                    && !ReloadOnAllItemsSourceSets)
                    return;

                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }

                _itemsSource = value;

                var collectionChanged = _itemsSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                {
                    _subscription = collectionChanged.WeakSubscribe(CollectionChangedOnCollectionChanged);
                }

                ReloadTableData();
            }
        }

        public QuickTableViewSource(UITableView tableView) : base(tableView)
        {
            tableView.RegisterClassForCellReuse(typeof(ImageCell), ImageCell.Key);
            tableView.RegisterClassForCellReuse(typeof(LabelCell), LabelCell.Key);
            tableView.RegisterClassForCellReuse(typeof(OptionCell), OptionCell.Key);
            tableView.RegisterClassForCellReuse(typeof(SwitchCell), SwitchCell.Key);
            tableView.RegisterClassForCellReuse(typeof(CommandCell), CommandCell.Key);
            tableView.RegisterClassForCellReuse(typeof(TextFieldCell), TextFieldCell.Key);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return ItemsSource.Count;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ItemsSource[(int)section].Rows.Count;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ItemsSource[indexPath.Section].Rows[indexPath.Row];
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return ItemsSource[(int)section].Title;
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            return item switch
            {
                ImageRowModel _ => tableView.DequeueReusableCell(ImageCell.Key, indexPath),
                LabelRowModel _ => tableView.DequeueReusableCell(LabelCell.Key, indexPath),
                SwitchRowModel _ => tableView.DequeueReusableCell(SwitchCell.Key, indexPath),
                CommandRowModel _ => tableView.DequeueReusableCell(CommandCell.Key, indexPath),
                TextEntryRowModel _ => tableView.DequeueReusableCell(TextFieldCell.Key, indexPath),
                OptionRowModel _ => tableView.DequeueReusableCell(OptionCell.Key, indexPath),
                _ => throw new Exception("Could not determine cell type")
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
            }

            base.Dispose(disposing);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);
            var rowModel = ItemsSource[indexPath.Section].Rows[indexPath.Row];
            if (rowModel is ImageRowModel imageRowModel && imageRowModel.ImageUrl != null)
            {
                if (imageRowModel.TapCommand?.CanExecute(imageRowModel.ImageUrl) ?? false)
                    imageRowModel.TapCommand?.Execute(imageRowModel.ImageUrl);
            }
        }
    }
}
