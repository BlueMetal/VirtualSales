// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Linq;
using System.Reactive.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using VirtualSales.iOS.Annotations;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Models.Annotations;
using VirtualSales.Core.ViewModels;
using VirtualSales.Core.ViewModels.Annotations;
using MonoKit.UI.ViewDeck;

namespace VirtualSales.iOS.ViewControllers
{
    public partial class AnnotationsToolViewController : ReactiveViewController, IViewFor<AnnotationToolViewModel>
	{
        private AnnotationToolViewModel _viewModel;
        //private IDisposable _clearOnAddedSub;

		public AnnotationsToolViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        private void DisableViewDeck()
        {
            var vd = ((WhiteBrandAppDelegate)UIApplication.SharedApplication.Delegate).ViewDeckController;
            vd.PanningMode = ViewDeckPanningMode.NoPanning;
        }
        private void EnableViewDeck()
        {
            var vd = ((WhiteBrandAppDelegate)UIApplication.SharedApplication.Delegate).ViewDeckController;
            vd.PanningMode = ViewDeckPanningMode.FullViewPanning;
        }

        private void HookupViewModel()
        {
            this.Bind(ViewModel, v => v.EllipseActive, v => v.toggleEllipseButton.Selected);
            this.Bind(ViewModel, v => v.RectActive, v => v.toggleRectangleButton.Selected);
            this.Bind(ViewModel, v => v.LineActive, v => v.toggleLineButton.Selected);
            this.Bind(ViewModel, v => v.FreeDrawActive, v => v.toggleFreeDrawButton.Selected);

            this.BindCommand(ViewModel, v => v.EllipseCommand, v => v.toggleEllipseButton);
            this.BindCommand(ViewModel, v => v.RectCommand, v => v.toggleRectangleButton);
            this.BindCommand(ViewModel, v => v.LineCommand, v => v.toggleLineButton);
            this.BindCommand(ViewModel, v => v.FreeDrawCommand, v => v.toggleFreeDrawButton);
            this.BindCommand(ViewModel, v => v.UndoCommand, v => v.undoButton);

            ViewModel.WhenAnyValue(p => p.Type, p => p).Subscribe(p =>
             {
                 if (p.HasValue) DisableViewDeck();
                 else EnableViewDeck();
             });

        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AnnotationToolViewModel)value; }
        }

        public AnnotationToolViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                RaiseAndSetIfChanged(ref _viewModel, value);
                HookupViewModel();
            }
        }
	}
}
