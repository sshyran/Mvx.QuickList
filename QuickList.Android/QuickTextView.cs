#nullable enable
using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace QuickList
{
    [Register("QuickList.QuickTextView")]
    public class QuickTextView : TextView
    {
        private bool _isBold;

        public bool IsBold
        {
            get => _isBold;
            set
            {
                _isBold = value;
                SetTypeface(null, _isBold ? TypefaceStyle.Bold : TypefaceStyle.Normal);
            }
        }

        protected QuickTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public QuickTextView(Context? context) : base(context)
        {
        }

        public QuickTextView(Context? context, IAttributeSet? attrs) : base(context, attrs)
        {
        }

        public QuickTextView(Context? context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public QuickTextView(Context? context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }
        
        
    }
}