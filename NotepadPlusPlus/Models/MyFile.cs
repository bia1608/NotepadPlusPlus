using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotepadPlusPlus.Models
{
    internal class MyFile
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime LastModified { get; set; }

        public MyFile()
        {
            Title = "Untitled";
            Content = string.Empty;
            LastModified = DateTime.Now;
        }

        public MyFile(string title, string content)
        {
            Title = title;
            Content = content;
            LastModified = DateTime.Now;
        }

        public void UpdateContent(string newContent)
        {
            Content = newContent;
            LastModified = DateTime.Now;
        }
    }
}
