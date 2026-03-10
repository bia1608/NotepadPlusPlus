using Microsoft.Win32;
using NotepadPlusPlus.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls; // Ensure this is present for MenuItem, TabControl, etc.
using System.Windows.Controls.Primitives; // Ensure this is present for GridSplitter
using System.Windows.Data; // For RoutedPropertyChangedEventArgs
using System.Windows.Media; // For Border
using System; // For DateTime


namespace NotepadPlusPlus
{
    public partial class MainWindow : Window
    {
        int fileCnt = 0;
        ObservableCollection<MyFile> files = new ObservableCollection<MyFile>();
        ObservableCollection<FileSystemItem> folderItems = new ObservableCollection<FileSystemItem>();


        public MainWindow()
        {
            InitializeComponent();
            EditorsTabControl.ItemsSource = files;
        }

        private void AddNewTab()
        {
            var newFile = new MyFile()
            {
                Title = $"new {++fileCnt}.txt",
                Content = string.Empty,
                LastModified = DateTime.Now
            };
            files.Add(newFile);
            EditorsTabControl.SelectedItem = newFile;
        }

        private void NewFileButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewTab();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string content = File.ReadAllText(filePath);
                var newFile = new MyFile(System.IO.Path.GetFileName(filePath), content)
                {
                    LastModified = DateTime.Now,
                    FilePath = filePath
                };
                newFile.MarkAsSaved();
                files.Add(newFile);
                EditorsTabControl.SelectedItem = newFile;
            }

        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            var currentFile = EditorsTabControl.SelectedItem as MyFile;
            if (currentFile != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllText(filePath, currentFile.Content);
                    currentFile.Title = System.IO.Path.GetFileName(filePath);
                    currentFile.LastModified = DateTime.Now;
                    currentFile.MarkAsSaved();
                }
            }
        }

        private void SaveAsFileButton_Click(object sender, RoutedEventArgs e)
        {
            var currentFile = EditorsTabControl.SelectedItem as MyFile;
            if (currentFile != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = currentFile.Title;
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    File.WriteAllText(filePath, currentFile.Content);
                    currentFile.Title = System.IO.Path.GetFileName(filePath);
                    currentFile.LastModified = DateTime.Now;
                    currentFile.MarkAsSaved();
                }
            }
        }

        private void CutButton_Click(object sender, RoutedEventArgs e)
        {
            var currentFile = EditorsTabControl.SelectedItem as MyFile;
            if (currentFile != null)
            {
                Clipboard.SetText(currentFile.Content);
                currentFile.Content = string.Empty;
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            var currentFile = EditorsTabControl.SelectedItem as MyFile;
            if (currentFile != null)
            {
                Clipboard.SetText(currentFile.Content);
            }

        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            var currentFile = EditorsTabControl.SelectedItem as MyFile;
            if (currentFile != null)
            {
                if (Clipboard.ContainsText())
                {
                    currentFile.Content += Clipboard.GetText();
                }
            }

        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            var currentFile = EditorsTabControl.SelectedItem as MyFile;
            if (currentFile != null)
            {
                currentFile.Undo();
            }
        }

        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            var currentFile = EditorsTabControl.SelectedItem as MyFile;
            if (currentFile != null)
            {
                currentFile.Redo();
            }
        }

        private void FolderTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is FileSystemItem item && !item.IsDirectory && File.Exists(item.FullPath))
            {
                OpenFile(item.FullPath);
            }
        }

        private void OpenFile(string fullPath)
        {
            string content = File.ReadAllText(fullPath);
            var newFile = new MyFile(System.IO.Path.GetFileName(fullPath), content)
            {
                LastModified = DateTime.Now,
                FilePath = fullPath
            };
            newFile.MarkAsSaved();
            files.Add(newFile);
            EditorsTabControl.SelectedItem = newFile;
        }

        private void ToggleFolderExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (FolderExplorerColumn.Width.Value > 0)
            {
                FolderExplorerColumn.Width = new GridLength(0);
            }
            else
            {
                FolderExplorerColumn.Width = new GridLength(250);
            }
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select Folder",
                Filter = "Folders|*.none",
                Title = "Select a folder to open"
            };

            if (dialog.ShowDialog() == true)
            {
                string? folderPath = System.IO.Path.GetDirectoryName(dialog.FileName);
                if (!string.IsNullOrEmpty(folderPath))
                {
                    LoadFolder(folderPath);

                    // Show the folder explorer if hidden
                    if (FolderExplorerColumn.Width.Value == 0)
                    {
                        FolderExplorerColumn.Width = new GridLength(250);
                        FolderExplorerMenuItem.IsChecked = true;
                    }
                }
            }
        }

        private void LoadFolder(string? folderPath)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                return;
            folderItems.Clear();
            var rootItem = new FileSystemItem(folderPath);
            folderItems.Add(rootItem);
            FolderTreeView.ItemsSource = folderItems;
        }

        private void LoadDirectory(FileSystemItem rootItem)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(rootItem.FullPath))
                {
                    var dirItem = new FileSystemItem(dir);
                    rootItem.Children.Add(dirItem);
                    LoadDirectory(dirItem);
                }
                foreach (var file in Directory.GetFiles(rootItem.FullPath))
                {
                    var fileItem = new FileSystemItem(file);
                    rootItem.Children.Add(fileItem);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle access exceptions if needed

            }

        }

        private string _sourceFolderPath = ""; // Variabila "memorie" pentru copy-paste

        private void CopyPath_Click(object sender, RoutedEventArgs e)
        {
            if (FolderTreeView.SelectedItem is FileSystemItem item)
            {
                Clipboard.SetText(item.FullPath);
            }
        }

        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            if (FolderTreeView.SelectedItem is FileSystemItem item && item.IsDirectory)
            {
                string newPath = Path.Combine(item.FullPath, "New_File.txt");
                File.WriteAllText(newPath, ""); // Creează un fișier gol instant
                MessageBox.Show("Fișier creat!");
            }
        }

        private void CopyFolder_Click(object sender, RoutedEventArgs e)
        {
            if (FolderTreeView.SelectedItem is FileSystemItem item && item.IsDirectory)
            {
                _sourceFolderPath = item.FullPath;
                PasteMenuItem.IsEnabled = true; // Activăm opțiunea Paste după ce am copiat ceva
                MessageBox.Show("Folder selectat pentru copiere.");
            }
        }

        private void PasteFolder_Click(object sender, RoutedEventArgs e)
        {
            if (FolderTreeView.SelectedItem is FileSystemItem targetItem && !string.IsNullOrEmpty(_sourceFolderPath))
            {
                string folderName = Path.GetFileName(_sourceFolderPath);
                string destination = Path.Combine(targetItem.FullPath, folderName);


                CopyDirectoryContents(_sourceFolderPath, destination);
                MessageBox.Show("Folder lipit cu succes!");
            }
        }

        private void CopyDirectoryContents(string source, string dest)
        {
            Directory.CreateDirectory(dest);
            foreach (string f in Directory.GetFiles(source))
                File.Copy(f, Path.Combine(dest, Path.GetFileName(f)), true);
            foreach (string d in Directory.GetDirectories(source))
                CopyDirectoryContents(d, Path.Combine(dest, Path.GetFileName(d)));
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new SearchWindow(files);
            win.Show();

        }
    }
}