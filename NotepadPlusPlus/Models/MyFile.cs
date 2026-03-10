using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NotepadPlusPlus.Models
{
    public class MyFile : INotifyPropertyChanged
    {
        private string _title;
        private string _content;
        private string _filePath;
        private string _savedContent;
        private DateTime _lastModified;
        private Stack<string> _undoStack = new Stack<string>();
        private Stack<string> _redoStack = new Stack<string>();

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    if (_content != null)
                    {
                        _undoStack.Push(_content);
                        _redoStack.Clear();
                    }
                    _content = value;
                    LastModified = DateTime.Now;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsSaved));
                }
            }
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public DateTime LastModified
        {
            get => _lastModified;
            set
            {
                _lastModified = value;
                OnPropertyChanged();
            }
        }

        public bool IsSaved => _content == _savedContent;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public MyFile()
        {
            _title = "Untitled";
            _content = string.Empty;
            _savedContent = string.Empty;
            _lastModified = DateTime.Now;
        }

        public MyFile(string title, string content)
        {
            _title = title;
            _content = content;
            _savedContent = content;
            _lastModified = DateTime.Now;
        }

        public void MarkAsSaved()
        {
            _savedContent = _content;
            OnPropertyChanged(nameof(IsSaved));
        }

        public void Undo()
        {
            if (CanUndo)
            {
                _redoStack.Push(_content);
                _content = _undoStack.Pop();
                LastModified = DateTime.Now;
                OnPropertyChanged(nameof(Content));
                OnPropertyChanged(nameof(CanUndo));
                OnPropertyChanged(nameof(CanRedo));
                OnPropertyChanged(nameof(IsSaved));
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                _undoStack.Push(_content);
                _content = _redoStack.Pop();
                LastModified = DateTime.Now;
                OnPropertyChanged(nameof(Content));
                OnPropertyChanged(nameof(CanUndo));
                OnPropertyChanged(nameof(CanRedo));
                OnPropertyChanged(nameof(IsSaved));
            }
        }

        public void UpdateContent(string newContent)
        {
            Content = newContent;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
