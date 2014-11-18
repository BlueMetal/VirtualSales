using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using ADL;
using Infragistics;
using ReactiveUI;
using VirtualSales.Core.ViewModels;
using VirtualSales.Models;
using Button = System.Windows.Controls.Button;
using Panel = System.Windows.Forms.Panel;
using Platform = ADL.Platform;
using RenderOptions = ADL.RenderOptions;
using UserControl = System.Windows.Controls.UserControl;

namespace VirtualSales.Wpf.Controls
{
    public class ADLVideoHelper
    {
        private const int MaxRetryCount = 10;
        private AddLiveServiceEventDispatcher _eDispatcher;
        private bool _localVideoStarted;
        private string _scopeId;
        private bool _isPlatformInitialized = false;
        private string _connectedScopeId;
        private PlatformInitListenerDispatcher _dispatcher;
        private int _retryCount = 0;
        private string _videoDevId;
        private Button _toggleCamerasButton;
        public VideoConfViewModel ViewModel { get; set; }
        public Panel LocalVideoPanel { get; set; }
        public Panel RemoteVideoPanel { get; set; }
        public TextBlock InitVideoText { get; set; }

        public Button ToggleCamerasButton
        {
            get { return _toggleCamerasButton;  }
            set
            {
                if (_toggleCamerasButton != null)
                {
                    _toggleCamerasButton.Click -= HandleToggleCamera;
                }
                
                _toggleCamerasButton = value;
                _toggleCamerasButton.Click += HandleToggleCamera;
            }
        }

        public ADLVideoHelper()
        {
            _dispatcher = new PlatformInitListenerDispatcher();
            _dispatcher.StateChanged += OnADLInitStateChanged;
        }

        private void RemoveRenderer(Panel container)
        {
            if (container.InvokeRequired)
            {
                container.Invoke(
                    new Action<Panel, RenderingWidget>(AppendRenderer),
                    new object[] { container });
                return;
            }

            container.Controls.Clear();
        }

        private void AppendRenderer(Panel container, RenderingWidget widget)
        {
            if (container.InvokeRequired)
            {
                container.Invoke(
                    new Action<Panel, RenderingWidget>(AppendRenderer),
                    new object[] {container, widget});
                return;
            }
            Debug.WriteLine("Local video feed renderer creaed. Appending to scene");
            widget.Width = container.Width;
            widget.Height = container.Height;
            container.Controls.Add(widget);
        }

        private AuthDetails GenAuthDetails(string scopeId, long userId)
        {
            // Fill the simple fields
            var authDetails = new AuthDetails();
            //authDetails.expires = // 5 minutes 
            //    (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds + 300;
            authDetails.expires = ViewModel.Config.Expires;
            authDetails.userId = userId;
            authDetails.salt = ViewModel.Config.Salt;

            // Calculate and fill the signature
            var signatureBody = "" + ViewModel.Config.AppId + scopeId + userId +
                                authDetails.salt + authDetails.expires + ViewModel.Config.ApiKey;
            var enc = new ASCIIEncoding();
            var sigBodyBinary = enc.GetBytes(signatureBody);
            var hasher = SHA256Managed.Create();
            var sigBinary = hasher.ComputeHash(sigBodyBinary);
            authDetails.signature = BitConverter.ToString(sigBinary).Replace("-", "");

            System.Diagnostics.Debug.WriteLine(string.Format("Creating signature with [AppId = {0}] [Scope = {1}] [UserId = {2}] [Salt = {3}] [Expires = {4}] [ApiKey = {5}]",
                ViewModel.Config.AppId, 
                scopeId,
                userId,
                authDetails.salt,
                authDetails.expires,
                ViewModel.Config.ApiKey));
            System.Diagnostics.Debug.WriteLine(authDetails.signature);
            return authDetails;
        }

        private void GenericErrorHandler(string methodName, int errCode, string errMessage)
        {
            Debug.WriteLine("Got error when processing method: " + methodName + ". Cause: " +
                            errMessage + "(" + errCode + ")");
        }

        private ErrHandler GenErrHandler(string methodName)
        {
            return new ErrHandler((_1, _2) => GenericErrorHandler(methodName, _1, _2));
        }

