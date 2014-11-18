using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.ViewModels;
using VirtualSales.Core.ViewModels.Annotations;
using VirtualSales.Models.Annotations;

namespace VirtualSales.iOS.Annotations
{
    public abstract class AnnotationRenderer<TContext>
    {
        public void Draw(TContext context, AnnotationViewModel annotation)
        {
            if (annotation.SurfacePoints.Count == 0)
                return; // nothing happens

            AnnotationPoint p1, p2;
            p1 = annotation.SurfacePoints[0];
            p2 = annotation.SurfacePoints.Count == 1 ? p1 : annotation.SurfacePoints[1];

            switch (annotation.Type)
            {
                case AnnotationType.AdHoc:
                    DrawAdHoc(context, annotation.SurfacePoints);
                    break;
                case AnnotationType.Ellipse:
                    DrawEllipse(context, p1, p2);
                    break;
                case AnnotationType.Line:
                    DrawLine(context, p1, p2);
                    break;
                case AnnotationType.Rectangle:
                    DrawRectangle(context, p1, p2);
                    break;
            }
        }

        protected abstract void DrawAdHoc(TContext context, IList<AnnotationPoint> points);
        protected abstract void DrawEllipse(TContext context, AnnotationPoint startPoint, AnnotationPoint endPoint);
        protected abstract void DrawRectangle(TContext context, AnnotationPoint startPoint, AnnotationPoint endPoint);
        protected abstract void DrawLine(TContext context, AnnotationPoint startPoint, AnnotationPoint endPoint);
    }
}
