using System;
using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace QuickList
{
    public class SectionRowModel : MvxNotifyPropertyChanged, IRowModel
    {
        public object? DataContext { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public IList<IRowModel> Rows { get; set; }

        public SectionRowModel(string title, IList<IRowModel> rows, object? dataContext = null)
        {
            _title = title;
            Rows = rows;
            DataContext = dataContext;
        }
    }
}
