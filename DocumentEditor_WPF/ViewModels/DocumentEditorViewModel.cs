using DocumentEditor_WPF.Models;
using DocumentEditor_WPF.Utils;
using Microsoft.Win32;
using IO = System.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DocumentEditor_WPF.ViewModels
{

    public class DocumentEditorViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<File> Files { get; set; } = new();

        public File SelectedFile { get; set; }

        public ICommand CloseFileCommand { get => new RelayCommand(CloseFile); }
        public ICommand NewFileCommand { get => new RelayCommand(NewFile); }
        public ICommand OpenFileCommand { get => new RelayCommand(OpenFile); }
        public ICommand CloseAppCommand { get => new RelayCommand(CloseApp); }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseFile()
        {
            Files.Remove(SelectedFile);
        }

        private void NewFile()
        {
            CreateFileDialog dialog = new();

            if(dialog.ShowDialog() == true)
            {
                var filename = IO.Path.ChangeExtension(dialog.ResponseText, ".rtf");
                var fullPath = IO.Path.Combine(Microsoft.VisualBasic.FileIO.SpecialDirectories.MyDocuments, filename);

                using (IO.StreamWriter sw = IO.File.CreateText(fullPath))
                {
                    sw.WriteLine("{\\rtf1}");
                }
                File created = new()
                {
                    Path = fullPath,
                    Name = filename
                };
                Files.Add(created);
                SelectedFile = created;
                onPropertyChanged(nameof(SelectedFile));
            }
        }

        private void OpenFile()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Pliki tekstowe (*.txt)|*.txt|" +
                         "Tekst Sformatowany (*.rtf)|*.rtf|" +
                         "Pliki XAML (*.xaml)|*.xaml|" +
                         //       "Pliki PDF (*.pdf)|*.pdf|" +
                         "Pliki XPS (*.xps)|*.xps"
            };

            if (dialog.ShowDialog() == true)
            {
                File selected = new()
                {
                    Path = dialog.FileName,
                    Name = IO.Path.GetFileName(dialog.FileName)
                };
                Files.Add(selected);
                SelectedFile = selected;
                onPropertyChanged(nameof(SelectedFile));
            }
        }

        private void CloseApp()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
