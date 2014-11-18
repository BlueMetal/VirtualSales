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
	[Register ("BasicInformationToolPage3ViewController")]
	partial class BasicInformationToolPage3ViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField addr1TextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField addr2TextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField cityTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField stateTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField zipTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (addr1TextField != null) {
				addr1TextField.Dispose ();
				addr1TextField = null;
			}

			if (addr2TextField != null) {
				addr2TextField.Dispose ();
				addr2TextField = null;
			}

			if (cityTextField != null) {
				cityTextField.Dispose ();
				cityTextField = null;
			}

			if (stateTextField != null) {
				stateTextField.Dispose ();
				stateTextField = null;
			}

			if (zipTextField != null) {
				zipTextField.Dispose ();
				zipTextField = null;
			}
		}
	}
}
