using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using QuizzApp.Model;
using System.Windows.Input;
using System.ComponentModel;
using QuizzApp.DataManagement;


namespace QuizzApp.ViewModel
{
    public class QuizzViewModel : INotifyPropertyChanged
    {
        private Question _currentQuestion = null;
        private QuizzStats statistic;
        private QuestionTable _loader;

        public string AppTitle => Properties.Resources.Title;
        
        public Category SelectedCategory { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public QuizzViewModel(Category category, int questionNumber = 10)
        {
            SelectedCategory = category;
            _loader = new QuestionTable(questionNumber);
            _loader.LoadMultiQuestionsByCategory(SelectedCategory.Id);
            statistic = new QuizzStats();
            this.NextQuestion();
        }

        public string Question
        {
            get {
                if (_currentQuestion != null) return _currentQuestion.QuestionText;
                return "Finished";
            }
        }

        public string QuestionNumber => "Question " + statistic.QuestionNumber.ToString();

        public string CorrectAnswered => "Correct: " + statistic.CorrectAnswered.ToString();

        public ObservableCollection<Answer> Answers
        {
            get {
                if (_currentQuestion != null)
                {
                    var result = _currentQuestion.Answers.Where(x => !string.Empty.Equals(x.Text));
                    return new ObservableCollection<Answer>(result);
                }
                return null;
            }
        }

        public bool ChosenAnswer(Answer answer)
        {
            if (answer.Correct)
            {
                statistic.CorrectAnswered++;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CorrectAnswered"));
                return true;
            }
            return false;
        }

        public void NextQuestion()
        {
            _currentQuestion = _loader.GetNextQuestion();
            if (_currentQuestion != null)
            {
                statistic.QuestionNumber++;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Question"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Answers"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("QuestionNumber"));
        }

    }
}
