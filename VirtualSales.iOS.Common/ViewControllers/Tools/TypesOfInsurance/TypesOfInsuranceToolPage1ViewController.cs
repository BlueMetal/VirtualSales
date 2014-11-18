using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using ReactiveUI.Cocoa;
using VirtualSales.Core.ViewModels.Tools;
using System.Collections.Generic;

namespace VirtualSales.iOS
{
	public partial class TypesOfInsuranceToolPage1ViewController : ReactiveViewController, IViewFor<TypesOfInsuranceViewModel.Page1>
	{
	    private TypesOfInsuranceViewModel.Page1 _viewModel;

	    public TypesOfInsuranceToolPage1ViewController()
            : base("TypesOfInsuranceToolPage1ViewController", null)
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
//            NSArray * items = ...;
//NSMutableString * bulletList = [NSMutableString stringWithCapacity:items.count*30];
//for (NSString * s in items)
//{
//   [bulletList appendFormat:@"\u2022 %@\n", s];
//}
//textView.text = bulletList;



            var term = new List<string>
            {
                "Income tax-free benefit",
                "Term period",
                "Fixed premiums",
                "No investment risk"
            };

            var universal = new List<string>
            {
                "Income tax-free death benefit",
                "Coverage period based on premiums paid",
                "Has cash or account value",
                "Can borrow against cash value",
                "Flexible premiums",
                "Can use cash value to pay premiums",
                "Insurer absorbs minimum guarantee risk; policyholder absorbs risk above that"
            };
            var variable = new List<string>
            {
                "Income tax-free death benefit",
                "Coverage period based on premiums paid",
                "Has cash or account value",
                "Can borrow against cash value",
                "Flexible premiums",
                "Can use cash value to pay premiums",
                "Insurer absorbs minimum guarantee risk; policyholder absorbs risk above that"
            };
            var whole = new List<string>
            {
                  "Income tax-free death benefit",
                  "Lifetime coverage",
                  "Has cash value",
                  "Can borrow against cash value",
                  "Fixed premium with some flexibility",
                  "Can use cash value to pay premiums",
                "Insurer will absorb any investment risk"
            };

            String termtext = "";
            foreach (var item in term)
            {
                termtext += String.Format("\u2022 {0}\n", item);
            }
            String universaltext = "";
            foreach (var item in universal)
            {
                universaltext += String.Format("\u2022 {0}\n", item);
            }
            String variabletext = "";
            foreach (var item in variable)
            {
                variabletext += String.Format("\u2022 {0}\n", item);
            }
            String wholetext = "";
            foreach (var item in whole)
            {
                wholetext += String.Format("\u2022 {0}\n", item);
            }

            wholeLifeTextView.Text = wholetext;
            termLifeTextView.Text = termtext;
            variableLifeTextView.Text = variabletext;
            universalLifeTextView.Text = universaltext;


		}

	    object IViewFor.ViewModel
	    {
	        get { return ViewModel; }
	        set { ViewModel = (TypesOfInsuranceViewModel.Page1)value; }
	    }

	    public TypesOfInsuranceViewModel.Page1 ViewModel
	    {
	        get { return _viewModel; }
            set { this.RaiseAndSetIfChanged(ref _viewModel, value); }
	    }
	}
}

