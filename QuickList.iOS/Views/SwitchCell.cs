using System;
using Cirrious.FluentLayouts.Touch;
using FluentTableView;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace QuickList.Views
{
    public class SwitchCell : MvxFluentTableViewCell
    {
        public static readonly NSString Key = new NSString("SwitchCell");
        
        private const int DefaultPadding = 16;
        private readonly UILabel _label = new UILabel()
        {
            Lines = 0
        };
        private readonly UISwitch _switch = new UISwitch();

        public SwitchCell(IntPtr handle) : base(handle)
        {
        }

        protected override void SetupConstraints()
        {
            ContentView.AddSubviews(_label, _switch);
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            ContentView.AddConstraints(
                _switch.WithSameCenterY(ContentView),
                _switch.AtRightOf(ContentView, DefaultPadding),
                // using _label.ToLeftOf doesn't seem to work properly. Using _label.AtRightOf instead.
                _label.AtRightOf(ContentView, DefaultPadding + _switch.Bounds.Width + DefaultPadding),
                _label.AtLeftOf(ContentView, DefaultPadding),
                _label.AtTopOf(ContentView, DefaultPadding),
                _label.AtBottomOf(ContentView, DefaultPadding));
        }

        protected override void Bind()
        {
            var set = this.CreateBindingSet<SwitchCell, SwitchRowModel>();
            set.Bind(_label)
                .To(vm => vm.Label);
            set.Bind(_switch)
                .To(vm => vm.IsTrue);
            set.Apply();
        }
    }
}