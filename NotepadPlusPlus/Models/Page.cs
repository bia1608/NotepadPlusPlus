using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotepadPlusPlus.Models
{
    internal class Page
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime LastModified { get; set; }

        public Page()
        {
            Title = "Untitled";
            Content = string.Empty;
            LastModified = DateTime.Now;
        }

        public Page(string title, string content)
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
