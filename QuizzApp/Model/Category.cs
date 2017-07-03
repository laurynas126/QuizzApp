using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzApp.Model
{
    public class Category
    {
        public long Id { get; set; } = -1;

        public string Title { get; set; }

        public Category() { }

        public Category(string title)
        {
            Title = title;
        }

        public Category(int id, string title) : this(title)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
