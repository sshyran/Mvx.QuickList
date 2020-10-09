using System;
using MvvmCross.ViewModels;

namespace QuickList
{
    public class LabelRowModel : MvxNotifyPropertyChanged, IRowModel
    {
        public object? DataContext { get; }

        private string _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private bool _isBold;
        public bool IsBold
        {
            get => _isBold;
            set => SetProperty(ref _isBold, value);
        }

        public LabelRowModel(string label, bool isBold, object? dataContext = null)
        {
            _label = label;
            _isBold = isBold;
            DataContext = dataContext;
        }
    }
}
