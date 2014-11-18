using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VirtualSales.Core.ViewModels.Annotations;
using VirtualSales.Models.Annotations;

namespace VirtualSales.Wpf.Views
{
    public class AnnotationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LineTemplate { get; set; }
        public DataTemplate EllipseTemplate { get; set; }
        public DataTemplate RectangleTemplate { get; set; }
        public DataTemplate AdHocTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var annotation = (AnnotationViewModel)item;
            switch (annotation.Type)
            {
                case AnnotationType.Line:
                    return LineTemplate;
                case AnnotationType.Ellipse:
                    return EllipseTemplate;
                case AnnotationType.AdHoc:
                    return AdHocTemplate;
                case AnnotationType.Rectangle:
                default:
                    return RectangleTemplate;
            }
        }
    }


}
