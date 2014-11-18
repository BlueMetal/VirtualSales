using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using VirtualSales.Core.Models.Annotations;
using MonoTouch.CoreGraphics;
using VirtualSales.Models.Annotations;

namespace VirtualSales.iOS.Annotations
{
    public class IOSAnnotationRenderer : AnnotationRenderer<CGContext>
    {
        protected override void DrawAdHoc(CGContext context, IList<AnnotationPoint> points)
        {
            if (points.Count < 2)
                return;

            int j = 1;
            for (int i = 0; i < points.Count - 1; i++)
            {
                var point_i = points[i];
                var point_j = points[j];

                context.MoveTo(point_i.X, point_i.Y);
                context.AddLineToPoint(point_j.X, point_j.Y);
                context.StrokePath();

                j++;
            }
        }
        protected override void DrawLine(CGContext context, AnnotationPoint startPoint, AnnotationPoint endPoint)
        {
            context.MoveTo(startPoint.X, startPoint.Y);
            context.AddLineToPoint(endPoint.X, endPoint.Y);
            context.StrokePath();
        }
        protected override void DrawEllipse(CGContext context, AnnotationPoint startPoint, AnnotationPoint endPoint)
        {
            var rect = new System.Drawing.RectangleF(startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
            context.AddEllipseInRect(rect);
            context.StrokeEllipseInRect(rect);
        }
        protected override void DrawRectangle(CGContext context, AnnotationPoint startPoint, AnnotationPoint endPoint)
        {
            var rect = new System.Drawing.RectangleF(startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
            context.AddRect(rect);
            context.StrokeRect(rect);
        }
    }
}