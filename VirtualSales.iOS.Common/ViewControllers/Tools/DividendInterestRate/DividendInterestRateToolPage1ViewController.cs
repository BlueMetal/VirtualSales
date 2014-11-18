using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels.Tools;
using Infragistics;
using System.Collections.Generic;

namespace VirtualSales.iOS
{
    public partial class DividendInterestRateToolPage1ViewController : ReactiveViewController, IViewFor<DividendInterestRatesViewModel.Page1>
	{
        private DividendInterestRatesViewModel.Page1 _viewModel;

        public DividendInterestRateToolPage1ViewController()
            : base("DividendInterestRateToolPage1ViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            IGGridView gridView = new IGGridView(new RectangleF(0, 30, this.View.Frame.Size.Width, this.View.Frame.Size.Height), IGGridViewStyle.IGGridViewStyleDefault);
            gridView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            gridView.WeakDataSource = new DividendInterestRateTableDataProvider(ViewModel);
            gridView.Delegate = new DividendInterestRateTableDelegate();
            gridView.SelectionType = IGGridViewSelectionType.IGGridViewSelectionTypeNone;

            this.View.AddSubview(gridView);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (DividendInterestRatesViewModel.Page1)value; }
        }

        public DividendInterestRatesViewModel.Page1 ViewModel
        {
            get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
        }

        public class DividendInterestRateTableDelegate : IGGridViewDelegate
        {
            public override float ResolveRowHeight(IGGridView gridView, IGRowPath path)
            {
                return 28;
            }
        }

        public class DividendInterestRateTableDataProvider : IGGridViewDataSource
        {
            private List<Tuple<DateTime, double>> _rates;
            public DividendInterestRateTableDataProvider(DividendInterestRatesViewModel.Page1 model)
            {
                _rates = new List<Tuple<DateTime, double>>(model.Rates);                
            }
            
            public override int NumberOfRowsInSection(IGGridView grid, int sectionIndex)
            {
                return _rates.Count;
            }

            public override int NumberOfColumns(IGGridView gridView)
            {
                return 2;
            }

            public override string TitleForHeaderInColumn(IGGridView gridView, int column)
            {
                return column == 0 ? "Year" : "Dividend Interest Rate";
            }

            public override IGGridViewCell CreateCell(IGGridView grid, IGCellPath path)
            {
                IGGridViewCell cell = (IGGridViewCell)grid.DequeueReusableCell("Cell");
                if (cell == null)
                    cell = new IGGridViewCell("Cell");

                var item = _rates[path.RowIndex];

                cell.TextLabel.Text = path.ColumnIndex == 0 ? item.Item1.ToString("yyyy") : item.Item2.ToString("P2");

                return cell;
            }
        }
	}
}

