using System;
namespace QuickList
{
    public static class QuickRowModelExtensions
    {
        public static QuickRowModelType RowModelType(this object obj)
        {
            return obj switch
            {
                SectionRowModel _ => QuickRowModelType.Section,
                SwitchRowModel _ => QuickRowModelType.Switch,
                TextEntryRowModel _ => QuickRowModelType.Text,
                CommandRowModel _ => QuickRowModelType.Command,
                ImageRowModel _ => QuickRowModelType.Image,
                LabelRowModel _ => QuickRowModelType.Label,
                OptionRowModel _ => QuickRowModelType.Option,
                _ => QuickRowModelType.Unknown
            };
        }
    }
}
