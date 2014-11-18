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
	[Register ("NavigationPaneViewController")]
	partial class NavigationPaneViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton invokeToolLibraryButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton logOutButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton provideFeedbackButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton showSettingsButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton upcomingMeetingsButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (invokeToolLibraryButton != null) {
				invokeToolLibraryButton.Dispose ();
				invokeToolLibraryButton = null;
			}

			if (provideFeedbackButton != null) {
				provideFeedbackButton.Dispose ();
				provideFeedbackButton = null;
			}

			if (showSettingsButton != null) {
				showSettingsButton.Dispose ();
				showSettingsButton = null;
			}

			if (logOutButton != null) {
				logOutButton.Dispose ();
				logOutButton = null;
			}

			if (upcomingMeetingsButton != null) {
				upcomingMeetingsButton.Dispose ();
				upcomingMeetingsButton = null;
			}
		}
	}
}
