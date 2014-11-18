// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace VirtualSales.iOS
{
	[Register ("PresentationViewController")]
	partial class PresentationViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIView navigationView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton nextButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton prevButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (nextButton != null) {
				nextButton.Dispose ();
				nextButton = null;
			}

			if (prevButton != null) {
				prevButton.Dispose ();
				prevButton = null;
			}

			if (navigationView != null) {
				navigationView.Dispose ();
				navigationView = null;
			}
		}
	}
}
