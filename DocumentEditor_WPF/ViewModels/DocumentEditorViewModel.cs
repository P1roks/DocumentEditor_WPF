using DocumentEditor_WPF.Models;
using DocumentEditor_WPF.Utils;
using Microsoft.Win32;
using IO = System.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using EnumExtensions;

namespace DocumentEditor_WPF.ViewModels
{

    public class DocumentEditorViewModel : INotifyPropertyChanged
    {
        private static readonly List<string> viewableExtensions = new() { ".pdf", ".txt", ".xps", ".rtf", ".xaml" };
        public static readonly List<string> SortOptions =
            Enum.GetValues(typeof(Enums.SortOptions)).Cast<Enums.SortOptions>().Select(x => x.toString()).ToList();
        public static readonly List<string> FilterOptions = 
            Enum.GetValues(typeof(Enums.FilterOptions)).Cast<Enums.FilterOptions>().Select(x => x.toString()).ToList();

        public ObservableCollection<File> OpenedFiles { get; set; } = new();
        public ObservableCollection<File> DirectoryFiles { get; set; } = new();
        private List<File> directoryFiles = new();

        public Enums.SortOptions Sort
        {
            set{
                IEnumerable<File> selected;
                switch(value){
                    case Enums.SortOptions.NameDesc:
                        selected = DirectoryFiles.OrderByDescending(x => x.Name).ToList();
                        break;
                    case Enums.SortOptions.NameAsc:
                        selected = DirectoryFiles.OrderBy(x => x.Name).ToList();
                        break;
                    case Enums.SortOptions.ExtensionDesc:
                        selected = DirectoryFiles.OrderByDescending(x => IO.Path.GetExtension(x.Name)).ToList();
                        break;
                    case Enums.SortOptions.ExtensionAsc:
                        selected = DirectoryFiles.OrderBy(x => IO.Path.GetExtension(x.Name)).ToList();
                        break;
                    default:
                        selected = DirectoryFiles;
                        break;
                }
                RepopulateDirectoryFiles(selected);
            }
        }

        public Enums.FilterOptions Filter
        {
            set{
                if(value == Enums.FilterOptions.All)
                {
                    RepopulateDirectoryFiles(directoryFiles);
                }
                else
                {
                    string extension = "." + value.ToString().ToLower();
                    IEnumerable<File> filtered = directoryFiles.Where(x => IO.Path.GetExtension(x.Name) == extension);
                    RepopulateDirectoryFiles(filtered);
                }
            }
        }

        public File SelectedFile { get; set; }

        public ICommand CloseFileCommand { get => new RelayCommand(CloseFile); }
        public ICommand NewFileCommand { get => new RelayCommand(NewFile); }
        public ICommand OpenFileCommand { get => new RelayCommand(OpenFile); }
        public ICommand CloseAppCommand { get => new RelayCommand(CloseApp); }
        public ICommand OpenDirectoryCommand { get => new RelayCommand(OpenDirectory);  }
        public ICommand SelectTreeViewCommand { get => new RelayCommand<File>(SelectTreeView); }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void onPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseFile()
        {
            OpenedFiles.Remove(SelectedFile);
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
                OpenedFiles.Add(created);
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
                         "Pliki PDF (*.pdf)|*.pdf|" +
                         "Pliki XPS (*.xps)|*.xps"
            };

            if (dialog.ShowDialog() == true)
            {
                File selected = new()
                {
                    Path = dialog.FileName,
                    Name = IO.Path.GetFileName(dialog.FileName)
                };
                OpenedFiles.Add(selected);
                SelectedFile = selected;
                onPropertyChanged(nameof(SelectedFile));
            }
        }

        private void OpenDirectory()
        {
            CommonOpenFileDialog dialog = new()
            {
                IsFolderPicker = true
            };

            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                directoryFiles =
                    IO.Directory.GetFiles(dialog.FileName)
                    .Where(file => viewableExtensions.Contains(IO.Path.GetExtension(file)))
                    .Select(file => new File()
                        { 
                            Path = file,
                            Name = IO.Path.GetFileName(file),
                        }
                    ).ToList();

                RepopulateDirectoryFiles(directoryFiles);
            }
        }

        private void CloseApp()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SelectTreeView(File selected)
        {
            if (!OpenedFiles.Contains(selected))
            {
                OpenedFiles.Add(selected);
            }
            SelectedFile = selected;
            onPropertyChanged(nameof(SelectedFile));
        }

        private void RepopulateDirectoryFiles(IEnumerable<File> collection)
        {
            DirectoryFiles.Clear();
            foreach(var file in collection)
            {
                DirectoryFiles.Add(file);
            }
        }
    }
}
