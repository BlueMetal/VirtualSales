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
	[Register ("LifetimeEconomicValueToolPage1ViewController")]
	partial class LifetimeEconomicValueToolPage1ViewController
	{
		[Outlet]
		MonoTouch.UIKit.UISegmentedControl annualIncomeGrowthSegment { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField annualIncomeTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton dateOfBirthButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField existingCoverageTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIStepper retirementAgeStepper { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField retirementAgeTextField { get; set; }

		[Action ("annualIncomeGrowthValueChanged:")]
		partial void annualIncomeGrowthValueChanged (MonoTouch.Foundation.NSObject sender);

		[Action ("retirementAgeStepperValueChanged:")]
		partial void retirementAgeStepperValueChanged (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (dateOfBirthButton != null) {
				dateOfBirthButton.Dispose ();
				dateOfBirthButton = null;
			}

			if (annualIncomeTextField != null) {
				annualIncomeTextField.Dispose ();
				annualIncomeTextField = null;
			}

			if (annualIncomeGrowthSegment != null) {
				annualIncomeGrowthSegment.Dispose ();
				annualIncomeGrowthSegment = null;
			}

			if (retirementAgeStepper != null) {
				retirementAgeStepper.Dispose ();
				retirementAgeStepper = null;
			}

			if (retirementAgeTextField != null) {
				retirementAgeTextField.Dispose ();
				retirementAgeTextField = null;
			}

			if (existingCoverageTextField != null) {
				existingCoverageTextField.Dispose ();
				existingCoverageTextField = null;
			}
		}
	}
}
