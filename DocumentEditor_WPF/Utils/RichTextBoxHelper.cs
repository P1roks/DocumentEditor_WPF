using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DocumentEditor_WPF.Utils
{
    public class RichTextBoxHelper : DependencyObject
    {
        public static string GetPath(DependencyObject obj) => (string)obj.GetValue(PathProperty);

        public static void SetPath(DependencyObject obj, string value) => obj.SetValue(PathProperty, value);

        public static readonly DependencyProperty PathProperty =
            DependencyProperty.RegisterAttached(
               "Path",
               typeof(string),
               typeof(RichTextBoxHelper),
               new FrameworkPropertyMetadata
               {
                   BindsTwoWayByDefault = true,
                   PropertyChangedCallback = (obj, e) =>
                   {
                       var rtb = (RichTextBox)obj;

                       var path = GetPath(rtb);
                       if (path is null) return;

                       TextRange range = new(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                       FileStream fstream = new(path, FileMode.OpenOrCreate);
                       string dataformat = Utils.GetPathDataFormat(Path.GetExtension(path));
                       range.Load(fstream, DataFormats.Rtf);
                       fstream.Close();
                   }
               }
            );
    }
}
