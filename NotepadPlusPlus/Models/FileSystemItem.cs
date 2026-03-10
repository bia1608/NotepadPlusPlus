using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace NotepadPlusPlus.Models
{
    internal class FileSystemItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsDirectory { get; set; }
        public ObservableCollection<FileSystemItem> Children { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                NotifyPropertyChanged();
                if (value)
                    LoadChildren();
            }
        }

        public FileSystemItem(string path)
        {
            FullPath = path;
            Name = string.IsNullOrEmpty(Path.GetFileName(path)) ? path : Path.GetFileName(path);
            IsDirectory = Directory.Exists(path);
            Children = new ObservableCollection<FileSystemItem>();
            if (IsDirectory)
                Children.Add(null);
        }

        private void LoadChildren()
        {
            if (!IsDirectory)
                return;

            if (Children.Count == 1 && Children[0] == null)
            {
                Children.Clear();

                try
                {
                    foreach (var dir in Directory.GetDirectories(FullPath))
                    {
                        var dirItem = new FileSystemItem(dir);
                        Children.Add(dirItem);
                    }

                    foreach (var file in Directory.GetFiles(FullPath))
                    {
                        var fileItem = new FileSystemItem(file);
                        Children.Add(fileItem);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    Children.Add(new FileSystemItem("[Access Denied]")
                    {
                        IsDirectory = false
                    });
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}