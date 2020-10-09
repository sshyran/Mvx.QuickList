using System;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace QuickList
{
    public class CommandRowModel : MvxNotifyPropertyChanged, IRowModel
    {
        public object? DataContext { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _enabled = true;
        public bool Enabled
        {
            get => _enabled;
            set => SetProperty(ref _enabled, value);
        }

        public IMvxCommand Command { get; set; }

        public CommandRowModel(string title, IMvxCommand command, object? dataContext = null)
        {
            _title = title;
            Command = command;
            DataContext = dataContext;
        }
    }
}
