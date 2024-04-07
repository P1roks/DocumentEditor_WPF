using DocumentEditor_WPF.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DocumentEditor_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class EditorPage : Window
    {
        public EditorPage()
        {
            InitializeComponent();
        }

        private void Document_TextChanged(object sender, TextChangedEventArgs e)
        {
            var rtb = (RichTextBox)sender;

            var path = RichTextBoxHelper.GetPath(rtb);
            if (path == null) return;

            string dataFormat = Utils.Utils.GetPathDataFormat(path);
            TextRange range = new(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            FileStream fStream = new(path, FileMode.Create, FileAccess.Write);
            range.Save(fStream, dataFormat);
            fStream.Close();
        }
    }
}
