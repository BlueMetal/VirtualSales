using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using System.Reactive.Linq;
using System.Linq;
using VirtualSales.Core.ViewModels.Tools;

namespace VirtualSales.iOS
{
    public partial class BasicInformationToolPage2ViewController : ReactiveViewController, IViewFor<BasicInformationToolViewModel.Page2>
	{
        public BasicInformationToolPage2ViewController()
            : base("BasicInformationToolPage2ViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

        private int? ParseResult(string text)
        {
            if (NumberFormattingHelper.IsEmptyValue(text)) return 0;

            int result;
            if (Int32.TryParse(text, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.CurrentCulture, out result))
            { return result; }
            return null;
        }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            SetAmountRequested();
            SetExistingCoverage();
            SetRetirementAge();
            SetAnnualIncome();
            SetAnnualIncomeGrowth();

            annualIncomeTextField.Delegate = new TextFieldCurrencyDelegate(7, s => ViewModel.Person.AnnualIncome = ParseResult(s));
            amountRequestedTextField.Delegate = new TextFieldCurrencyDelegate(9, s => ViewModel.Person.CoverageAmountRequesting = ParseResult(s));
            existingCoverageTextField.Delegate = new TextFieldCurrencyDelegate(9, s => ViewModel.Person.ExistingCoverage = ParseResult(s));

            if (ViewModel.Mode == Core.AppMode.Client)
            {
                annualIncomeGrowthSegment.Enabled = false;
                retirementAgeStepper.Enabled = false;
                existingCoverageTextField.Enabled = false;
                amountRequestedTextField.Enabled = false;
                annualIncomeTextField.Enabled = false;
            }
            ViewModel.Person.ObservableForProperty(p => p.CoverageAmountRequesting).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetAmountRequested());
            ViewModel.Person.ObservableForProperty(p => p.ExistingCoverage).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetExistingCoverage());
            ViewModel.Person.ObservableForProperty(p => p.AnnualIncomeGrowthPercent).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetAnnualIncomeGrowth());
            ViewModel.Person.ObservableForProperty(p => p.AnnualIncome).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetAnnualIncome());
            ViewModel.Person.ObservableForProperty(p => p.RetirementAge).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetRetirementAge());
        }


        private void SetAmountRequested()
        {
            amountRequestedTextField.Text = ViewModel.Person.CoverageAmountRequesting.HasValue ?  string.Format("{0:C0}", ViewModel.Person.CoverageAmountRequesting.Value) : string.Empty;
        }
        private void SetExistingCoverage()
        {
            existingCoverageTextField.Text = ViewModel.Person.ExistingCoverage.HasValue ? string.Format("{0:C0}", ViewModel.Person.ExistingCoverage.Value) : string.Empty;
        }
        private void SetAnnualIncome()
        {
            annualIncomeTextField.Text = ViewModel.Person.AnnualIncome.HasValue  ? string.Format("{0:C0}", ViewModel.Person.AnnualIncome.Value) : string.Empty;
        }

        private void SetAnnualIncomeGrowth()
        {
            if (ViewModel.Person.AnnualIncomeGrowthPercent.HasValue)
            {
                annualIncomeGrowthSegment.SelectedSegment = ViewModel.Person.AnnualIncomeGrowthPercent.Value - 1;
            }
            else
            {
                annualIncomeGrowthSegment.SelectedSegment = 0;
            }
        }

        private void SetRetirementAge()
        {
            if (ViewModel.Person.RetirementAge.HasValue)
            {
                retirementAgeStepper.Value = (int)ViewModel.Person.RetirementAge;
                retirementAgeTextField.Text = ViewModel.Person.RetirementAge.ToString();
            }
            else
            {
                ViewModel.Person.RetirementAge = 65;
                retirementAgeStepper.Value = 65;
                retirementAgeTextField.Text = "65";
            }
        }
        partial void retirementAgeValueChanged(MonoTouch.Foundation.NSObject sender)
        {
            ViewModel.Person.RetirementAge = (int)retirementAgeStepper.Value;
            SetRetirementAge();
        }

        partial void annualIncomeGrowthValueChanged(MonoTouch.Foundation.NSObject sender)
        {
            ViewModel.Person.AnnualIncomeGrowthPercent = annualIncomeGrowthSegment.SelectedSegment + 1;
        }
        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (BasicInformationToolViewModel.Page2)value; }
        }

        private BasicInformationToolViewModel.Page2 _viewModel;

        public BasicInformationToolViewModel.Page2 ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }



	}
}

