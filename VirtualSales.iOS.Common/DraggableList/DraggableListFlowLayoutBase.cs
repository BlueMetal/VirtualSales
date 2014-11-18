using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreAnimation;

namespace VirtualSales.iOS.DraggableList
{
    public abstract class DraggableListFlowLayoutBase : UICollectionViewFlowLayout
    {
        public UIPanGestureRecognizer PanGestureRecognizer { get; protected set; }
        public UILongPressGestureRecognizer LongPressGestureRecognizer { get; protected set; }

        public event EventHandler<NSIndexPath> WillBeginDraggingItem;
        public event EventHandler<NSIndexPath> DidBeginDraggingItem;
        public event EventHandler<NSIndexPath> WillEndDraggingItem;
        public event EventHandler<NSIndexPath> DidEndDraggingItem;

        protected void OnWillBeginDraggingItem(NSIndexPath path)
        {
            var ev = WillBeginDraggingItem;
            if (ev != null)
            {
                ev(this, path);
            }
        }
        protected void OnDidBegingDraggingItem(NSIndexPath path)
        {
            var ev = DidBeginDraggingItem;
            if (ev != null)
            {
                ev(this, path);
            }
        }
        protected void OnWillEndDraggingItem(NSIndexPath path)
        {
            var ev = WillEndDraggingItem;
            if (ev != null)
            {
                ev(this, path);
            }
        }
        protected void OnDidEndDraggingItem(NSIndexPath path)
        {
            var ev = DidEndDraggingItem;
            if (ev != null)
            {
                ev(this, path);
            }
        }

        public float ScrollingSpeed { get; set; }
        public UIEdgeInsets ScrollingTriggerEdgeInsets { get; set; }
        public NSIndexPath SelectedItemIndexPath { get; protected set; }
        public IDraggableDataSource DataSource { get; set; }
        public UIView CurrentView { get; set; }
        public PointF CurrentViewCenter { get; set; }
        public PointF PanTranslationInCollectionView { get; set; }
        public CADisplayLink DisplayLink { get; set; }

        protected UIImage RastertizedImage(UICollectionViewCell cell)
        {
            UIGraphics.BeginImageContextWithOptions(cell.Bounds.Size, cell.Opaque, 0.0f);
            cell.Layer.RenderInContext(UIGraphics.GetCurrentContext());
            var img = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return img;
        }

        protected DraggableListFlowLayoutBase(IntPtr handle) : base(handle)
        {
            SetDefaults();
            AddObserver(this, new NSString("collectionView"), NSKeyValueObservingOptions.New, System.IntPtr.Zero);
        }

        private void SetDefaults()
        {
            ScrollingSpeed = 300.0f;
            var inset = 75.0f;
            ScrollingTriggerEdgeInsets = new UIEdgeInsets(inset, inset, inset, inset);
        }

        protected PointF AddPoints(PointF point1, PointF point2)
        {
            return new PointF(point1.X + point2.X, point1.Y + point2.Y);
        }
        private void HandleApplicationWillResignActive(NSNotification notification)
        {
            PanGestureRecognizer.Enabled = false;
            PanGestureRecognizer.Enabled = true;
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            if (keyPath == "collectionView")
            {
                if (CollectionView != null)
                    this.SetupCollectionView();
                else
                    HandleCollectionViewNull();
            }
        }

        protected virtual void HandleCollectionViewNull() { }
        protected abstract void HandleLongPressGesture();
        protected abstract void HandlePanGesture();

        public void SetupCollectionView()
        {
            if (LongPressGestureRecognizer != null)
            {
                CollectionView.RemoveGestureRecognizer(LongPressGestureRecognizer);
            }
            if (PanGestureRecognizer != null)
            {
                CollectionView.RemoveGestureRecognizer(PanGestureRecognizer);
            }

            LongPressGestureRecognizer = new UILongPressGestureRecognizer(HandleLongPressGesture);
            LongPressGestureRecognizer.Delegate = new DraggableGestureRecognizerDelegate(this);

            foreach (var gestureRecognizer in CollectionView.GestureRecognizers)
            {
                if (gestureRecognizer is UILongPressGestureRecognizer)
                {
                    gestureRecognizer.RequireGestureRecognizerToFail(LongPressGestureRecognizer);
                }
            }

            CollectionView.AddGestureRecognizer(LongPressGestureRecognizer);

            PanGestureRecognizer = new UIPanGestureRecognizer(HandlePanGesture);
            PanGestureRecognizer.Delegate = new DraggableGestureRecognizerDelegate(this);

            CollectionView.AddGestureRecognizer(PanGestureRecognizer);

            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillResignActiveNotification, HandleApplicationWillResignActive);
        }
    }
}