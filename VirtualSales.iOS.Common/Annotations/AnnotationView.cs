using System;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using VirtualSales.Core.Models.Annotations;
using System.Collections.Generic;
using VirtualSales.Models.Annotations;
using ReactiveUI.Cocoa;
using System.Reactive.Linq;
using ReactiveUI;
using VirtualSales.Core.ViewModels.Annotations;

namespace VirtualSales.iOS.Annotations
{
    [Register("AnnotationView")]
    public class AnnotationView : ReactiveView
    {
        private IOSAnnotationRenderer renderer = new IOSAnnotationRenderer();
        private AnnotationBuilder currentBuilder = null;

        public AnnotationView(IntPtr ptr) : base(ptr)
        {
            Initialize();
        }
        public AnnotationView(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }
        public AnnotationView()
        {
            Initialize();
        }
        private void Initialize()
        {
            UserInteractionEnabled = false;
            BackgroundColor = UIColor.Clear;
        }

        private void HookupAnnotationsModel()
        {
            AnnotationViewModel.WhenAny(v => v.Annotations, v => v.Value).
                ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => SetNeedsDisplay());

            if (AnnotationViewModel is AgentAnnotationViewModel)
            {
                var vm = (AgentAnnotationViewModel)AnnotationViewModel;
                vm.AnnotationTools.WhenAny(p => p.Type, p => p.Value)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(p => Toggle(p != null));
            }
        }

        private void Toggle(bool enabled)
        {
            UserInteractionEnabled = enabled;
        }

        private AnnotationPoint ToAnnotationPoint(PointF pt)
        {
            var point = new AnnotationPoint() { X = pt.X, Y = pt.Y };
            return point;
        }
        private void PopulateCurrentMove(PointF pt, AgentAnnotationViewModel vm)
        {
            var point = ToAnnotationPoint(pt);
            var type = vm.AnnotationTools.Type.Value;

            currentBuilder = vm.CreateBuilder(type);
            currentBuilder.OnNextPoint(point);

            var annotation = vm.CreateAnnotationViewModel(currentBuilder.Annotation);
            vm.AddInProgressAnnotation(annotation);
        }

        private static readonly CGColor StokeColor = UIColor.FromRGB(255, 0, 0).CGColor;

        public override void Draw(System.Drawing.RectangleF rect)
        {
            CGContext context = UIGraphics.GetCurrentContext();
            context.ClearRect(rect);

            context.SetFillColor(0f, 0f);
            context.FillRect(rect);

            context.SetLineWidth(2.0f);
            context.SetStrokeColor(StokeColor);

            if (AnnotationViewModel == null) return;

            foreach (var annotation in AnnotationViewModel.Annotations)
            {
                renderer.Draw(context, annotation);
            }
        }


        public void Refresh()
        {
            SetNeedsDisplay();
        }

        private UITouch GetTouch(NSSet touches)
        {
            var touch = touches.ToArray<UITouch>()[0];
            return touch;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
           
            if (AnnotationViewModel == null || AnnotationViewModel.SurfaceSize == null || !(AnnotationViewModel is AgentAnnotationViewModel)) return;
            var vm = (AgentAnnotationViewModel)AnnotationViewModel;
            if (!vm.AnnotationTools.Type.HasValue) return;

            vm.IsEditing = true;
            var point = GetTouch(touches).LocationInView(this);
            PopulateCurrentMove(point, vm);
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            if (AnnotationViewModel == null || AnnotationViewModel.SurfaceSize == null || !(AnnotationViewModel is AgentAnnotationViewModel)) return;
            var vm = (AgentAnnotationViewModel)AnnotationViewModel;

            var pt = GetTouch(touches).LocationInView(this);
            var point = ToAnnotationPoint(pt);
            var screensize = vm.SurfaceSize;
            if (point.X < 0 || point.Y < 0 || point.X > screensize.Width || point.Y > screensize.Height)
            {
                WrapUpMove(vm);
                return;
            }

            currentBuilder.OnNextPoint(point);
            SetNeedsDisplay();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            if (AnnotationViewModel == null || AnnotationViewModel.SurfaceSize == null || !(AnnotationViewModel is AgentAnnotationViewModel)) return;
            var vm = (AgentAnnotationViewModel)AnnotationViewModel;

            WrapUpMove(vm);
        }

        private void WrapUpMove(AgentAnnotationViewModel vm)
        {
            SetNeedsDisplay();

            vm.AnnotationTools.Type = null;
            vm.AnnotationTools.ClearAllActive();
            vm.IsEditing = false;
        }

        private UserAnnotationViewModel _annotationViewModel;
        public UserAnnotationViewModel AnnotationViewModel
        {
            get { return _annotationViewModel; }
            set
            {
                RaiseAndSetIfChanged(ref _annotationViewModel, value);
                HookupAnnotationsModel();
            }
        }
    }
}