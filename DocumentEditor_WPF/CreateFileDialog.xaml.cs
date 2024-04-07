using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace DocumentEditor_WPF
{
    /// <summary>
    /// Interaction logic for CreateFileDialog.xaml
    /// </summary>
    public partial class CreateFileDialog : Window
    {
        public CreateFileDialog()
        {
            InitializeComponent();
        }

        public string ResponseText
        {
            get => ResponseTextBox.Text;
            set {  ResponseTextBox.Text = value; } 
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
