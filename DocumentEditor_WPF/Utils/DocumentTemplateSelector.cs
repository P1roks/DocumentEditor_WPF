using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DocumentEditor_WPF.Models;

namespace DocumentEditor_WPF.Utils
{
    public class DocumentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EditableDocumentTemplate { get; set; }
        public DataTemplate ViewableDocumentTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var extension = System.IO.Path.GetExtension(((File)item).Path).ToLower();
            Trace.WriteLine(extension);

            switch (extension)
            {
                case ".rtf":
                case ".txt":
                case ".xaml":
                    return EditableDocumentTemplate;
                case ".pdf":
                case ".xps":
                default:
                    return ViewableDocumentTemplate;
            }
        }
    }
}
