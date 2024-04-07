using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
                   PropertyChangedCallback = (obj, e) =>
                   {
                       var viewer = (DocumentViewer)obj;

                       var path = GetPath(viewer);
                       if (path is null) return;
                       XpsDocument doc = new XpsDocument(path, System.IO.FileAccess.Read);

                       viewer.Document = doc.GetFixedDocumentSequence();
                   }
               }
            );
    }
}