        private Responder<T> GenGenericResponder<T>(string methodName)
        {
            return Platform.R<T>(
                delegate(T sth) { Debug.WriteLine("Got successfull result of method call: " + methodName); }, GenErrHandler(methodName));
        }

        private void InitializeCDOEventListener()
        {
            _eDispatcher = new AddLiveServiceEventDispatcher();
            _eDispatcher.UserEvent += OnUserEvent;
            _eDispatcher.MediaConnTypeChanged += OnMediaConnTypeChanged;
            _eDispatcher.MediaStream += OnMediaStream;
        }

        private void MaybeSwitchLocalVideo(object nothing = null)
        {
            if (!_localVideoStarted)
            {
                MaybeStartLocalVideo();
                return;
            }

            RemoveRenderer(LocalVideoPanel);

            ADL.Responder<object> wasStopped = new ResponderAdapter<object>(
                s => Platform.Service.startLocalVideo(Platform.R<string>(OnLocalVideoChanged, GenErrHandler("startLocalVideo"))), GenErrHandler("stopLocalVideo"));
            
            Platform.Service.stopLocalVideo(wasStopped);
        }

        private void MaybeStartLocalVideo(object nothing = null)
        {
            if (_localVideoStarted)
                return;
            Debug.WriteLine("Starting local video");
            Platform.Service.startLocalVideo(Platform.R<string>(OnLocalVideoStarted, GenErrHandler("startLocalVideo")));
            _localVideoStarted = true;
        }

        private void OnADLInitStateChanged(object sender, InitStateChangedEvent e)
        {
            if (e.state == InitStateChangedEvent.InitState.ERROR)
            {
                if (_retryCount <= MaxRetryCount)
                {
                    _retryCount++;
                    Debug.WriteLine("Retying init");
                    Platform.init(_dispatcher);
                    return;
                }
                Debug.WriteLine("Failed to initialize platform. Cause: " + e.errMessage + "(" + e.errCode + ")");
                return;
            }
            InitializeCDOEventListener();

            Debug.WriteLine("Platform initialized. Proceeding with the initialization");

            PostInitializePlatform();
        }

        private void PostInitializePlatform()
        {
            Platform.Service.getVersion(Platform.R<string>(OnVersion, GenErrHandler("getVersion")));
            Platform.Service.setApplicationId(GenGenericResponder<object>("setApplicationId"), ViewModel.Config.AppId);
            Platform.Service.addServiceListener(GenGenericResponder<object>("addServiceListener"), _eDispatcher);

            Platform.Service.getAudioCaptureDeviceNames(
                Platform.R<Dictionary<string, string>>(OnAudioCaptureDevices));
            Platform.Service.getAudioOutputDeviceNames(
                Platform.R<Dictionary<string, string>>(OnAudioOutputDevices));
            Platform.Service.getVideoCaptureDeviceNames(
                Platform.R<Dictionary<string, string>>(OnVideoCaptureDevices));            
        }

        private void OnAudioCaptureDevices(Dictionary<string, string> devs)
        {
            Debug.WriteLine("Got audio capture devices (" + devs.Count + ")");
        }

        private void OnAudioOutputDevices(Dictionary<string, string> devs)
        {
            Debug.WriteLine("Got audio output devices (" + devs.Count + ")");
        }

