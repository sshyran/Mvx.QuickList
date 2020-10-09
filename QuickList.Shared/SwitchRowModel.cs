using System;
using MvvmCross.ViewModels;

namespace QuickList
{
    public class SwitchRowModel : MvxNotifyPropertyChanged, IRowModel
    {
        public object? DataContext { get; }

        private string _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private bool _isTrue;
        public bool IsTrue
        {
            get => _isTrue;
            set => SetProperty(ref _isTrue, value);
        }

        public SwitchRowModel(bool isTrue, string label, object? dataContext)
        {
            _isTrue = isTrue;
            _label = label;
            DataContext = dataContext;
        }
    }
}
