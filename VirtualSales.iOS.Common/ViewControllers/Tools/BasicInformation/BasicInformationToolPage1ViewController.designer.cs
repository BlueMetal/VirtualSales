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
	[Register ("BasicInformationToolPage1ViewController")]
	partial class BasicInformationToolPage1ViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton dateOfBirthButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField firstNameTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISegmentedControl genderSegmentedControl { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField lastNameTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel numOfDependentsLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIStepper numOfDependentsStepper { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISwitch smokerSwitcher { get; set; }

		[Action ("genderSegmentedValueChanged:")]
		partial void genderSegmentedValueChanged (MonoTouch.Foundation.NSObject sender);

		[Action ("numOfDependentsValueChanged:")]
		partial void numOfDependentsValueChanged (MonoTouch.Foundation.NSObject sender);

		[Action ("smokerSwitchValueChanged:")]
		partial void smokerSwitchValueChanged (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (firstNameTextField != null) {
				firstNameTextField.Dispose ();
				firstNameTextField = null;
			}

			if (genderSegmentedControl != null) {
				genderSegmentedControl.Dispose ();
				genderSegmentedControl = null;
			}

			if (lastNameTextField != null) {
				lastNameTextField.Dispose ();
				lastNameTextField = null;
			}

			if (numOfDependentsLabel != null) {
				numOfDependentsLabel.Dispose ();
				numOfDependentsLabel = null;
			}

			if (numOfDependentsStepper != null) {
				numOfDependentsStepper.Dispose ();
				numOfDependentsStepper = null;
			}

			if (dateOfBirthButton != null) {
				dateOfBirthButton.Dispose ();
				dateOfBirthButton = null;
			}

			if (smokerSwitcher != null) {
				smokerSwitcher.Dispose ();
				smokerSwitcher = null;
			}
		}
	}
}
