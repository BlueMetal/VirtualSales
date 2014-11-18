using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualSales.Core;
using VirtualSales.Core.AppServices;
using VirtualSales.iOS.AppServices;

namespace VirtualSales.iOS
{
    public class iOSViewModelLocator : ViewModelLocator
    {
        public iOSViewModelLocator(bool isAgent) : base(isAgent)
        {
        }

        protected override void InitializeTypes()
        {
            Container.Register<ISettings>(new Settings(Mode));
            Container.Register<IPlatformServices, PlatformServices>();
        }
    }
}