using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace VirtualSales.iOS
{
	public partial class ToolNavigationController : UINavigationController
	{
		public ToolNavigationController (IntPtr handle) : base (handle)
		{
            this.NavigationBarHidden = true;
		}

	    public override void ViewDidLoad()
	    {
	        base.ViewDidLoad();
	    }
	}
}
