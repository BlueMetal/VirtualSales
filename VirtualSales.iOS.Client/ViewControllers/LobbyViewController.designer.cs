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
	[Register ("LobbyViewController")]
	partial class LobbyViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton connectButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton disconnectButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel errorLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lobbyStatusLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField meetingIdTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (connectButton != null) {
				connectButton.Dispose ();
				connectButton = null;
			}

			if (lobbyStatusLabel != null) {
				lobbyStatusLabel.Dispose ();
				lobbyStatusLabel = null;
			}

			if (meetingIdTextField != null) {
				meetingIdTextField.Dispose ();
				meetingIdTextField = null;
			}

			if (disconnectButton != null) {
				disconnectButton.Dispose ();
				disconnectButton = null;
			}

			if (errorLabel != null) {
				errorLabel.Dispose ();
				errorLabel = null;
			}
		}
	}
}
