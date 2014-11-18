using System;
using System.Drawing;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using VirtualSales.iOS.DraggableList;
using System.Collections.Generic;
using VirtualSales.Core.ViewModels.Tools;
using ReactiveUI;
using System.Linq;
using System.Collections.Specialized;

namespace VirtualSales.iOS.ViewControllers
{
    public partial class AvailableToolsViewController : UICollectionViewController, IDraggableDataSource
    {
        internal DraggableListSourceFlowLayout DraggableLayout
        {
            get { return (DraggableListSourceFlowLayout)Layout; }
        }

        public IReadOnlyList<IToolViewModel> AvailableTools
        {
            get; set;
        }

        public IReadOnlyList<IToolViewModel> SelectedTools
        {
            get; set;
        }

        public AvailableToolsViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            DraggableLayout.DataSource = this;
            DraggableLayout.SetupCollectionView();

            CollectionView.AllowsSelection = false;
            CollectionView.AllowsMultipleSelection = false;

            CollectionView.RegisterNibForCell(ToolCell.Nib, ToolCell.Key);
        }

        public override int NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override int GetItemsCount(UICollectionView collectionView, int section)
        {
            return AvailableTools.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell(ToolCell.Key, indexPath) as ToolCell;
            cell.Text = AvailableTools[indexPath.Item].Name;
            cell.SetBorder();
            return cell;
        }

        #region IDraggableDataSource Implementation
        public void ItemWillMoveToIndexPath(NSIndexPath fromIndexPath, NSIndexPath toIndexPath)
        {
            throw new NotImplementedException("Reordering is not supported for AvailableToolsViewController");
        }

        public void ItemDidMoveToIndexPath(NSIndexPath fromIndexPath, NSIndexPath toIndexPath)
        {
            throw new NotImplementedException("Reordering is not supported for AvailableToolsViewController");
        }

        public object ItemAtIndexPath(NSIndexPath path)
        {
            return AvailableTools[path.Item];
        }

        public void ItemWillRemoveFromIndexPath(NSIndexPath path)
        {
            throw new NotImplementedException("Removing items is not supported for AvailableToolsViewController");
        }

        public void ItemDidRemoveFromIndexPath(NSIndexPath path)
        {
            throw new NotImplementedException("Removing items is not supported for AvailableToolsViewController");
        }

        public void ItemWillAddToIndexPath(NSIndexPath path, object data)
        {
            throw new NotImplementedException("Adding items is not supported for AvailableToolsViewController");
        }

        public void ItemDidAddToIndexPath(NSIndexPath path, object data)
        {
            throw new NotImplementedException("Adding items is not supported for AvailableToolsViewController");
        }

        public bool CanMoveItemAtIndexPath(NSIndexPath path)
        {
            var o = AvailableTools[path.Item];
            return !SelectedTools.Contains(o);
        }
        #endregion
    }
}