using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentEditor_WPF.Utils
{
    public static class Utils
    {
        public static string GetPathDataFormat(string path)
        {
           switch (Path.GetExtension(path))
           {
               case ".rtf":
                   return DataFormats.Rtf;
               case ".xaml":
                   return DataFormats.Xaml;
               case ".txt":
               default:
                   return DataFormats.Text;
           }
        }
    }
}
