using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace VirtualSales.iOS.DraggableList
{
    public static class UIViewExtensions
    {
        public static UIViewController GetViewController(this UIView view)
        {
            return (UIViewController)TraverseResponderChainForUIViewController(view);
        }

        private static object TraverseResponderChainForUIViewController(UIView view)
        {
            var nextResponder = view.NextResponder;
            if (nextResponder is UIViewController)
                return nextResponder;

            if (nextResponder is UIView)
                return TraverseResponderChainForUIViewController((UIView)nextResponder);

            return null;
        }

        public static UIView ExtendedHitTest(this UIView view, PointF point, UIEvent ev)
        {
            var collection = view.HitTest(point, ev).Superview.Subviews;
            foreach (var item in collection)
            {
                if (item.PointInside(view.ConvertPointToView(point, item), ev))
                {
                    return item;
                }
            }
            return null;
        }
    }
}