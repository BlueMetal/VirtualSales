using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using VirtualSales.Models.Annotations;

namespace VirtualSales.iOS.Annotations
{
    public class AnnotationEventArgs : EventArgs
    {
        public Annotation Annotation { get; private set; }
        public AnnotationEventArgs(Annotation annotation)
        {
            Annotation = annotation;
        }
    }
}