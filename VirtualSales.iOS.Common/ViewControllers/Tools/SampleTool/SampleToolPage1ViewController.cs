using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels.Tools;

namespace VirtualSales.iOS
{
    public partial class SampleToolPage1ViewController : ReactiveViewController, IViewFor<SampleToolViewModel.Page1>
	{

        public SampleToolPage1ViewController () : base ("SampleToolPage1ViewController", NSBundle.MainBundle)
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
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (SampleToolViewModel.Page1)value; }
        }

        private SampleToolViewModel.Page1 _viewModel;

        public SampleToolViewModel.Page1 ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }
	}
}

