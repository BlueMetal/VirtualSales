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
	[Register ("MeetingViewController")]
	partial class MeetingViewController
	{
		[Outlet]
		VirtualSales.iOS.Annotations.AnnotationView annotationView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton createMeetingPDFButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton endMeetingButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton hamburgerButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView presentationView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton startMeetingButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView toolboxView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (annotationView != null) {
				annotationView.Dispose ();
				annotationView = null;
			}

			if (createMeetingPDFButton != null) {
				createMeetingPDFButton.Dispose ();
				createMeetingPDFButton = null;
			}

			if (endMeetingButton != null) {
				endMeetingButton.Dispose ();
				endMeetingButton = null;
			}

			if (hamburgerButton != null) {
				hamburgerButton.Dispose ();
				hamburgerButton = null;
			}

			if (presentationView != null) {
				presentationView.Dispose ();
				presentationView = null;
			}

			if (startMeetingButton != null) {
				startMeetingButton.Dispose ();
				startMeetingButton = null;
			}

			if (toolboxView != null) {
				toolboxView.Dispose ();
				toolboxView = null;
			}
		}
	}
}
