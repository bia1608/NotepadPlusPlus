using Microsoft.Win32;
using NotepadPlusPlus.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotepadPlusPlus
{
    public partial class MainWindow : Window
    {
        int fileCnt = 0;
        ObservableCollection<MyFile> files = new ObservableCollection<MyFile>();

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
                    LastModified = DateTime.Now
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
    }
}