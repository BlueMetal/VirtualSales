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
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField emailAddressTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel errorLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton loginButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField passwordTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (emailAddressTextField != null) {
				emailAddressTextField.Dispose ();
				emailAddressTextField = null;
			}

			if (errorLabel != null) {
				errorLabel.Dispose ();
				errorLabel = null;
			}

			if (passwordTextField != null) {
				passwordTextField.Dispose ();
				passwordTextField = null;
			}

			if (loginButton != null) {
				loginButton.Dispose ();
				loginButton = null;
			}
		}
	}
}
