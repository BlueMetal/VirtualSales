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
	[Register ("TypesOfInsuranceToolPage1ViewController")]
	partial class TypesOfInsuranceToolPage1ViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextView termLifeTextView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView universalLifeTextView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView variableLifeTextView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView wholeLifeTextView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (wholeLifeTextView != null) {
				wholeLifeTextView.Dispose ();
				wholeLifeTextView = null;
			}

			if (variableLifeTextView != null) {
				variableLifeTextView.Dispose ();
				variableLifeTextView = null;
			}

			if (universalLifeTextView != null) {
				universalLifeTextView.Dispose ();
				universalLifeTextView = null;
			}

			if (termLifeTextView != null) {
				termLifeTextView.Dispose ();
				termLifeTextView = null;
			}
		}
	}
}
