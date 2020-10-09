using System;
using Cirrious.FluentLayout.Extensions;
using Cirrious.FluentLayouts.Touch;
using FFImageLoading.Cross;
using FluentTableView;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace QuickList.Views
{
    public class ImageCell: MvxFluentTableViewCell
    {
        public static readonly NSString Key = new NSString("ImageCell");
        
        private const int DefaultPadding = 16;
        private readonly MvxCachedImageView _imageView = new MvxCachedImageView()
        {
            ContentMode = UIViewContentMode.ScaleAspectFit
        };
        
        public ImageCell(IntPtr handle) : base(handle)
        {
        }

        protected override void SetupConstraints()
        {
            ContentView.AddSubviews(_imageView);
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            ContentView.AddConstraints(
                _imageView.InsideOf(ContentView, DefaultPadding)
                );
        }

        protected override void Bind()
        {
            var set = this.CreateBindingSet<ImageCell, ImageRowModel>();
            set.Bind(_imageView)
                .For(v => v.ImagePath)
                .To(vm => vm.ImageUrl);
            set.Apply();
        }
    }
}