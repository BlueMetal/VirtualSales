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
	[Register ("AnnotationsToolViewController")]
	partial class AnnotationsToolViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton toggleEllipseButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton toggleFreeDrawButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton toggleLineButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton toggleRectangleButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton undoButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (toggleRectangleButton != null) {
				toggleRectangleButton.Dispose ();
				toggleRectangleButton = null;
			}

			if (toggleEllipseButton != null) {
				toggleEllipseButton.Dispose ();
				toggleEllipseButton = null;
			}

			if (toggleLineButton != null) {
				toggleLineButton.Dispose ();
				toggleLineButton = null;
			}

			if (toggleFreeDrawButton != null) {
				toggleFreeDrawButton.Dispose ();
				toggleFreeDrawButton = null;
			}

			if (undoButton != null) {
				undoButton.Dispose ();
				undoButton = null;
			}
		}
	}
}
