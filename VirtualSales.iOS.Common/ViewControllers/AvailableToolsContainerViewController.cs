using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Linq;

namespace VirtualSales.iOS.ViewControllers
{
    public partial class AvailableToolsContainerViewController : UIViewController
    {
        public event EventHandler CloseToolboxRequested;

        public AvailableToolsContainerViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.doneButton.TouchUpInside += HandleDoneTouch;
        }

        private void HandleDoneTouch(object sender, EventArgs e)
        {
            var ev = CloseToolboxRequested;
            if (ev != null)
            {
                ev(this, e);
            }
        }
    }
}