using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using System.Drawing;

namespace VirtualSales.iOS.DraggableList
{
    // you can drop items onto the list and rearrange this target flow layout and only drag items inside of it
    [Register("DraggableListTargetFlowLayout")]
    public class DraggableListTargetFlowLayout : DraggableListFlowLayoutBase
    {
        private Dictionary<CADisplayLink, NSDictionary> userInfos = new Dictionary<CADisplayLink, NSDictionary>();

        public DraggableListTargetFlowLayout(IntPtr handle) :base(handle)
        {
        }

        // enum used to indicate scrolling direction when item dragged to edges
        private enum ScrollingDirection
        {
            ScrollingDirectionUnknown = 0,
            ScrollingDirectionUp,
            ScrollingDirectionDown,
            ScrollingDirectionLeft,
            ScrollingDirectionRight
        };


        // stops automatic scrolling
        private void InvalidatesScrollTimer()
        {
            if (DisplayLink == null)
                return;

            if (DisplayLink.Paused)
            {
                DisplayLink.Invalidate();
            }
            userInfos.Remove(DisplayLink);
            DisplayLink = null;
        }

        protected override void HandleLongPressGesture()
        {
            switch (LongPressGestureRecognizer.State)
            {
                case UIGestureRecognizerState.Began:
                    {
                        // we get the current item and draw a floating copy on it on the Collection View, so that it cannot be taken outside the bounds of the collection view
                        // we get the copy usign RasterizedImage
                        var currentIndexPath = CollectionView.IndexPathForItemAtPoint(LongPressGestureRecognizer.LocationInView(CollectionView));
                        SelectedItemIndexPath = currentIndexPath;
                        if (SelectedItemIndexPath == null)
                            return;

                        if (!DataSource.CanMoveItemAtIndexPath(SelectedItemIndexPath))
                            return;

                        var collectionViewCell = CollectionView.CellForItem(SelectedItemIndexPath);
                        CurrentView = new UIView(collectionViewCell.Frame);

                        collectionViewCell.Highlighted = true;
                        var highlightedImageView = new UIImageView(RastertizedImage(collectionViewCell));
                        highlightedImageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                        highlightedImageView.Alpha = 1.0f;

                        collectionViewCell.Highlighted = false;
                        var imageView = new UIImageView(RastertizedImage(collectionViewCell));
                        imageView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                        imageView.Alpha = 0.0f;

                        CurrentView.AddSubview(imageView);
                        CurrentView.AddSubview(highlightedImageView);
                        CollectionView.AddSubview(CurrentView);
                        CurrentViewCenter = CurrentView.Center;

                        OnWillBeginDraggingItem(SelectedItemIndexPath);

                        // animate the floating copy into existence
                        UIView.Animate(0.3, 0.0, UIViewAnimationOptions.BeginFromCurrentState,
                            () =>
                            {
                                CurrentView.Transform = CGAffineTransform.MakeScale(1.1f, 1.1f);
                                highlightedImageView.Alpha = 0.0f;
                                imageView.Alpha = 1.0f;
                            },
                            () =>
                            {
                                highlightedImageView.RemoveFromSuperview();
                                OnDidBegingDraggingItem(SelectedItemIndexPath);
                            });
                        InvalidateLayout();
                    }
                    break;
                case UIGestureRecognizerState.Cancelled:
                case UIGestureRecognizerState.Ended:
                    {
                        var currentIndexPath = SelectedItemIndexPath;
                        if (currentIndexPath == null)
                            return;
                        SelectedItemIndexPath = null;
                        CurrentViewCenter = PointF.Empty;

                        OnWillEndDraggingItem(currentIndexPath);

                        UIView.Animate(0.3, 0.0, UIViewAnimationOptions.BeginFromCurrentState,
                            () =>
                            {
                                var layoutAttributes = LayoutAttributesForItem(currentIndexPath);
                                CurrentView.Transform = CGAffineTransform.MakeScale(1.0f, 1.0f);
                                CurrentView.Center = layoutAttributes.Center;
                            },
                            () =>
                            {
                                CurrentView.RemoveFromSuperview();
                                CurrentView = null;
                                InvalidateLayout();

                                OnDidEndDraggingItem(currentIndexPath);
                            });

                    }
                    break;
            }
        }

