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
using System.Windows.Media.Animation;

namespace QuizzApp.View
{
    /// <summary>
    /// Interaction logic for Quizz.xaml
    /// </summary>
    public partial class QuizzView : Page
    {
        private Storyboard myStoryboard;

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
                FadeObject(CorrectText);
            }
            else
            {
                IncorrectButton(button);
            }
            continueButton.Visibility = Visibility.Visible;
            itemControl.IsEnabled = false;
        }

        private void FadeObject(TextBlock element)
        {
            DoubleAnimation doubleAnim = new DoubleAnimation();
            doubleAnim.From = 0.0;
            doubleAnim.To = 1.0;
            doubleAnim.Duration = new Duration(TimeSpan.FromSeconds(1));
            doubleAnim.AutoReverse = false;
            doubleAnim.AccelerationRatio = 0.6;
            doubleAnim.DecelerationRatio = 0.4;

            var staticAnim = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(1)));
            staticAnim.BeginTime = TimeSpan.FromSeconds(3);

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(doubleAnim);
            myStoryboard.Children.Add(staticAnim);

            Storyboard.SetTarget(doubleAnim, element);
            Storyboard.SetTarget(staticAnim, element);
            Storyboard.SetTargetProperty(doubleAnim, new PropertyPath(TextBlock.OpacityProperty));
            Storyboard.SetTargetProperty(staticAnim, new PropertyPath(TextBlock.OpacityProperty));
            myStoryboard.Begin();

        }
        private void IncorrectButton(Button button)
        {
            button.Background = new SolidColorBrush(Colors.IndianRed);
            button.Foreground = new SolidColorBrush(Colors.White);
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
            var value = button.Background as SolidColorBrush;
            SolidColorBrush brush = new SolidColorBrush(Colors.Transparent);
            button.Background = brush;
            ColorAnimation animation = new ColorAnimation(value.Color, Colors.LimeGreen, TimeSpan.FromSeconds(0.8));
            animation.AccelerationRatio = 0.8;
            animation.DecelerationRatio = 0.2;
            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(animation);

            brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            //myStoryboard.Begin();

            //button.Background = new SolidColorBrush(Colors.LimeGreen);
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
