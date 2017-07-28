using QuizzApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizzApp.View
{
    /// <summary>
    /// Interaction logic for QuizzEdit.xaml
    /// </summary>
    public partial class QuizzEdit : Page
    {
        public QuizzEdit()
        {
            InitializeComponent();
            var father = this.Parent;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void CategoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditViewModel vm)
            {
                vm.DeleteCommand();
            }
        }

        private void NewCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditViewModel vm)
            {
                vm.NewCategory();
            }
        }

        private void SaveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditViewModel vm)
            {
                vm.SaveCategory();
            }
        }

        private void NewQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditViewModel vm)
            {
                vm.NewQuestion();
            }
        }

        private void SaveQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditViewModel vm)
            {
                vm.SaveQuestion();
            }
        }

        private void DeleteQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditViewModel vm)
            {
                vm.DeleteQuestion();
            }
            SelectedQuestionList.Items.Refresh();
        }

        private void AddToCategory_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditViewModel vm)
            {
                vm.AddToCategory();
            }
            SelectedQuestionList.Items.Refresh();
        }

        private void RemoveFromCategory_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditViewModel vm)
            {
                vm.RemoveFromCategory();
            }
            SelectedQuestionList.Items.Refresh();
        }
    }
}
