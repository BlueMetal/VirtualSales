using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualSales.Core.AppServices;

namespace VirtualSales.Wpf.AppServices
{
    class PdfDocumentTemplateProvider : IPdfDocumentTemplateProvider
    {
        public Task<Stream> GetTemplate(PdfDocType docType)
        {
            var tcs = new TaskCompletionSource<Stream>();

            try
            {
                var fileName = "";
                if (docType == PdfDocType.FormFill1)
                {
                    fileName = "formfill.pdf";
                }

                var fs = File.Open(Path.Combine("Documents", fileName), FileMode.Open);

                tcs.SetResult(fs);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
            return tcs.Task;
        }
    }
}
