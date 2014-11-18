using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VirtualSales.Core.Models.Annotations;
using VirtualSales.Core.ViewModels;
using VirtualSales.Core.ViewModels.Annotations;
using VirtualSales.Models.Annotations;

namespace VirtualSales.Wpf.Views
{
    public class AnnotationViewModelAddedEventsArgs : EventArgs
    {
        public AnnotationViewModel Annotation { get; private set; }
        public AnnotationViewModelAddedEventsArgs(AnnotationViewModel annotation)
        {
            Annotation = annotation;
        }
    }

    public class AnnotationCanvas : Canvas
    {
        public static DependencyProperty CurrentAnnotationTypeProperty = 
            DependencyProperty.Register("CurrentAnnotationType",
                                        typeof(Nullable<AnnotationType>), typeof(AnnotationCanvas), 
                                        new PropertyMetadata(HandleCurrentAnnotationChanged));

        public static DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(AnnotationCanvas));

        private AnnotationViewModel _currentAnnotation;
        private AnnotationBuilder _currentBuilder;
        private static SolidColorBrush ClickableBrush = new SolidColorBrush(Colors.Transparent);

        private static void HandleCurrentAnnotationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            var canvas = (AnnotationCanvas)obj;
            if(e.NewValue != null) {
                canvas.Background = ClickableBrush;
            } else {
                canvas.Background = null;
            }
        }

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        public Nullable<AnnotationType> CurrentAnnotationType
        {
            get { return (Nullable<AnnotationType>)GetValue(CurrentAnnotationTypeProperty); }
            set { SetValue(CurrentAnnotationTypeProperty, value); }
        }

        public event EventHandler<AnnotationViewModelAddedEventsArgs> AnnotationAdded;

        public AnnotationCanvas()
            : base()
        {
            MouseDown += AnnotationCanvas_MouseDown;
            MouseUp += AnnotationCanvas_MouseUp;
            MouseMove += AnnotationCanvas_MouseMove;
        }

        protected void OnAnnotationViewModelAdded(AnnotationViewModel annotation)
        {
            var ev = AnnotationAdded;
            if (ev != null)
            {
                ev(this, new AnnotationViewModelAddedEventsArgs(annotation));
            }
        }

        void AnnotationCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (CurrentAnnotationType == null || _currentBuilder == null) return;
            var p = e.GetPosition(this);

            // if the point is outside the bounds of this canvas, just finish the move. 
            if (p.X < 0 || p.Y < 0 || p.X > this.ActualWidth || p.Y > this.ActualWidth) 
            {
                WrapUpMove();
                return;
            }

            // add the point to the builder
            _currentBuilder.OnNextPoint(ConvertPoint(p));
            _currentAnnotation.OnChanged();
        }

        void AnnotationCanvas_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CurrentAnnotationType == null) return;
            WrapUpMove();
        }

        private void WrapUpMove()
        {
            _currentBuilder = null;
            _currentAnnotation = null;

            ReleaseMouseCapture();

            IsEditing = false;
            CurrentAnnotationType = null;
        }

        void AnnotationCanvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CurrentAnnotationType == null) return;

            this.CaptureMouse();
            IsEditing = true;

            var p = e.GetPosition(this);
            var vm = DataContext as AgentAnnotationViewModel;


            _currentBuilder = vm.CreateBuilder(CurrentAnnotationType.Value);
            _currentAnnotation = vm.CreateAnnotationViewModel(_currentBuilder.Annotation);
            vm.AddInProgressAnnotation(_currentAnnotation);

            _currentBuilder.OnNextPoint(ConvertPoint(p));
        }

        /// <summary>
        /// converts a WPF point into a AnnotationPoint
        /// </summary>
        private AnnotationPoint ConvertPoint(Point p)
        {
            return new AnnotationPoint
            {
                X = (float)p.X,
                Y = (float)p.Y
            };
        }
    }
}
