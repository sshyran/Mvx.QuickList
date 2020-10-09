using System;
using System.Collections.Generic;
using Foundation;
using MvvmCross.Binding.Bindings;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace QuickList
{
    public class OptionTableViewSource : MvxStandardTableViewSource
    {
        public OptionTableViewSource(UITableView tableView) : base(tableView)
        {
        }

        public OptionTableViewSource(UITableView tableView, NSString cellIdentifier) : base(tableView, cellIdentifier)
        {
        }

        public OptionTableViewSource(UITableView tableView, string bindingText) : base(tableView, bindingText)
        {
        }

        public OptionTableViewSource(IntPtr handle) : base(handle)
        {
        }

        public OptionTableViewSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, string bindingText, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None) : base(tableView, style, cellIdentifier, bindingText, tableViewCellAccessory)
        {
        }

        public OptionTableViewSource(UITableView tableView, UITableViewCellStyle style, NSString cellIdentifier, IEnumerable<MvxBindingDescription> descriptions, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None) : base(tableView, style, cellIdentifier, descriptions, tableViewCellAccessory)
        {
        }

        private int _checkedRow = -1;

        public string? CheckedRow
        {
            get => GetOptionAt(_checkedRow);
            set
            {
                var shouldReload = GetOptionAt(_checkedRow) != value;
                _checkedRow = ItemsSource.GetPosition(value);
                if (shouldReload) ReloadTableData();
            }
        }

        private string? GetOptionAt(int index)
        {
            if (index < 0) return null;
            return (string)ItemsSource.ElementAt(index);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = base.GetOrCreateCellFor(tableView, indexPath, item);
            cell.Accessory = _checkedRow == indexPath.Row ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
            cell.TextLabel.TextColor = UIColor.FromName("Secondary");
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var lastCheckedRow = _checkedRow; //get last checked row information before selecting the row!
            base.RowSelected(tableView, indexPath);
            if (lastCheckedRow == indexPath.Row)
            {
                CheckedRow = null;
            }
            else
            {
                var item = (string)GetItemAt(indexPath);
                if (item != CheckedRow)
                {
                    CheckedRow = item;
                }
            }
        }
    }
}
