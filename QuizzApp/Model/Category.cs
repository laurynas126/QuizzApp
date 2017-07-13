using QuizzApp.DataManagement;
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

        private List<Question> _questions;
        public List<Question> Questions {
            get
            {
                if (_questions == null) _questions = QuestionTable.GetQuestionsByCategory(Id);
                return _questions;
            }
            set
            {
                _questions = value;
            }
        }

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
