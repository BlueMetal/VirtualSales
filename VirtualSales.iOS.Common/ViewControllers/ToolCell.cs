using System;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI.Cocoa;

namespace VirtualSales.iOS.ViewControllers
{
	public partial class ToolCell : ReactiveCollectionViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("ToolCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("ToolCell");

	    private static readonly CGColor BorderColor = UIColor.FromRGB(0, 122, 255).CGColor; 
		public ToolCell (IntPtr handle) : base (handle)
		{
		}

		public static ToolCell Create ()
		{
			return (ToolCell)Nib.Instantiate (null, null) [0];
		}

	    public void SetBorder()
	    {
            Layer.MasksToBounds = true;
            Layer.BorderColor = BorderColor;
            Layer.BorderWidth = 1.0f;
	    }

        public string Text
        {
            get { return this.textLabel.Text;  }
            set { this.textLabel.Text = value; }
        }
	}
}

