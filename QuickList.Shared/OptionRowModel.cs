using System;
using System.Collections.Generic;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace QuickList
{
    public class OptionRowModel : MvxNotifyPropertyChanged, IRowModel
    {
        public object? DataContext { get; }

        private string _label;

        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public IList<string> Options { get; set; }

        private string? _selectedOption;
        public string? SelectedOption
        {
            get => _selectedOption;
            set => SetProperty(ref _selectedOption, value);
        }

        private IMvxCommand<string>? _selectedOptionCommand;
        public IMvxCommand<string> SelectionOptionCommand => _selectedOptionCommand ??= new MvxCommand<string>((option) =>
        {
            SelectedOption = option;
        });

        public OptionRowModel(
            string label,
            IList<string> options,
            object? dataContext = null)
        {
            Options = options;
            _label = label;
            DataContext = dataContext;
        }
    }
}
