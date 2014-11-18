using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels.Tools;
using System.Reactive.Linq;
using System.Linq;

namespace VirtualSales.iOS
{
    public partial class BasicInformationToolPage3ViewController : ReactiveViewController, IViewFor<BasicInformationToolViewModel.Page3>
	{
        public BasicInformationToolPage3ViewController()
            : base("BasicInformationToolPage3ViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            SetAddr1();
            SetAddr2();
            SetCity();
            SetZip();
            SetState();
            if (ViewModel.Mode == Core.AppMode.Agent)
            {
                addr1TextField.EditingChanged += (o, e) => ViewModel.Person.Addr1 = addr1TextField.Text;
                addr2TextField.EditingChanged += (o, e) => ViewModel.Person.Addr2 = addr2TextField.Text;
                cityTextField.EditingChanged += (o, e) => ViewModel.Person.City = cityTextField.Text;
                stateTextField.EditingChanged += (o, e) => ViewModel.Person.State = stateTextField.Text;
                zipTextField.EditingChanged += (o, e) => ViewModel.Person.Zip = zipTextField.Text;
            }
            else
            {
                addr1TextField.Enabled = false;
                addr2TextField.Enabled = false;
                cityTextField.Enabled = false;
                stateTextField.Enabled = false;
                zipTextField.Enabled = false;
            }
           
            ViewModel.Person.ObservableForProperty(p => p.Addr1).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetAddr1());
            ViewModel.Person.ObservableForProperty(p => p.Addr2).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetAddr2());
            ViewModel.Person.ObservableForProperty(p => p.City).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetCity());
            ViewModel.Person.ObservableForProperty(p => p.State).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetState());
            ViewModel.Person.ObservableForProperty(p => p.Zip).ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ => SetZip());
        }

        private void SetAddr1()
        {
            addr1TextField.Text = ViewModel.Person.Addr1;
        }
        private void SetAddr2()
        {
            addr2TextField.Text = ViewModel.Person.Addr2;
        }
        private void SetCity()
        {
            cityTextField.Text = ViewModel.Person.City;
        }
        private void SetState()
        {
            stateTextField.Text = ViewModel.Person.State;
        }
        private void SetZip()
        {
            zipTextField.Text = ViewModel.Person.Zip;
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (BasicInformationToolViewModel.Page3)value; }
        }

        private BasicInformationToolViewModel.Page3 _viewModel;

        public BasicInformationToolViewModel.Page3 ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }
	}
}