        // sets up the scrolling in the correct direction
        // utilizes CSDisplayLink, a timer synchronized to the display refresh rate, to execute the smooth automated scrolling 
        private void SetupScrollTimerInDirection(ScrollingDirection direction)
        {
            if (DisplayLink != null && !DisplayLink.Paused)
            {
                var userInfo = userInfos[DisplayLink];
                var scrollingDirection = userInfo[ScrollingDirectionKey];
                var number = (NSNumber)NSNumber.FromObject(scrollingDirection);

                ScrollingDirection oldDirection = (ScrollingDirection)number.Int32Value;

                if (direction == oldDirection)
                {
                    return;
                }
            }
            InvalidatesScrollTimer();

            DisplayLink = CADisplayLink.Create(HandleScroll);
            userInfos.Add(DisplayLink, NSDictionary.FromObjectAndKey(NSNumber.FromInt32((int)direction), new NSString(ScrollingDirectionKey)));

            DisplayLink.AddToRunLoop(NSRunLoop.Main, NSRunLoop.NSRunLoopCommonModes);
        }

        // some methods to verify whether a point in in the scrolling bounds.
        private bool IsPointInTopScrollingBounds(PointF pt)
        {
            var isAboveInset = pt.Y < (CollectionView.Bounds.GetMinY() + ScrollingTriggerEdgeInsets.Top);
            var isBelowTop = pt.Y > CollectionView.Bounds.GetMinY();

            return isAboveInset && isBelowTop;
        }
        private bool IsPointInBottomScrollingBounds(PointF pt)
        {
            var isBelowInset = pt.Y > (CollectionView.Bounds.GetMaxY() - ScrollingTriggerEdgeInsets.Bottom);
            var isAboveBottom = pt.Y < CollectionView.Bounds.GetMaxY();

            return isBelowInset && isAboveBottom;
        }
        private bool IsInHorizontalBounds(PointF pt)
        {
            var result = pt.X < CollectionView.Bounds.GetMaxX() && pt.X > CollectionView.Bounds.GetMinX();
            return result;
        }
        private bool IsInVerticalBounds(PointF pt)
        {
            var result = pt.Y < CollectionView.Bounds.GetMaxY() && pt.Y > CollectionView.Bounds.GetMinY();
            return result;
        }
        private bool IsPointInLeftScrollingBounds(PointF pt)
        {
            var isLeftOfInset = pt.X < (CollectionView.Bounds.GetMinX() + ScrollingTriggerEdgeInsets.Left);
            var isRightOfEdge = pt.X > CollectionView.Bounds.GetMinX();
            return isLeftOfInset && isRightOfEdge;
        }
        private bool IsPointInRightScrollingBounds(PointF pt)
        {
            var isRightOfInset = pt.X > (CollectionView.Bounds.GetMaxX() - ScrollingTriggerEdgeInsets.Right);
            var isLeftOfEdge = pt.X < CollectionView.Bounds.GetMaxX();
            return isRightOfInset && isLeftOfEdge;
        }

