using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzApp.Model
{
    public class Answer
    {
        private string _text;
        public string Text {
            get { return _text; }
            set { _text = value.Trim(); }
        }

        public bool Correct { get; }

        public Answer(string text, bool correct)
        {
            Text = text;
            Correct = correct;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
