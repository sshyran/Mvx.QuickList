using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.Widget;

namespace QuickList
{
    [Register("QuickList.QuickRadioButton")]
    public class QuickRadioButton : AppCompatRadioButton
    {
        
        protected QuickRadioButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public QuickRadioButton(Context context) : base(context)
        {
        }

        public QuickRadioButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public QuickRadioButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }
        
        public override void Toggle()
        {
            if (Checked) {
                if (Parent is RadioGroup radioGroup) {
                    radioGroup.ClearCheck();
                }
            } else {
                base.Toggle();
            }
        }
    }
}