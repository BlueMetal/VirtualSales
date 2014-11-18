
using ADL;
using System;

namespace VirtualSales.iOS.ViewControllers
{
    public class ADLServiceListener : CustomServiceListener
    {
        #region Methods

        public override void ConnectionLost(ALConnectionLostEvent evt)
        {
            Console.WriteLine("ADLServiceListener.ConnectionLost");
            if (this.ViewController == null) return;
            this.ViewController.ConnectionLost(evt);
        }
        public override void MediaConnTypeChanged(ALMediaConnTypeChangedEvent evt)
        {
            Console.WriteLine("ADLServiceListener.MediaConnTypeChanged");
        }
        public override void MediaInterrupt(ALMediaInterruptEvent evt)
        {
            Console.WriteLine("ADLServiceListener.MediaInterrupt");
        }
        public override void MediaIssue(ALMediaIssueEvent evt)
        {
            Console.WriteLine("ADLServiceListener.MediaIssue");
        }
        public override void MediaStats(ALMediaStatsEvent evt)
        {
            Console.WriteLine("ADLServiceListener.MediaStats");

        }
        public override void MediaStream(ALUserStateChangedEvent evt)
        {
            Console.WriteLine("ADLServiceListener.MediaStream");
            if (this.ViewController == null) return;
            this.ViewController.MediaStream(evt);
        }
        public override void Message(ALMessageEvent evt)
        {
            Console.WriteLine("ADLServiceListener.Message");
        }
        public override void UserEvent(ALUserStateChangedEvent evt)
        {
            Console.WriteLine("ADLServiceListener.UserEvent");
            if (this.ViewController == null) return;
            this.ViewController.UserEvent(evt);
        }
        public override void VideoFrameSizeChanged(ALVideoFrameSizeChangedEvent evt)
        {
            Console.WriteLine("ADLServiceListener.VideoFrameSizeChanged");
            if (this.ViewController == null) return;
            this.ViewController.VideoFrameSizeChanged(evt);
        }

        #endregion

        #region Properties

        public VideoConferenceViewController ViewController { get; set; }

        #endregion
    }
}