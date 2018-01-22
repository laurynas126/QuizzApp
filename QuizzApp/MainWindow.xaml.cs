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

namespace QuizzApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string BackgroundColor { get; private set; } = "#FF56A3FF";

        public MainWindow()
        {
            var colorObject = ColorConverter.ConvertFromString(Properties.Settings.Default.Background);
            if (colorObject != null)
            {
                BackgroundColor = Properties.Settings.Default.Background;
            }
            InitializeComponent();
        }
    }
}