        private void OnLocalVideoChanged(string sinkId)
        {

            Debug.WriteLine("Local video changed. Creating renderer");
            var rOptions = new RenderOptions();
            rOptions.mirror = true;
            rOptions.sinkId = sinkId;
            rOptions.filter = VideoScalingFilter.FAST_BILINEAR;
            var rHandler =
                new ResultHandler<RenderingWidget>((_1) => AppendRenderer(LocalVideoPanel, _1));
            Platform.renderSink(Platform.R<RenderingWidget>(rHandler,
                                                            GenErrHandler("renderSink")), rOptions);
        }
        private void OnLocalVideoStarted(string sinkId)
        {
            Debug.WriteLine("Local video started. Creating renderer");
            var rOptions = new RenderOptions();
            rOptions.mirror = true;
            rOptions.sinkId = sinkId;
            rOptions.filter = VideoScalingFilter.FAST_BILINEAR;
            var rHandler =
                new ResultHandler<RenderingWidget>((_1) => AppendRenderer(LocalVideoPanel, _1));
            Platform.renderSink(Platform.R<RenderingWidget>(rHandler,
                                                            GenErrHandler("renderSink")), rOptions);

            Debug.WriteLine("Connecting to scope with id: " + ViewModel.Config.ScopeId);
            var connDescr = new ConnectionDescription();
            connDescr.autopublishAudio = true;
            connDescr.autopublishVideo = true;
            connDescr.scopeId = ViewModel.Config.ScopeId;
            connDescr.url = "174.127.76.172:443/" + ViewModel.Config.ScopeId;
            connDescr.token = ViewModel.UserId.ToString(CultureInfo.InvariantCulture);
            connDescr.lowVideoStream.maxBitRate = 64;
            connDescr.lowVideoStream.maxWidth = 150;
            connDescr.lowVideoStream.maxHeight = 150;
            connDescr.lowVideoStream.maxFps = 5;
            connDescr.lowVideoStream.publish = true;
            connDescr.lowVideoStream.receive = true;

            connDescr.highVideoStream.maxBitRate = 512;
            connDescr.highVideoStream.maxWidth = 150;
            connDescr.highVideoStream.maxHeight = 150;
            connDescr.highVideoStream.maxFps = 15;
            connDescr.highVideoStream.publish = true;
            connDescr.highVideoStream.receive = true;

            connDescr.authDetails = GenAuthDetails(ViewModel.Config.ScopeId, ViewModel.UserId);
            _connectedScopeId = ViewModel.Config.ScopeId;
            Platform.Service.connect(GenGenericResponder<object>("connect"), connDescr);
            Platform.Service.publish(GenGenericResponder<object>("publishVideo"), _connectedScopeId,
                                     MediaType.VIDEO, null);

            ViewModel.VideoInitCompleted = true;
            Action a1 = () => InitVideoText.Visibility = Visibility.Collapsed;
            Application.Current.Dispatcher.BeginInvoke(a1, null);

            if (_availableVideoDevices.Count >= 2 && ToggleCamerasButton != null)
            {
                Action a = () => ToggleCamerasButton.Visibility = Visibility.Visible;
                System.Windows.Application.Current.Dispatcher.BeginInvoke(a, null);
            }
        }

        private void OnMediaConnTypeChanged(object sender, MediaConnTypeChangedEvent e)
        {
            Debug.WriteLine("Got new media connection type: " + e.ConnType.StringValue);
        }

        private void OnMediaStream(object sender, UserStateChangedEvent e)
        {
            Debug.WriteLine("Got new media stream event for user with id: " + e.UserId +
                            ". Media type: " + e.MediaType);
        }

        private void OnUserEvent(object sender, UserStateChangedEvent e)
        {
            Debug.WriteLine("Got new user event for user with id: " + e.UserId);
            Debug.WriteLine("User just " + (e.IsConnected ? "joined" : "left") + "the scope");
            if (e.IsConnected)
            {
                Debug.WriteLine("User is " + (!e.AudioPublished ? "not" : "") + " publishing audio stream");
                Debug.WriteLine("User is " + (!e.VideoPublished ? "not" : "") + " publishing video stream");
                if (e.VideoPublished)
                {
                    Debug.WriteLine("Creating renderer for remote user sink: " + e.VideoSinkId);
                    var rOptions = new RenderOptions();
                    rOptions.mirror = true;
                    rOptions.sinkId = e.VideoSinkId;
                    rOptions.filter = VideoScalingFilter.FAST_BILINEAR;
                    var rHandler =
                        new ResultHandler<RenderingWidget>((_1) => AppendRenderer(RemoteVideoPanel, _1));
                    Platform.renderSink(Platform.R<RenderingWidget>(rHandler,
                                                                    GenErrHandler("renderSink")), rOptions);
                }
            }
            else
            {
            }
        }

        private void OnVersion(string version)
        {
            Debug.WriteLine("Got platform version: " + version);
        }

        private void HandleToggleCamera(object sender, RoutedEventArgs e)
        {

            _currentVideoDevice++;
            _currentVideoDevice = _currentVideoDevice  % _availableVideoDevices.Count;
            _videoDevId = _availableVideoDevices[_currentVideoDevice];
            Debug.WriteLine("Switching to video device id: " + _videoDevId);

            Platform.Service.setVideoCaptureDevice(Platform.R<object>(MaybeStartLocalVideo, GenErrHandler("setVideoCaptureDevice")), _videoDevId);
        }

