using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzApp.Model
{
    public class Question
    {
        public long Id { get; set; } = -1;
        public string QuestionText { get; set; }
        public List<Answer> Answers { get; }
        public string ImageName { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }

        public Question(string question, List<Answer> answers) : this()
        {
            QuestionText = question;
            Answers.AddRange(answers);
        }

        public Question(string question, string[] answers) : this()
        {
            QuestionText = question;
            if (answers.Length < 2)
            {
                return;
            }
            Answers.Add(new Answer(answers[0], true));
            for(int i = 1; i < answers.Length; i++)
            {
                Answers.Add(new Answer(answers[i], false));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Question q)
            {
                return q.Id == this.Id;
            }
            return false;
        }
    }
}
