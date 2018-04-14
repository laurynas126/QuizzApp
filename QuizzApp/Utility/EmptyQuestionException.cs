using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzApp.Utility
{
    public class EmptyQuestionException : ArgumentException
    {
        public EmptyQuestionException() : base()
        {
            
        }
        public EmptyQuestionException(string message) : base(message)
        {
        }
    }
}
