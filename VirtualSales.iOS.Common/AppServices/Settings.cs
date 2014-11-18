using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using VirtualSales.Core;
using VirtualSales.Core.AppServices;

namespace VirtualSales.iOS.AppServices
{
    public class Settings : ISettings
    {
        public Settings(AppMode mode)
        {
            Mode = mode;
        }
        public string GetValue(string key)
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary(key).ToString();
        }

        public AppMode Mode { get; private set; }
    }
}