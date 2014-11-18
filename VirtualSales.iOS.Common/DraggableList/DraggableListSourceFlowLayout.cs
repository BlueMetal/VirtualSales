using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace VirtualSales.iOS.DraggableList
{
    // the source just allows you to drag an item over a parent surface, but not rearrange in the list
    [Register("DraggableListSourceFlowLayout")]
    public class DraggableListSourceFlowLayout : DraggableListFlowLayoutBase
    {
        public DraggableListTargetFlowLayout Target { get; set; }
        public UIView DragSurface { get; set; }

        public DraggableListSourceFlowLayout(IntPtr handle)
            : base(handle)
        {
        }

        protected override void HandleLongPressGesture()
        {
            switch (LongPressGestureRecognizer.State)
            {
                case UIGestureRecognizerState.Began:
                    {
                        // we try to grab the current seelected item and draw a floating copy of it on the Drag Surface
                        // the copy is just an image of it using RasterizedImage
                        var currentIndexPath = CollectionView.IndexPathForItemAtPoint(LongPressGestureRecognizer.LocationInView(CollectionView));

                        SelectedItemIndexPath = currentIndexPath;
                        if (SelectedItemIndexPath == null)
                            return;

                        if (!DataSource.CanMoveItemAtIndexPath(SelectedItemIndexPath))
                            return;

                        var collectionViewCell = CollectionView.CellForItem(SelectedItemIndexPath);

                        var tpoint = DragSurface.ConvertPointFromView(collectionViewCell.Frame.Location, CollectionView);
                        RectangleF frame = new RectangleF(tpoint.X, tpoint.Y, collectionViewCell.Frame.Size.Width, collectionViewCell.Frame.Size.Height);
                        CurrentView = new UIView(frame);

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
                        DragSurface.AddSubview(CurrentView); // add this to the top level view so that we can drag outside
                        CurrentViewCenter = CurrentView.Center;

                        OnWillBeginDraggingItem(SelectedItemIndexPath);

                        // we animate the drawing out of the floating copy
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
                        if (currentIndexPath == null || CurrentView == null)
                            return;
                        SelectedItemIndexPath = null;
                        CurrentViewCenter = PointF.Empty;

                        OnWillEndDraggingItem(currentIndexPath);

                        UIView.Animate(0.3, 0.0, UIViewAnimationOptions.BeginFromCurrentState,
                            () =>
                            {
                                var layoutAttributes = this.LayoutAttributesForItem(currentIndexPath);
                                CurrentView.Transform = CGAffineTransform.MakeScale(1.0f, 1.0f);
                                CurrentView.Center = CollectionView.ConvertPointToView(layoutAttributes.Center, DragSurface);
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

        protected override void HandlePanGesture()
        {
            switch (PanGestureRecognizer.State)
            {
                case UIGestureRecognizerState.Began:
                case UIGestureRecognizerState.Changed:
                    {
                        if (CurrentView == null) return;
                        PanTranslationInCollectionView = PanGestureRecognizer.TranslationInView(CollectionView);
                        var pt = CurrentView.Center = AddPoints(CurrentViewCenter, PanTranslationInCollectionView);
                        var viewCenter = DragSurface.ConvertPointToView(pt, Target.CollectionView);

                        // Logic on what to do when we go over the Target Collection View
                        if (Target != null && Target.CollectionView.Frame.Contains(viewCenter))
                        {

                            // clone the current item, because we will be placing it into the target
                            var result = DataSource.ItemAtIndexPath(SelectedItemIndexPath);

                            // get the path for where we are inserting the new item into the target
                            var path = Target.CollectionView.IndexPathForItemAtPoint(viewCenter);
                            if (path == null)
                            {
                                // there are a few cases to cover
                                // if the list is empty, we add it to the first index path
                                // if the list is not empty, we add it to the end of the list if we are dragging it over any position right of the right-most item 
                                //    this is a relevant case for when we are just building up the collection
                                var count = (Target.DataSource as IUICollectionViewDataSource).GetItemsCount(Target.CollectionView, 0);

                                if (count == 0)
                                {
                                    path = NSIndexPath.FromItemSection(0, 0);
                                }
                                else if (count > 0)
                                {
                                    var cell = Target.CollectionView.CellForItem(NSIndexPath.FromItemSection(count - 1, 0));
                                    if (viewCenter.X > cell.Frame.GetMaxX())
                                    {
                                        path = NSIndexPath.FromItemSection(count, 0);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    return;
                                }
                            }

                            // once we know where we are putting it into the target, we simply add it and commit that change
                            // if the user needs to reorder, start long press on the target list
                            Target.DataSource.ItemWillAddToIndexPath(path, result);

                            NSAction updates = () =>
                            {
                                Target.CollectionView.InsertItems(new[] { path });
                            };
                            UICompletionHandler completion = (bool finished) =>
                            {
                                Target.DataSource.ItemDidAddToIndexPath(path, result);
                            };
                            PanGestureRecognizer.Enabled = false;
                            PanGestureRecognizer.Enabled = true;
                            LongPressGestureRecognizer.Enabled = false;
                            LongPressGestureRecognizer.Enabled = true;

                            Target.CollectionView.PerformBatchUpdates(updates, completion);
                        }
                    }
                    break;
                case UIGestureRecognizerState.Cancelled:
                case UIGestureRecognizerState.Ended:
                    {
                    }
                    break;
                default:
                    break;
            }
        }
    }
}