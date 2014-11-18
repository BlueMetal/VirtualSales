using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace VirtualSales.iOS.DraggableList
{

    public class DraggableGestureRecognizerDelegate : UIGestureRecognizerDelegate
    {
        public DraggableListFlowLayoutBase Layout { get; private set; }
        public DraggableGestureRecognizerDelegate(DraggableListFlowLayoutBase layout)
        {
            Layout = layout;
        }
        public override bool ShouldBegin(UIGestureRecognizer recognizer)
        {
            if (recognizer == Layout.PanGestureRecognizer)
            {
                return Layout.SelectedItemIndexPath != null;
            }
            return true;
        }

        public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (gestureRecognizer == Layout.LongPressGestureRecognizer)
            {
                return Layout.PanGestureRecognizer == otherGestureRecognizer;
            }
            if (gestureRecognizer == Layout.PanGestureRecognizer)
            {
                return Layout.LongPressGestureRecognizer == otherGestureRecognizer;
            }
            return false;
        }
    }
}