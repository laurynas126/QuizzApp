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
using QuizzApp.ViewModel;
using QuizzApp.Model;

namespace QuizzApp.View
{
    /// <summary>
    /// Interaction logic for Quizz.xaml
    /// </summary>
    public partial class QuizzView : Page
    {
        public QuizzView(object dataContext)
        {
            InitializeComponent();
            this.DataContext = dataContext;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to quit?", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result.HasFlag(MessageBoxResult.No)) return;
            this.NavigationService.GoBack();
        }

        private void AnswerButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            bool isCorrect = false;     
            if (this.DataContext is QuizzViewModel context)
            {
                isCorrect = context.ChosenAnswer((Answer)button.Content);
            }
            if (isCorrect)
            {
                CorrectButton(button);
            }
            else
            {
                IncorrectButton(button);
            }
            continueButton.Visibility = Visibility.Visible;
            itemControl.IsEnabled = false;
        }
        private void IncorrectButton(Button button)
        {
            button.Background = new SolidColorBrush(Colors.IndianRed);
            foreach (var item in itemControl.Items)
            {
                var buttonAnswer = (Answer)item;
                if (buttonAnswer.Correct)
                {
                    UIElement uiElement = (UIElement)itemControl.ItemContainerGenerator.ContainerFromItem(item);
                    CorrectButton(FindVisualChild<Button>(uiElement));
                }
            }
        }
        private void CorrectButton(Button button)
        {
            button.Background = new SolidColorBrush(Colors.LimeGreen);
        }

        private static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }

                T childItem = FindVisualChild<T>(child);
                if (childItem != null) return childItem;
            }
            return null;
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            MakeHidden(sender as UIElement);
            if (this.DataContext is QuizzViewModel context)
            {
                context.NextQuestion();
            }
            itemControl.IsEnabled = true;
        }

        private static void MakeHidden(UIElement sender)
        {
            if(sender != null)
                sender.Visibility = Visibility.Hidden;
        }
    }
}
