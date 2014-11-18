// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels;

namespace VirtualSales.iOS.ViewControllers
{
    public partial class LobbyViewController : ReactiveViewController, IViewFor<ClientLobbyViewModel>
    {
        private ClientLobbyViewModel _viewModel;
        
        public override void ViewDidLoad()
        {
            NavigationController.NavigationBarHidden = true;
            this.Title = "Lobby";

            this.Bind(ViewModel, t => t.MeetingId, t => t.meetingIdTextField.Text);
            this.Bind(ViewModel, t => t.Title, t => t.lobbyStatusLabel.Text);
            this.Bind(ViewModel, t => t.Error, t => t.errorLabel.Text);

            this.BindCommand(ViewModel, t => t.DisconnectCommand, t => t.disconnectButton);
            this.BindCommand(ViewModel, t => t.ConnectCommand, t => t.connectButton);
        }

        public LobbyViewController(IntPtr handle) : base(handle)
        {
            ViewModel = this.GetViewModel();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ClientLobbyViewModel)value; }
        }

        public ClientLobbyViewModel ViewModel
        {
            get { return _viewModel; }
            set { RaiseAndSetIfChanged(ref _viewModel, value); }
        }
    }
  }