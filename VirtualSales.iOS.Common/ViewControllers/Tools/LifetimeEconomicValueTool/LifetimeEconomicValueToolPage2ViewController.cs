using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels.Tools;
using Infragistics;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace VirtualSales.iOS
{
    public partial class LifetimeEconomicValueToolPage2ViewController : ReactiveViewController, IViewFor<LifetimeEconomicValueViewModel.Page2>
	{
        public LifetimeEconomicValueToolPage2ViewController()
            : base("LifetimeEconomicValueToolPage2ViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

        public class CustomDelegate : IGChartViewDelegate
        {
            public CustomDelegate()
            {
            }

            public override string ResolveLabelForAxis(IGChartView chartView, IGAxis axis, NSObject item)
            {
                if (axis.Key.Equals("yAxis"))
                {
                    var num = (NSNumber)item;
                    return string.Format("{0:C0}", num.DoubleValue);
                }
                else
                {
                    var point = (IGCategoryDatePoint)item;
                    return point.Label;
                }
            }
        }

        private UILabel _invalidInputsLabel;
        private IGChartView _chart;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _chart = new IGChartView(new RectangleF(this.View.Frame.X, 30, this.View.Frame.Width, this.View.Frame.Height - 40));
            _chart.Delegate = new CustomDelegate();
            _chart.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;

            var xAxis = new IGCategoryXAxis("xAxis");
            var yAxis = new IGNumericYAxis("yAxis");

            _chart.AddAxis(xAxis);
            _chart.AddAxis(yAxis);
            yAxis.Minimum = 0;

            var columnSeries = new IGAreaSeries("columnSeries");
            columnSeries.XAxis = xAxis;
            columnSeries.YAxis = yAxis;
            SetDataSourceOnSeries(columnSeries);
            _chart.AddSeries(columnSeries);

            ViewModel.DataPoints.Changed.ObserveOn(RxApp.MainThreadScheduler).Subscribe(p => SetDataSourceOnSeries(columnSeries));


            _invalidInputsLabel = new UILabel(new RectangleF(this.View.Frame.X+60, 40, this.View.Frame.Width-60, this.View.Frame.Height - 40));
            _invalidInputsLabel.Lines = 0;
            _invalidInputsLabel.LineBreakMode = UILineBreakMode.WordWrap;
            _invalidInputsLabel.Text = ViewModel.InvalidInputsMessage;
            _invalidInputsLabel.TextAlignment = UITextAlignment.Left;

            this.View.AddSubview(_invalidInputsLabel);
            this.View.AddSubview(_chart);


            Action assignVisibility = () =>
                    {
                        _invalidInputsLabel.Hidden = ViewModel.HasValidInputs;
                        _chart.Hidden = !ViewModel.HasValidInputs;
                    };
            assignVisibility();

            ViewModel.WhenAnyValue(p => p.HasValidInputs)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(p => assignVisibility());
        }

        private void SetDataSourceOnSeries(IGSeries series)
        {
            var values = new List<NSObject>(ViewModel.DataPoints.Select(p => new NSNumber(p.Value)));
            var labels = new List<NSObject>(ViewModel.DataPoints.Select(p => new NSString(p.Category)));

            var source = new IGCategoryDateSeriesDataSourceHelper();
            source.Values = values.ToArray();
            source.Labels = labels.ToArray();
            series.DataSource = source;
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (LifetimeEconomicValueViewModel.Page2)value; }
        }

        private LifetimeEconomicValueViewModel.Page2 _viewModel;

        public LifetimeEconomicValueViewModel.Page2 ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }
	}
}

