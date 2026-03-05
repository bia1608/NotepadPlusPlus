using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NotepadPlusPlus.Models
{
    internal class MyFile : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsSaved { get; set; }

        public MyFile()
        {
            Title = "Untitled";
            Content = string.Empty;
            LastModified = DateTime.Now;
            IsSaved = false;
        }

        public MyFile(string title, string content)
        {
            Title = title;
            Content = content;
            LastModified = DateTime.Now;
            IsSaved = false;
        }

        public void UpdateContent(string newContent)
        {
            Content = newContent;
            LastModified = DateTime.Now;
        }

        internal void Undo()
        {

        }

        internal void Redo()
        {
            throw new NotImplementedException();
        }

        internal void MarkAsSaved()
        {
            IsSaved = true;
            OnPropertyChanged(nameof(IsSaved));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
