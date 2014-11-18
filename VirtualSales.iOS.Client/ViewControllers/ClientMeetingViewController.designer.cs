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
	[Register ("ClientMeetingViewController")]
	partial class ClientMeetingViewController
	{
		[Outlet]
		VirtualSales.iOS.Annotations.AnnotationView annotationView { get; set; }

		[Action ("unwind:")]
		partial void unwind (MonoTouch.UIKit.UIStoryboardSegue segue);
		
		void ReleaseDesignerOutlets ()
		{
			if (annotationView != null) {
				annotationView.Dispose ();
				annotationView = null;
			}
		}
	}
}
