using System;
using System.Drawing;

using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace VirtualSales.iOS
{
    // This dummy class is here 
    public class Dummy : UIViewController
    {
        public Dummy()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
       

            base.ViewDidLoad();

            // Perform any additional setup after loading the view

            var l = new UITextField();

            l.Text = "foo";

            Console.WriteLine(l.Text);

            var lbl = new UILabel();
            lbl.Text = "foo";
            Console.WriteLine(lbl.Text);

            var btn = new UIButton();
            btn.Selected = true;
            btn.Enabled = true;

            Console.WriteLine(btn.Selected);
            Console.WriteLine(btn.Enabled);
        }
    }
}