        private List<string> _availableVideoDevices = new List<string>();
        private int _currentVideoDevice = 0;

        private void OnVideoCaptureDevices(Dictionary<string, string> devs)
        {
            Debug.WriteLine("Got video capture devices (" + devs.Count + ")");
            if (devs.Count == 0)
            {
                Debug.WriteLine("Got 0 video capture devices. Not starting local video.");
                return;
            }
            _availableVideoDevices = new List<string>(devs.Keys);
            _currentVideoDevice = 0;
            _videoDevId = _availableVideoDevices[_currentVideoDevice];
            Debug.WriteLine("Starting video on device id: " + _videoDevId);
            StartLocalVideo();
        }

        public void CleanupVideoConf()
        {
            if (_localVideoStarted)
            {
                if (ViewModel != null) { 
                    ViewModel.VideoInitCompleted = false;
                }

                RemoveRenderer(LocalVideoPanel);
                RemoveRenderer(RemoteVideoPanel);

                if (Platform.Service != null)
                {
                    Platform.Service.stopLocalVideo(GenGenericResponder<object>("stopLocalVideo"));
                    Platform.Service.unpublish(GenGenericResponder<object>("unpublish"), _scopeId, MediaType.VIDEO);
                    Platform.Service.disconnect(GenGenericResponder<object>("disconnect"), _scopeId);

                }
                Thread.Sleep(1000);
                Platform.release();
                _scopeId = null;
            }
        }

        private void InitPlatform()
        {
            _localVideoStarted = false;
            try
            {
                Platform.init(_dispatcher);
            }
            catch (Exception )
            {
                MessageBox.Show("oops");
            }
            _isPlatformInitialized = true;
        }

        public void PrepVideoConf()
        {
            Action<object> a = _ => InitVideoText.Visibility = Visibility.Visible;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, a, null);
            
            _scopeId = ViewModel.Config.ScopeId;
            System.Diagnostics.Debug.WriteLine("Setting up video for scope " + _scopeId);
            InitPlatform();            
        }

        private void StartLocalVideo()
        {
            Platform.Service.setVideoCaptureDevice(Platform.R<object>(MaybeStartLocalVideo, GenErrHandler("setVideoCaptureDevice")), _videoDevId);
        }

        private void SwitchLocalVideo()
        {
            Platform.Service.setVideoCaptureDevice(Platform.R<object>(MaybeSwitchLocalVideo, GenErrHandler("setVideoCaptureDevice")), _videoDevId);            
        }
    }

    /// <summary>
    ///     Interaction logic for VideoConferenceControl.xaml
    /// </summary>
    public partial class VideoConferenceControl : UserControl
    {
        private VideoChatConfiguration _config;
        private IDisposable _configDisp = Disposable.Empty;
        private static ADLVideoHelper _videoHelper;

        public VideoConferenceControl()
        {
            InitializeComponent();

            if (DesignModeDetector.IsDesignMode) return;


            if (_videoHelper == null)
            {
                _videoHelper = new ADLVideoHelper();
            }
            _videoHelper.ToggleCamerasButton = ToggleCameras;
            _videoHelper.LocalVideoPanel = LocalVideoPanel;
            _videoHelper.RemoteVideoPanel = RemoteVideoPanel;
            _videoHelper.InitVideoText = InitVideoText;

            _configDisp = this.WhenAnyDynamic(new[] {"DataContext", "Config"}, c => (VideoChatConfiguration)c.Value)
                              .ObserveOn(RxApp.MainThreadScheduler)
                              .Subscribe(config =>
                                         {
                                             if (config == null && _config != null)
                                             {
                                                 // clean up
                                                 _config = null;
                                                 _videoHelper.CleanupVideoConf();
                                                 _videoHelper.ViewModel = null;
                                             }
                                             else if (config != null)
                                             {
                                                 _config = config;
                                                 if (_videoHelper.ViewModel == null || _config != _videoHelper.ViewModel.Config)
                                                 {
                                                     _videoHelper.ViewModel = (VideoConfViewModel)DataContext;
                                                     _videoHelper.PrepVideoConf();
                                                 }
                                             }
                                         });
        }
    }
}