using System;
using Cirrious.FluentLayouts.Touch;
using FluentTableView;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace QuickList.Views
{
    public class OptionCell : MvxFluentTableViewCell
    {
        public static readonly NSString Key = new NSString("OptionCell");
        
        private const int DefaultPadding = 16;
        
        private readonly UILabel _label = new UILabel()
        {
            Lines = 0
        };
        
        private readonly UITableView _optionsTableView = new FullHeightTableView
        {
            ScrollEnabled = false,
            AllowsSelection = true,
            AllowsMultipleSelection = false
        };

        private readonly OptionTableViewSource _tableViewSource;
        
        public OptionCell(IntPtr handle) : base(handle)
        {
            _tableViewSource = new OptionTableViewSource(_optionsTableView)
            {
                DeselectAutomatically = true
            };
            _optionsTableView.Source = _tableViewSource;
        }

        protected override void SetupConstraints()
        {
            ContentView.AddSubviews(_label, _optionsTableView);
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            ContentView.AddConstraints(
                _optionsTableView.Below(_label, DefaultPadding),
                _optionsTableView.AtLeadingOf(ContentView),
                _optionsTableView.AtRightOf(ContentView),
                _optionsTableView.AtBottomOf(ContentView),
                // using _label.ToLeftOf doesn't seem to work properly. Using _label.AtRightOf instead.
                _label.AtRightOf(ContentView, DefaultPadding),
                _label.AtLeftOf(ContentView, DefaultPadding),
                _label.AtTopOf(ContentView, DefaultPadding));
        }

        protected override void Bind()
        {
            var set = this.CreateBindingSet<OptionCell, OptionRowModel>();
            set.Bind(_label)
                .To(vm => vm.Label);
            set.Bind(_tableViewSource)
                .To(vm => vm.Options);
            set.Bind(_tableViewSource)
                .For(v => v.CheckedRow)
                .To(vm => vm.SelectedOption);
            set.Bind(_tableViewSource)
                .For(v => v.SelectionChangedCommand)
                .To(vm => vm.SelectionOptionCommand);
            set.Apply();
        }
    }
}