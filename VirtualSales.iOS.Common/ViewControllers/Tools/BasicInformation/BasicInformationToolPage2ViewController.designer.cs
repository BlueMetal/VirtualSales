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
	[Register ("BasicInformationToolPage2ViewController")]
	partial class BasicInformationToolPage2ViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField amountRequestedTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISegmentedControl annualIncomeGrowthSegment { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField annualIncomeTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField existingCoverageTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIStepper retirementAgeStepper { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField retirementAgeTextField { get; set; }

		[Action ("annualIncomeGrowthValueChanged:")]
		partial void annualIncomeGrowthValueChanged (MonoTouch.Foundation.NSObject sender);

		[Action ("retirementAgeValueChanged:")]
		partial void retirementAgeValueChanged (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (annualIncomeGrowthSegment != null) {
				annualIncomeGrowthSegment.Dispose ();
				annualIncomeGrowthSegment = null;
			}

			if (amountRequestedTextField != null) {
				amountRequestedTextField.Dispose ();
				amountRequestedTextField = null;
			}

			if (existingCoverageTextField != null) {
				existingCoverageTextField.Dispose ();
				existingCoverageTextField = null;
			}

			if (retirementAgeStepper != null) {
				retirementAgeStepper.Dispose ();
				retirementAgeStepper = null;
			}

			if (retirementAgeTextField != null) {
				retirementAgeTextField.Dispose ();
				retirementAgeTextField = null;
			}

			if (annualIncomeTextField != null) {
				annualIncomeTextField.Dispose ();
				annualIncomeTextField = null;
			}
		}
	}
}
