using Microsoft.Win32;
using QuizzApp.DataManagement;
using QuizzApp.Model;
using QuizzApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuizzApp.ViewModel
{
    public class EditViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Category> _categories = new ObservableCollection<Category>(CategoryTable.GetAllCategories());

        public ObservableCollection<Category> CategoryList { get => _categories; }

        private readonly List<Question> _questions = QuestionTable.GetQuestionsByCategory(0);

        public ObservableCollection<Question> AllQuestionList
        {
            get
            {
                ObservableCollection<Question> collection;
                if (IsSelectedCategory)
                    collection = new ObservableCollection<Question>(_questions.Where(x => !SelectedCategory.Questions.Contains(x)));
                else
                    collection = new ObservableCollection<Question>(_questions);
                return collection;
            }
        }

        //Can add question to category?
        public bool CanAdd { get => SelectedCategory != null && SelectedQuestion != null && !SelectedCategory.Questions.Contains(SelectedQuestion); }
        public bool CanRemove { get => SelectedCategory != null && SelectedQuestion != null && SelectedCategory.Questions.Contains(SelectedQuestion); }

        private Question _question;
        public bool IsSelectedQuestion { get => SelectedQuestion != null; }
        public Question SelectedQuestion {
            get => _question;
            set
            {
                _question = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedQuestion"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelectedQuestion"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CanAdd"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CanRemove"));
            }
        }

        private Category _category;
        public bool IsSelectedCategory { get => SelectedCategory != null; }
        public Category SelectedCategory {
            get => _category;
            set
            {
                _category = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelectedCategory"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedCategory"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllQuestionList"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CanAdd"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CanRemove"));
            }
        }

        public EditViewModel() { }

        public void DeleteCommand()
        {
            var result = MessageBox.Show("This action cannot be undone. Are you sure?", $"Delete \"{SelectedCategory.Title}\" category", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                CategoryQuestionTable.DeleteQuestionsFromCategory(SelectedCategory);
                CategoryTable.DeleteCategory(SelectedCategory);
                CategoryList.Remove(SelectedCategory);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CategoryList"));
            }
        }

        public void NewCategory()
        {
            SelectedCategory = new Category("Untitled");
            CategoryList.Add(SelectedCategory);
        }

        public void SaveCategory()
        {
            try
            {
                CategoryTable.SaveCategory(SelectedCategory);
            }
            catch (SQLiteException ex)
            {
                if (ex.ResultCode == SQLiteErrorCode.Constraint)
                {
                    MessageBox.Show(StringResources.DuplicateCategoryError);
                }
            }
        }

        public void NewQuestion()
        {
            SelectedQuestion = new Question("", new string[] { "", "", "", "" });
        }

        public void NewTextQuestion()
        {
            SelectedQuestion = new Question("", new string[] { "" });
            SelectedQuestion.IsFreeText = true;
        }

        public void SaveQuestion()
        {
            try
            {
                SelectedQuestion.ImageName = FileHandler.SaveFileToResourceFolder(SelectedQuestion.ImageName);
                QuestionTable.SaveQuestion(SelectedQuestion);
                if (!_questions.Contains(SelectedQuestion))
                    _questions.Add(SelectedQuestion);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllQuestionList"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedQuestion"));
            }
            catch (SQLiteException ex)
            {
                if (ex.ResultCode == SQLiteErrorCode.Constraint)
                {
                    MessageBox.Show(StringResources.DuplicateQuestionError);
                }
                else
                {
                    MessageBox.Show("Unknown SQL error: " + ex.ResultCode.ToString());
                }
            }
            catch (EmptyQuestionException ex)
            {
                MessageBox.Show(StringResources.EmptyQuestionError);
            }
        }

        public void DeleteQuestion()
        {
            QuestionTable.DeleteQuestion(SelectedQuestion);
            CategoryQuestionTable.DeleteQuestion(SelectedQuestion);
            _questions.Remove(SelectedQuestion);
            if(IsSelectedCategory)
            {
                SelectedCategory.Questions.Remove(SelectedQuestion);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedCategory"));
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllQuestionList"));
        }

        public void AddToCategory()
        {
            if (!CanAdd) return;
            if (CategoryQuestionTable.AddCategoryQuestion(SelectedCategory, SelectedQuestion) <= 0)
            {
                MessageBox.Show("Could not add question to category");
                return;
            }
            SelectedCategory.Questions.Add(SelectedQuestion);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllQuestionList"));
        }

        public void RemoveFromCategory()
        {
            if (!CanRemove) return;
            CategoryQuestionTable.DeleteCategoryQuestion(SelectedCategory, SelectedQuestion);
            SelectedCategory.Questions.Remove(SelectedQuestion);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllQuestionList"));
        }

        public void SelectImage()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select Image",
                Filter = "Image Files (*.jpeg;*jpg;*png)|*.jpeg;*.jpg;*.png|JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png"
            };
            ofd.ShowDialog();
            SelectedQuestion.ImageName = ofd.FileName;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedQuestion"));
        }
    }
}
