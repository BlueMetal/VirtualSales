using System;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels;
using VirtualSales.Core.ViewModels.Tools;
using VirtualSales.Models;

namespace VirtualSales.iOS
{
    public partial class BasicInformationToolPage1ViewController : ReactiveViewController, IViewFor<BasicInformationToolViewModel.Page1>
    {
        private UIDatePicker _datePicker;
        private UIPopoverController _popoverController;

        public BasicInformationToolPage1ViewController()
            : base("BasicInformationToolPage1ViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

        private void SetGender()
        {
            if (ViewModel.Person.Gender.HasValue)
            {
                genderSegmentedControl.SelectedSegment = ViewModel.Person.Gender == Gender.Male ? 0 : 1;
            }
            else
            {
                genderSegmentedControl.SelectedSegment = 0;
            }
        }

        private void SetSmoker()
        {
            smokerSwitcher.On = ViewModel.Person.IsSmoker;
        }

        private void SetNumberOfDependents()
        {
            if (ViewModel.Person.NumOfDependents.HasValue)
            {
                numOfDependentsStepper.Value = (int)ViewModel.Person.NumOfDependents;
                numOfDependentsLabel.Text = ViewModel.Person.NumOfDependents.ToString();
            }
            else
            {
                numOfDependentsStepper.Value = 0;
                numOfDependentsLabel.Text = "0";
            }
        }

        private void SetBirthDate()
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

        private void SetFirstName()
        {
            firstNameTextField.Text = ViewModel.Person.FirstName;
        }

        private void SetLastName()
        {
            lastNameTextField.Text = ViewModel.Person.LastName;
        }
        partial void genderSegmentedValueChanged(MonoTouch.Foundation.NSObject sender)
        {
            ViewModel.Person.Gender = genderSegmentedControl.SelectedSegment == 0 ? Gender.Male : Gender.Female;
        }

        partial void numOfDependentsValueChanged(MonoTouch.Foundation.NSObject sender)
        {
            ViewModel.Person.NumOfDependents = (int)numOfDependentsStepper.Value;
            SetNumberOfDependents();
        }

        partial void smokerSwitchValueChanged(MonoTouch.Foundation.NSObject sender)
        {
            ViewModel.Person.IsSmoker = smokerSwitcher.On;
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
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            _datePicker = new UIDatePicker();
            _datePicker.Mode = UIDatePickerMode.Date;
            _datePicker.ValueChanged += HandleDatePickerValueChanged;
            dateOfBirthButton.TouchUpInside += (o, e) => ShowDateOfBirthPicker();
            SetBirthDate();

            SetGender();
            SetSmoker();
            SetNumberOfDependents();

            SetFirstName();
            SetLastName();

            if (ViewModel.Mode == Core.AppMode.Agent)
            {
                firstNameTextField.EditingChanged += (o, e) => ViewModel.Person.FirstName = firstNameTextField.Text;
                lastNameTextField.EditingChanged += (o, e) => ViewModel.Person.LastName = lastNameTextField.Text;
            }
            else
            {
                firstNameTextField.Enabled = false;
                lastNameTextField.Enabled = false;
                dateOfBirthButton.Enabled = false;
                smokerSwitcher.Enabled = false;
                genderSegmentedControl.Enabled = false;
                numOfDependentsStepper.Enabled = false;
            }

            ViewModel.Person.ObservableForProperty(p => p.FirstName).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetFirstName());
            ViewModel.Person.ObservableForProperty(p => p.LastName).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetLastName());
            ViewModel.Person.ObservableForProperty(p => p.Gender).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetGender());
            ViewModel.Person.ObservableForProperty(p => p.DateOfBirth).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetBirthDate());
            ViewModel.Person.ObservableForProperty(p => p.IsSmoker).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetSmoker());
            ViewModel.Person.ObservableForProperty(p => p.NumOfDependents).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetNumberOfDependents());
		}

        void HandleDatePickerValueChanged(object sender, EventArgs e)
        {
            ViewModel.Person.DateOfBirth = _datePicker.Date;
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (BasicInformationToolViewModel.Page1)value; }
        }
        
        

        private BasicInformationToolViewModel.Page1 _viewModel;

        public BasicInformationToolViewModel.Page1 ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }
	}
}