        protected override void HandlePanGesture()
        {
            switch (PanGestureRecognizer.State)
            {
                case UIGestureRecognizerState.Began:
                case UIGestureRecognizerState.Changed:
                    {
                        // when an item is panned we check whether it is in the scrolling bound and scroll if we have to
                        PanTranslationInCollectionView = PanGestureRecognizer.TranslationInView(CollectionView);
                        var viewCenter = CurrentView.Center = AddPoints(CurrentViewCenter, PanTranslationInCollectionView);

                        var newIndexPath = CollectionView.IndexPathForItemAtPoint(viewCenter);
                        if (newIndexPath == null)
                        {
                            return;
                        }

                        InvalidateLayoutIfNecesary();
                        switch (ScrollDirection)
                        {

                            case UICollectionViewScrollDirection.Vertical:
                                {
                                    if (IsPointInTopScrollingBounds(viewCenter) && IsInHorizontalBounds(viewCenter))
                                    {
                                        SetupScrollTimerInDirection(ScrollingDirection.ScrollingDirectionUp);
                                    }
                                    else if (IsPointInBottomScrollingBounds(viewCenter) && IsInHorizontalBounds(viewCenter))
                                    {
                                        SetupScrollTimerInDirection(ScrollingDirection.ScrollingDirectionDown);
                                    }
                                    else
                                    {
                                        InvalidatesScrollTimer();
                                    }
                                }
                                break;
                            case UICollectionViewScrollDirection.Horizontal:
                                {
                                    if (IsPointInLeftScrollingBounds(viewCenter) && IsInVerticalBounds(viewCenter))
                                    {
                                        SetupScrollTimerInDirection(ScrollingDirection.ScrollingDirectionLeft);
                                    }
                                    else if (IsPointInRightScrollingBounds(viewCenter) && IsInHorizontalBounds(viewCenter))
                                    {
                                        SetupScrollTimerInDirection(ScrollingDirection.ScrollingDirectionRight);
                                    }
                                    else
                                    {
                                        InvalidatesScrollTimer();
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case UIGestureRecognizerState.Cancelled:
                case UIGestureRecognizerState.Ended:
                    {
                        InvalidatesScrollTimer();
                    }
                    break;
                default:
                    break;
            }
        }

        public static readonly string ScrollingDirectionKey = "LX_scrollingdirection";
        public static readonly float LX_FRAMES_PER_SECOND = 60.0f;

        // actual scrolling logic that gets called by the CADisplayInfo timer
        // only matters if we have an active scroll animation command
        public void HandleScroll()
        {
            if (DisplayLink == null)
                return;
            var userInfo = userInfos[DisplayLink];
            var scrollingDirection = userInfo[ScrollingDirectionKey];
            var number = (NSNumber)NSNumber.FromObject(scrollingDirection);

            ScrollingDirection direction = (ScrollingDirection)(number.Int32Value);
            if (direction == ScrollingDirection.ScrollingDirectionUnknown)
                return;

            var frameSize = CollectionView.Bounds.Size;
            var contentSize = CollectionView.ContentSize;
            var contentOffset = CollectionView.ContentOffset;
            var distance = (float)Math.Round(ScrollingSpeed / LX_FRAMES_PER_SECOND);
            var translation = new PointF();

            switch (direction)
            {
                case ScrollingDirection.ScrollingDirectionUp:
                    distance = -distance;
                    var minY = 0.0f;
                    if ((contentOffset.Y + distance) <= minY)
                    {
                        distance = -contentOffset.Y;
                    }

                    translation = new PointF(0.0f, distance);
                    break;
                case ScrollingDirection.ScrollingDirectionDown:
                    float maxY = Math.Max(contentSize.Height, frameSize.Height) - frameSize.Height;

                    if ((contentOffset.Y + distance) >= maxY)
                    {
                        distance = maxY - contentOffset.Y;
                    }

                    translation = new PointF(0.0f, distance);
                    break;
                case ScrollingDirection.ScrollingDirectionLeft:
                    distance = -distance;
                    float minX = 0.0f;

                    if ((contentOffset.X + distance) <= minX)
                    {
                        distance = -contentOffset.X;
                    }

                    translation = new PointF(distance, 0.0f);
                    break;
                case ScrollingDirection.ScrollingDirectionRight:
                    float maxX = Math.Max(contentSize.Width, frameSize.Width) - frameSize.Width;

                    if ((contentOffset.X + distance) >= maxX)
                    {
                        distance = maxX - contentOffset.X;
                    }

                    translation = new PointF(distance, 0.0f);
                    break;
                default:
                    break;
            }

            CurrentViewCenter = AddPoints(CurrentViewCenter, translation);
            if (CurrentView != null)
            {
                CurrentView.Center = AddPoints(CurrentViewCenter, PanTranslationInCollectionView);
            }
            CollectionView.ContentOffset = AddPoints(contentOffset, translation);
        }

        // figures out if the item in the list needs to be redrawn into a different position and does it if the need is there
        private void InvalidateLayoutIfNecesary()
        {
            var newIndexPath = CollectionView.IndexPathForItemAtPoint(CurrentView.Center);
            var previousIndexPath = SelectedItemIndexPath;

            if ((newIndexPath == null) || newIndexPath == previousIndexPath)
            {
                return;
            }

            SelectedItemIndexPath = newIndexPath;

            DataSource.ItemWillMoveToIndexPath(previousIndexPath, newIndexPath);
            NSAction action = () =>
            {
                CollectionView.DeleteItems(new[] { previousIndexPath });
                CollectionView.InsertItems(new[] { newIndexPath });
            };
            UICompletionHandler completion = (bool finished) =>
            {
                DataSource.ItemDidMoveToIndexPath(previousIndexPath, newIndexPath);
            };
            CollectionView.PerformBatchUpdates(action, completion);
        }
        protected override void HandleCollectionViewNull()
        {
            this.InvalidatesScrollTimer();
        }
    }
}