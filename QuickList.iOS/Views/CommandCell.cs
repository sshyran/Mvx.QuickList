using System;
using Cirrious.FluentLayouts.Touch;
using FluentTableView;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace QuickList.Views
{
    public class CommandCell : MvxFluentTableViewCell
    {
        public static readonly NSString Key = new NSString("CommandCell");
        
        private readonly UIButton _button = new UIButton();

        public string? Title
        {
            get => _button.TitleLabel.Text;
            set => _button.SetTitle(value, UIControlState.Normal);
        }
        
        public CommandCell(IntPtr handle) : base(handle)
        {
            _button.BackgroundColor = UIColor.FromName("Primary");
        }

        protected override void SetupConstraints()
        {
            ContentView.AddSubviews(_button);
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            ContentView.AddConstraints(_button.AtLeftOf(ContentView));
            ContentView.AddConstraints(_button.Height().EqualTo(44));
        }

        protected override void Bind()
        {
            var set = this.CreateBindingSet<CommandCell, CommandRowModel>();
            set.Bind(_button)
                .To(vm => vm.Command);
            set.Bind(this)
                .For(v => v.Title)
                .To(vm => vm.Title);
            set.Bind(_button)
                .For(v => v.Enabled)
                .To(vm => vm.Enabled);
            set.Apply();
        }
    }
}