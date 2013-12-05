using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodBooks.Models
{
    public class MyNotes
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string DateCreated { get; set; }

        public MyNotes()
        {
            this.Title = string.Empty;
            this.Content = string.Empty;
            this.DateCreated = DateTime.Now.ToString("dd/mm/yyyy:hh/mm/ss");
        }
    }
}
