using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using FloatLabeledEntry;
using FluentTableView;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace QuickList.Views
{
    public class TextFieldCell : MvxFluentTableViewCell
    {
        public static readonly NSString Key = new NSString("FloatingLabelTextFieldCell");
        
        private const int DefaultPadding = 16;
        private readonly UILabel _label = new UILabel
        {
            Lines = 0
        };

        public string? Label
        {
            get => _label.Text;
            set
            {
                _label.Text = value;
                ResetConstraints();
            }
        }
        
        
        private readonly FloatLabeledTextField _textField = new FloatLabeledTextField(CGRect.Empty);
        
        private FluentLayout[]? _labelConstraints;
        private FluentLayout[] LabelConstraints
        {
            get
            {
                return _labelConstraints ??= new[]
                {
                    _label.AtLeftOf(ContentView, DefaultPadding),
                    _label.AtTopOf(ContentView, DefaultPadding),
                    _label.AtRightOf(ContentView, DefaultPadding),
                    _textField.Below(_label, DefaultPadding),
                    _textField.AtLeftOf(ContentView, DefaultPadding),
                    _textField.AtBottomOf(ContentView, DefaultPadding),
                    _textField.AtRightOf(ContentView, DefaultPadding)
                };
            }
        }

        private FluentLayout[]? _noLabelConstraints;
        private FluentLayout[] NoLabelConstraints
        {
            get
            {
                return _noLabelConstraints ??= new[]
                {
                    _textField.AtLeftOf(ContentView, DefaultPadding),
                    _textField.AtTopOf(ContentView, DefaultPadding),
                    _textField.AtRightOf(ContentView, DefaultPadding),
                    _textField.AtBottomOf(ContentView, DefaultPadding)
                };
            }
        }
        
        public TextFieldCell(IntPtr handle) : base(handle)
        {
            _textField.FloatingLabelActiveTextColor = UIColor.FromName("Primary");
        }

        protected override void SetupConstraints()
        {
            ContentView.AddSubviews(_label, _textField);
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            ResetConstraints();
        }

        protected void ResetConstraints()
        {
            ContentView.RemoveConstraints(NoLabelConstraints);
            ContentView.RemoveConstraints(LabelConstraints);
            ContentView.AddConstraints(string.IsNullOrEmpty(_label.Text) ? NoLabelConstraints : LabelConstraints);
            ContentView.SetNeedsUpdateConstraints();
        }

        protected override void Bind()
        {
            var set = this.CreateBindingSet<TextFieldCell, TextEntryRowModel>();
            set.Bind(_textField)
                .For(t => t.Placeholder)
                .To(vm => vm.Hint);
            set.Bind(_textField)
                .For(t => t.Text)
                .To(vm => vm.Text);
            set.Bind(this)
                .For(v => v.Label)
                .To(vm => vm.Label);
            set.Apply();
        }
    }
}