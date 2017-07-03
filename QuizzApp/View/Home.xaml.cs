using QuizzApp.Model;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int number = (int)numberBox.SelectedItem;
            Category selectedCategory = categorySelect.SelectedItem as Category; 
            QuizzViewModel model = new QuizzViewModel(selectedCategory, number);

            QuizzView quizz = new QuizzView(model);
            this.NavigationService.Navigate(quizz);
        }
    }
}
