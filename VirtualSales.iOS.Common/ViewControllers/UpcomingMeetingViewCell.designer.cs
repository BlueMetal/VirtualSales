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
	[Register ("UpcomingMeetingViewCell")]
	partial class UpcomingMeetingViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UIView ContainerBorderView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel datOfTheWeekLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel dayNumberLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel meetingIdentifierLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel monthAndYearLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel personLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel phoneNumberLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel statusLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel timeLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ContainerBorderView != null) {
				ContainerBorderView.Dispose ();
				ContainerBorderView = null;
			}

			if (datOfTheWeekLabel != null) {
				datOfTheWeekLabel.Dispose ();
				datOfTheWeekLabel = null;
			}

			if (dayNumberLabel != null) {
				dayNumberLabel.Dispose ();
				dayNumberLabel = null;
			}

			if (meetingIdentifierLabel != null) {
				meetingIdentifierLabel.Dispose ();
				meetingIdentifierLabel = null;
			}

			if (monthAndYearLabel != null) {
				monthAndYearLabel.Dispose ();
				monthAndYearLabel = null;
			}

			if (personLabel != null) {
				personLabel.Dispose ();
				personLabel = null;
			}

			if (timeLabel != null) {
				timeLabel.Dispose ();
				timeLabel = null;
			}

			if (statusLabel != null) {
				statusLabel.Dispose ();
				statusLabel = null;
			}

			if (phoneNumberLabel != null) {
				phoneNumberLabel.Dispose ();
				phoneNumberLabel = null;
			}
		}
	}
}
