using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels.Tools;
using Infragistics;
using System.Linq;
using System.Reactive.Linq;

namespace VirtualSales.iOS
{
    public partial class LifetimeEconomicValueToolPage1ViewController : ReactiveViewController, IViewFor<LifetimeEconomicValueViewModel.Page1>
	{
        private UIDatePicker _datePicker;
        private UIPopoverController _popoverController;

        public LifetimeEconomicValueToolPage1ViewController()
            : base("LifetimeEconomicValueToolPage1ViewController", null)
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

        private void ShowDateOfBirthPicker()
        {
            UIView popoverView = new UIView(new RectangleF(0, 0, 320, 300));
            popoverView.BackgroundColor = UIColor.White;
            _datePicker.Frame = new RectangleF(0, 0, 320, 300);
            popoverView.AddSubview(_datePicker);

            UIViewController popoverContent = new UIViewController();
            popoverContent.View = popoverView;

            _popoverController = new UIPopoverController(popoverContent);
            _popoverController.PopoverContentSize = new SizeF(320, 244);
            _popoverController.PresentFromRect(dateOfBirthButton.Frame, dateOfBirthButton, UIPopoverArrowDirection.Any, true);
            _popoverController.DidDismiss += (o, e) => _popoverController = null;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _datePicker = new UIDatePicker();
            _datePicker.Mode = UIDatePickerMode.Date;
            _datePicker.ValueChanged += HandleDatePickerValueChanged;
            dateOfBirthButton.TouchUpInside += (o, e) => ShowDateOfBirthPicker();
            SetDateOfBirth();

            SetExistingCoverage();
            SetRetirementAge();
            SetAnnualIncome();
            SetAnnualIncomeGrowth();

            existingCoverageTextField.Delegate = new TextFieldCurrencyDelegate(9, s => ViewModel.Person.ExistingCoverage = ParseResult(s));
            annualIncomeTextField.Delegate = new TextFieldCurrencyDelegate(7, s => ViewModel.Person.AnnualIncome = ParseResult(s));

            if (ViewModel.Mode == Core.AppMode.Client)
            {
                annualIncomeGrowthSegment.Enabled = false;
                retirementAgeStepper.Enabled = false;
                existingCoverageTextField.Enabled = false;
                dateOfBirthButton.Enabled = false;
                annualIncomeTextField.Enabled = false;
            }
            ViewModel.Person.ObservableForProperty(p => p.DateOfBirth).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetDateOfBirth());
            ViewModel.Person.ObservableForProperty(p => p.ExistingCoverage).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetExistingCoverage());
            ViewModel.Person.ObservableForProperty(p => p.AnnualIncomeGrowthPercent).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetAnnualIncomeGrowth());
            ViewModel.Person.ObservableForProperty(p => p.AnnualIncome).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetAnnualIncome());
            ViewModel.Person.ObservableForProperty(p => p.RetirementAge).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetRetirementAge());
        }

        void HandleDatePickerValueChanged(object sender, EventArgs e)
        {
            ViewModel.Person.DateOfBirth = _datePicker.Date;
        }

        private void SetDateOfBirth()
        {
            if (ViewModel.Person.DateOfBirth.HasValue)
            {
                var dob = ViewModel.Person.DateOfBirth.Value;
                dateOfBirthButton.SetTitle(dob.ToString("MMMM d, yyyy"), UIControlState.Normal);
            }
            else
            {
                dateOfBirthButton.SetTitle(ViewModel.Mode == Core.AppMode.Agent ? "Tap to set Birth Date" : string.Empty, UIControlState.Normal);
            }

            var dt = ViewModel.Person.DateOfBirth.HasValue ? ViewModel.Person.DateOfBirth.Value : DateTime.Now;
            _datePicker.SetDate(DateTime.SpecifyKind(dt, DateTimeKind.Utc), false);
        }
        private void SetExistingCoverage()
        {
            existingCoverageTextField.Text = ViewModel.Person.ExistingCoverage.HasValue ? string.Format("{0:C0}", ViewModel.Person.ExistingCoverage.Value) : string.Empty;
        }
        private void SetAnnualIncome()
        {
            annualIncomeTextField.Text = ViewModel.Person.AnnualIncome.HasValue ? string.Format("{0:C0}", ViewModel.Person.AnnualIncome.Value) : string.Empty;
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
        partial void retirementAgeStepperValueChanged(MonoTouch.Foundation.NSObject sender)
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
            set { ViewModel = (LifetimeEconomicValueViewModel.Page1)value; }
        }

        private LifetimeEconomicValueViewModel.Page1 _viewModel;

        public LifetimeEconomicValueViewModel.Page1 ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }
	}
}

