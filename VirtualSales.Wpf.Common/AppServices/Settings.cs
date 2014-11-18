using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core;
using VirtualSales.Core.AppServices;

namespace VirtualSales.Wpf.AppServices
{
    class Settings : ISettings
    {
        public Settings(AppMode mode)
        {
            Mode = mode;
        }
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public AppMode Mode { get; private set; }
    }
}
