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
            if (this.DataContext is QuizzViewModel context)
            {
                if (!context.IsFinished)
                {
                    var result = MessageBox.Show("Are you sure you want to quit?", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result.HasFlag(MessageBoxResult.No)) return;
                }
            }
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
                //AnimateColorTransition(button, Colors.LimeGreen);
                button.Background = new SolidColorBrush(Colors.LimeGreen);
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
            doubleAnim.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            doubleAnim.AutoReverse = false;
            doubleAnim.AccelerationRatio = 0.6;
            doubleAnim.DecelerationRatio = 0.4;

            var staticAnim = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromSeconds(0.5)));
            staticAnim.BeginTime = TimeSpan.FromSeconds(2.5);

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
                    AnimateColorTransition(FindVisualChild<Button>(uiElement), Colors.LimeGreen);
                }
            }
        }
        private void AnimateColorTransition(Control element, Color toColor)
        {
            var value = element.Background as SolidColorBrush;
            SolidColorBrush brush = new SolidColorBrush(Colors.Transparent);
            element.Background = brush;
            ColorAnimation animation = new ColorAnimation(value.Color, toColor, TimeSpan.FromSeconds(0.3));
            animation.AccelerationRatio = 0.8;
            animation.DecelerationRatio = 0.2;
            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(animation);

            brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
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
                if (context.IsFinished)
                {
                    returnButton.Visibility = Visibility.Visible;
                    textAnswer.Visibility = Visibility.Hidden;
                }
                else if (context.IsTextQuestion)
                {
                    ResetInputBox(textQuestionInputBox);
                    SubmitInputButton.IsEnabled = true;
                }
            }
            itemControl.IsEnabled = true;
        }

        private static void MakeHidden(UIElement sender)
        {
            if(sender != null)
                sender.Visibility = Visibility.Hidden;
        }

        private void SubmitInputButton_Click(object sender, RoutedEventArgs e)
        {
            bool IsCorrect = false;
            var context = DataContext as QuizzViewModel;

            if (textQuestionInputBox.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Enter your answer into the text field");
                return;
            }
            if (sender is Button b)
            {
                b.IsEnabled = false;
            }
            IsCorrect = context.SubmitTextAnswer(textQuestionInputBox.Text);
            if (IsCorrect)
            {
                FadeObject(CorrectText);
                AnimateColorTransition(textQuestionInputBox, Colors.LimeGreen);
            }
            else
            {
                AnimateColorTransition(textQuestionInputBox, Colors.IndianRed);
                textQuestionInputBox.Foreground = new SolidColorBrush(Colors.White);
                textAnswer.Visibility = Visibility.Visible;
                textAnswer.Text = context.CorrectAnswer;
            }
            continueButton.Visibility = Visibility.Visible;
        }

        private void ResetInputBox(TextBox box)
        {
            textAnswer.Visibility = Visibility.Hidden;
            box.Text = string.Empty;
            box.Background = new SolidColorBrush(Colors.White);
            box.Foreground = new SolidColorBrush(Colors.Black);
        }
    }
}
