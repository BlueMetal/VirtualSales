using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using Splat;
using VirtualSales.Core;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.Infrastructure;
using VirtualSales.iOS.ViewControllers;
using MonoKit.UI.ViewDeck;

namespace VirtualSales.iOS
{
    public abstract class WhiteBrandAppDelegate : UIApplicationDelegate
    {
        private readonly bool _isAgent;
        public ViewModelLocator ViewModelLocator { get; private set; }
        public ViewDeckController ViewDeckController { get; private set; }
        protected WhiteBrandAppDelegate(bool isAgent)
        {
            _isAgent = isAgent;
        }

        //public class D : ViewDeckControllerDelegate
        //{
        //    override 
        //}
        public override async void FinishedLaunching(UIApplication application)
        {
            // NB: GrossHackAlertTiem™:
            //
            // Monotouch appears to not load assemblies when you request them 
            // via Type.GetType, unlike every other platform (even 
            // Xamarin.Android). So, we've got to manually do what RxUI and 
            // Akavache would normally do for us
            var r = Locator.CurrentMutable;
            (new ReactiveUI.Cocoa.Registrations()).Register((f, t) => r.Register(f, t));
            //(new ReactiveUI.Mobile.Registrations()).Register((f, t) => r.Register(f, t));
            //(new Akavache.Registrations()).Register(r.Register);
            //(new Akavache.Mobile.Registrations()).Register(r.Register);
            //(new Akavache.Sqlite3.Registrations()).Register(r.Register);

            // Logger is slow right now
            var logger = new RxUiLogger();
            Locator.CurrentMutable.RegisterConstant(logger, typeof(ILogger));

            ViewModelLocator = new iOSViewModelLocator(_isAgent);

            var prevroot = this.Window.RootViewController;

            if (_isAgent)
            {
                var navpane = new NavigationPaneViewController(ViewModelLocator);
                ViewDeckController = new ViewDeckController(prevroot, navpane);
                ViewDeckController.LeftLedge = 704; // 1024 - 320 (width of the control)
            }
            else
            {
                ViewDeckController = new ViewDeckController(prevroot);
            }
            this.Window.RootViewController = ViewDeckController;

            var service = ViewModelLocator.MainService;

            await service.StartApp();
        }
    }
}