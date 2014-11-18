using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace VirtualSales.iOS.DraggableList
{
    // the source just allows you to drag an item over a parent surface, but not rearrange in the list
    // you can drop items onto the list and rearrange this target flow layout and only drag items inside of it
    public interface IDraggableDataSource
    {
        void ItemWillMoveToIndexPath(NSIndexPath fromIndexPath, NSIndexPath toIndexPath);
        void ItemDidMoveToIndexPath(NSIndexPath fromIndexPath, NSIndexPath toIndexPath);

        object ItemAtIndexPath(NSIndexPath path);
        void ItemWillRemoveFromIndexPath(NSIndexPath path);
        void ItemDidRemoveFromIndexPath(NSIndexPath path);
        void ItemWillAddToIndexPath(NSIndexPath path, object data);
        void ItemDidAddToIndexPath(NSIndexPath path, object data);
        bool CanMoveItemAtIndexPath(NSIndexPath path);
    }
}