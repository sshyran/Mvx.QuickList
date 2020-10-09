using System;
using MvvmCross.ViewModels;

namespace QuickList
{
    public class TextEntryRowModel : MvxNotifyPropertyChanged, IRowModel
    {
        public object? DataContext { get; }

        private string _hint;
        public string Hint
        {
            get => _hint;
            set => SetProperty(ref _hint, value);
        }

        private string? _label;
        public string? Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public TextEntryRowModel(string hint, string text, string? label, object? dataContext = null)
        {
            _hint = hint;
            _text = text;
            _label = label;
            DataContext = dataContext;
        }
    }
}
