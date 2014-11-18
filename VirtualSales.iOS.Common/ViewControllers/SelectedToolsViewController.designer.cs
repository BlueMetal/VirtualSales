// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace VirtualSales.iOS.ViewControllers
{
	[Register ("SelectedToolsViewController")]
	partial class SelectedToolsViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel dragItemsIntoHereLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (dragItemsIntoHereLabel != null) {
				dragItemsIntoHereLabel.Dispose ();
				dragItemsIntoHereLabel = null;
			}
		}
	}
}
