using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels;

namespace VirtualSales.iOS
{
    public partial class UpcomingMeetingViewCell : ReactiveTableViewCell, IViewFor<MeetingViewModel>
    {
        public static readonly UINib Nib = UINib.FromName("UpcomingMeetingViewCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("UpcomingMeetingViewCell");
        private MeetingViewModel _viewModel;

        public UpcomingMeetingViewCell(IntPtr handle) : base(handle)
        {
            this.Bind(ViewModel, vm => vm.Client.PhoneNumber, c => c.phoneNumberLabel.Text);
            this.Bind(ViewModel, vm => vm.Status, c => c.statusLabel.Text);
            this.Bind(ViewModel, vm => vm.Client.FullName, c => c.personLabel.Text);
            this.Bind(ViewModel, vm => vm.TimeAndDuration, c => c.timeLabel.Text);
            this.Bind(ViewModel, vm => vm.DayOfTheMonth, c => c.dayNumberLabel.Text);
            this.Bind(ViewModel, vm => vm.DayOfTheWeek, c => c.datOfTheWeekLabel.Text);
            this.Bind(ViewModel, vm => vm.MonthAndYear, c => c.monthAndYearLabel.Text);
            this.Bind(ViewModel, vm => vm.Identifier, c => c.meetingIdentifierLabel.Text);
        }

        public void SetBorder()
        {
            ContainerBorderView.Layer.MasksToBounds = true;
            ContainerBorderView.Layer.BorderColor = UIColor.Gray.CGColor;
            ContainerBorderView.Layer.BorderWidth = 1.0f;
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MeetingViewModel)value; }
        }

        public MeetingViewModel ViewModel
        {
            get { return _viewModel; }
            set { RaiseAndSetIfChanged(ref _viewModel, value); }
        }

        public static UpcomingMeetingViewCell Create()
        {
            return (UpcomingMeetingViewCell)Nib.Instantiate(null, null)[0];
        }
    }
}