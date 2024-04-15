using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace DocumentEditor_WPF.Utils
{
    public class DocumentViewerHelper : DependencyObject
    {
        public static string GetPath(DependencyObject obj) => (string)obj.GetValue(PathProperty);

        public static void SetPath(DependencyObject obj, string value) => obj.SetValue(PathProperty, value);

        public static readonly DependencyProperty PathProperty =
            DependencyProperty.RegisterAttached(
               "Path",
               typeof(string),
               typeof(DocumentViewerHelper),
               new FrameworkPropertyMetadata
               {
                   BindsTwoWayByDefault = true,
                   PropertyChangedCallback = async (obj, e) =>
                   {
                       var viewer = (DocumentViewer)obj;

                       var path = GetPath(viewer);
                       if (path is null) return;

                       var doc = await Task.Run(() =>
                       {
                           switch (Path.GetExtension(path))
                           {
                               case ".xps":
                                   return new XpsDocument(path, System.IO.FileAccess.Read);
                               case ".pdf":
                                   string xpsPath = Path.ChangeExtension(path, ".xps");

                                   if (!File.Exists(xpsPath))
                                   {
                                       PdfDocument pdfDoc = new();
                                       pdfDoc.LoadFromFile(path);

                                       pdfDoc.SaveToFile(xpsPath, FileFormat.XPS);
                                       pdfDoc.Close();
                                   }

                                   return new XpsDocument(xpsPath, System.IO.FileAccess.Read);
                               default:
                                   return null;
                           }
                       });
                       if(doc is null) return;

                       viewer.Document = doc.GetFixedDocumentSequence();
                   }
               }
            );
    }
}
