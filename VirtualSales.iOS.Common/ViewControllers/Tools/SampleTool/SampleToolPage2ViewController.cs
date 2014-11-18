using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels.Tools;

namespace VirtualSales.iOS
{
    public partial class SampleToolPage2ViewController : ReactiveViewController, IViewFor<SampleToolViewModel.Page2>
	{
		public SampleToolPage2ViewController () : base ("SampleToolPage2ViewController", null)
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
            set { ViewModel = (SampleToolViewModel.Page2)value; }
        }

        private SampleToolViewModel.Page2 _viewModel;

        public SampleToolViewModel.Page2 ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }
	}
}

