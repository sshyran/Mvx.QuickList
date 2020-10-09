using System;
using CoreGraphics;
using UIKit;

namespace QuickList
{
    public class FullHeightTableView : UITableView
    {
        public override void ReloadData()
        {
            base.ReloadData();
            base.InvalidateIntrinsicContentSize();
            base.LayoutIfNeeded();
        }

        public override CGSize IntrinsicContentSize => new CGSize(ContentSize.Width, ContentSize.Height);
    }
}
