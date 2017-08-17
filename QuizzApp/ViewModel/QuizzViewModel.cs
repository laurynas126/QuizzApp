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
using QuizzApp.Utility;
using System.Windows;

namespace QuizzApp.ViewModel
{
    public class QuizzViewModel : INotifyPropertyChanged
    {
        private Question _currentQuestion = null;
        private QuizzStats statistic;
        private QuestionTable _loader;

        public string AppTitle => StringResources.Title;
        
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

        public string Image
        {
            get
            {
                if (_currentQuestion != null && _currentQuestion.ImageName != null && _currentQuestion.ImageName != string.Empty)
                    return StringResources.ResourceFolder + "\\" + _currentQuestion.ImageName;
                return StringResources.DefaultImage;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Image"));
        }

    }
}
