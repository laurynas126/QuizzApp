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

        public bool CanDelete { get => SelectedCategory != null; }

        private Question _question;
        public Question SelectedQuestion {
            get => _question;
            set
            {
                _question = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedQuestion"));
            }
        }

        private Category _category;
        public Category SelectedCategory {
            get => _category;
            set
            {
                _category = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CanDelete"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedCategory"));
            }

        }

        public EditViewModel() { }

        public void DeleteCommand()
        {
            var result = MessageBox.Show("This action cannot be undone. Are you sure?", "Delete category", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
    }
}
