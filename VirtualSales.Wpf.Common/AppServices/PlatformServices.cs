using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.AppServices;

namespace VirtualSales.Wpf.AppServices
{
    class PlatformServices : IPlatformServices
    {
        public async Task SaveAndLaunchFile(Stream stream, string fileType)
        {
            var name = Path.GetTempFileName() + "." + fileType;


            using (var fs = File.OpenWrite(name))
            {
                await stream.CopyToAsync(fs);
            }

            Process.Start(name);
        }
    }
}
