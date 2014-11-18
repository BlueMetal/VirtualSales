using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels;
using ReactiveUI;
using VirtualSales.Core;

namespace VirtualSales.iOS.ViewControllers
{
    public partial class NavigationPaneViewController : ReactiveViewController, IViewFor<NavigationPaneViewModel>
    {
        private NavigationPaneViewModel _viewModel;

        public NavigationPaneViewController(IntPtr handle)
            : base(handle)
        {
        }

        public NavigationPaneViewController(IViewModelLocator locator)
            : base("NavigationPaneViewController", null)
        {
            ViewModel = locator.NavigationPaneViewModel;
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (NavigationPaneViewModel)value; }
        }

        public NavigationPaneViewModel ViewModel
        {
            get { return _viewModel; }
            set { RaiseAndSetIfChanged(ref _viewModel, value); }
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.BindCommand(ViewModel, v => v.InvokeToolLibraryCommand, v => v.invokeToolLibraryButton);
            this.BindCommand(ViewModel, v => v.InvokeUpcomingMeetingsCommand, v => v.upcomingMeetingsButton);
            this.BindCommand(ViewModel, v => v.LogoutCommand, v => v.logOutButton);

            ViewModel.WhenAnyValue(p => p.IsToolLibraryButtonHidden, p => p).
                      ObserveOn(RxApp.MainThreadScheduler).Subscribe(p => invokeToolLibraryButton.Hidden = p);
            ViewModel.WhenAnyValue(p => p.IsUpcomingMeetingsButtonHidden, p => p).
                      ObserveOn(RxApp.MainThreadScheduler).Subscribe(p => upcomingMeetingsButton.Hidden = p);


            ViewModel.HideNavPaneAction = () =>
                                          {
                                              var vd = ((WhiteBrandAppDelegate)UIApplication.SharedApplication.Delegate).ViewDeckController;
                                              vd.CloseLeftView(true);
                                          };
        }
    }
}