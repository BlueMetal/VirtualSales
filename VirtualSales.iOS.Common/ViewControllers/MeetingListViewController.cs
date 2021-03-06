// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core;
using VirtualSales.Core.AppServices;
using VirtualSales.Core.ViewModels;
using VirtualSales.iOS;

namespace VirtualSales.iOS
{
    public partial class MeetingListViewController : ReactiveTableViewController, IViewFor<MeetingListViewModel>
    {
        private MeetingListViewModel _viewModel;
        private IDisposable _meetingSelectedSub;

        public class CustomMeetingListTableViewSource : ReactiveTableViewSource
        {
            public CustomMeetingListTableViewSource(UITableView tableView, IReactiveList<MeetingViewModel> meetings, NSString cellKey, float sizeHint)
                : base(tableView, meetings, cellKey, sizeHint)
            {
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = (UpcomingMeetingViewCell)base.GetCell(tableView, indexPath);
                cell.SetBorder();
                return cell;
            }
        }

        public MeetingListViewController(IntPtr handle) : base(handle)
        {
            ViewModel = this.GetViewModel();

            var tblSrc = new CustomMeetingListTableViewSource(TableView, ViewModel.Meetings, UpcomingMeetingViewCell.Key, 160.0f);
            _meetingSelectedSub = tblSrc.ElementSelected.Cast<MeetingViewModel>().Subscribe(OnMeetingSelected);

            TableView.SeparatorColor = UIColor.Clear;
            TableView.RegisterNibForCellReuse(UpcomingMeetingViewCell.Nib, UpcomingMeetingViewCell.Key);
            TableView.Source = tblSrc;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            System.Diagnostics.Debug.WriteLine((NavigationController == null) + " | " + (NavigationItem == null));

            this.Bind(ViewModel, vm => vm.Title, ctrl => ctrl.Title);
            NavigationController.NavigationItem.HidesBackButton = true;
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MeetingListViewModel)value; }
        }

        public MeetingListViewModel ViewModel
        {
            get { return _viewModel; }
            set { RaiseAndSetIfChanged(ref _viewModel, value); }
        }

        private void OnMeetingSelected(MeetingViewModel meeting)
        {
            ViewModel.SelectedMeeting = meeting;
            ViewModel.EnterMeetingCommand.Execute(meeting);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            NavigationController.NavigationBarHidden = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_meetingSelectedSub != null)
                {
                    _meetingSelectedSub.Dispose();
                    _meetingSelectedSub = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}