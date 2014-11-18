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
	[Register ("ClientInfoViewController")]
	partial class ClientInfoViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField firstNameTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (firstNameTextField != null) {
				firstNameTextField.Dispose ();
				firstNameTextField = null;
			}
		}
	}
}
