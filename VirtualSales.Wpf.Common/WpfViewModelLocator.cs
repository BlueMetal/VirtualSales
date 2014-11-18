using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core;
using VirtualSales.Core.AppServices;
using VirtualSales.Wpf.AppServices;

namespace VirtualSales.Wpf
{
    public class WpfViewModelLocator : ViewModelLocator
    {
        public WpfViewModelLocator(bool isAgent) : base(isAgent)
        {
        }

        protected override void InitializeTypes()
        {
            Container.Register<ISettings>(new Settings(Mode));
            //Container.Register<IPdfDocumentTemplateProvider, PdfDocumentTemplateProvider>();
            Container.Register<IPlatformServices, PlatformServices>();
        }
    }
}
