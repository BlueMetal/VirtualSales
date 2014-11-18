using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using VirtualSales.Core.AppServices;
using System.Threading.Tasks;
using System.IO;

namespace VirtualSales.iOS.AppServices
{
    public class PlatformServices : IPlatformServices
    {
        private UIPopoverController _popoverController;

        public UIView OriginView { get; set; }

        public Task SaveAndLaunchFile(Stream stream, string fileType)
        {
            if (OriginView == null) return Task.FromResult(true);

            var data = NSData.FromStream(stream);
            var width = 824;
            var height = 668;

            var popoverView = new UIView(new RectangleF(0, 0, width, height));
            popoverView.BackgroundColor = UIColor.White;
            var webView = new UIWebView();
            webView.Frame = new RectangleF(0, 45, width, height - 45);

            var b = new UIButton(UIButtonType.RoundedRect);
            b.SetTitle("Done", UIControlState.Normal);
            b.Frame = new RectangleF(10,10, 60, 25);
            b.TouchUpInside += (o, e) => _popoverController.Dismiss(true);

            popoverView.AddSubview(b);
            popoverView.AddSubview(webView);

            var bundlePath = NSBundle.MainBundle.BundlePath;
            System.Diagnostics.Debug.WriteLine(bundlePath);
            webView.LoadData(data, "application/pdf", "utf-8", NSUrl.FromString("http://google.com"));

            var popoverContent = new UIViewController();
            popoverContent.View = popoverView;

            _popoverController = new UIPopoverController(popoverContent);
            _popoverController.PopoverContentSize = new SizeF(width, height);
            _popoverController.PresentFromRect(new RectangleF(OriginView.Frame.Width/2, 50, 1, 1), OriginView, UIPopoverArrowDirection.Any, true);
            _popoverController.DidDismiss += (o, e) => _popoverController = null;

            return Task.FromResult(true);
        }
    }
}