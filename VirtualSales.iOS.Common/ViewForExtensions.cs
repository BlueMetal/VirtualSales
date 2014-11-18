using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveUI;
using VirtualSales.Core;

namespace VirtualSales.iOS
{
    public static class ViewForExtensions
    {

        private static readonly Lazy<IViewModelLocator> _locator = new Lazy<IViewModelLocator>(GetLocator);
        public static T GetViewModel<T>(this IViewFor<T> This) where T : class
        {
            return (T)_locator.Value.NavigationService.CurrentViewModel;
        }

        public static IViewModelLocator GetViewModelLocator<T>(this IViewFor<T> This) where T : class
        {
            return _locator.Value;
        }

        private static IViewModelLocator GetLocator()
        {
            var appDel = (WhiteBrandAppDelegate)UIApplication.SharedApplication.Delegate;
            return appDel.ViewModelLocator;
        }
    }
}