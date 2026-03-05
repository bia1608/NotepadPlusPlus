using NotepadPlusPlus.Models;
using System.Collections.ObjectModel;
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
    }
}