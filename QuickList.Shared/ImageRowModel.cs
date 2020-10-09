using System;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace QuickList
{
    public class ImageRowModel : MvxNotifyPropertyChanged, IRowModel
    {
        public object? DataContext { get; }

        private string? _imageUrl;

        public string? ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        public IMvxCommand<string>? TapCommand { get; set; }

        public ImageRowModel(string imageUrl, IMvxCommand<string>? command, object? dataContext = null)
        {
            ImageUrl = imageUrl;
            TapCommand = command;
            DataContext = dataContext;
        }
    }
}
