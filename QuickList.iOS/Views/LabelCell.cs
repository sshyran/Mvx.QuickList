using System;
using Cirrious.FluentLayout.Extensions;
using Cirrious.FluentLayouts.Touch;
using FluentTableView;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace QuickList.Views
{
    public class LabelCell : MvxFluentTableViewCell
    {
        public static readonly NSString Key = new NSString("LabelCell");
        
        private const int DefaultPadding = 16;
        private readonly UILabel _label = new UILabel()
        {
            Lines = 0
        };

        private bool _isBold;

        public bool IsBold
        {
            get => _isBold;
            set
            {
                _isBold = value;
                _label.Font = _isBold ? UIFont.BoldSystemFontOfSize(17) : UIFont.SystemFontOfSize(17);
            }
        }

        public LabelCell(IntPtr handle) : base(handle)
        {
        }

        protected override void SetupConstraints()
        {
            ContentView.AddSubviews(_label);
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            ContentView.AddConstraints(
                _label.InsideOf(ContentView, DefaultPadding)
            );
        }

        protected override void Bind()
        {
            var set = this.CreateBindingSet<LabelCell, LabelRowModel>();
            set.Bind(_label)
                .To(vm => vm.Label);
            set.Bind(this)
                .For(v => v.IsBold)
                .To(vm => vm.IsBold);
            set.Apply();
        }
    }
}