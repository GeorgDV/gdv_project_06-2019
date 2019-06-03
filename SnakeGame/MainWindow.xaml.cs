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

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double currentLeft = Canvas.GetLeft(rectangle1);
                Canvas.SetLeft(rectangle1, currentLeft + 20);
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                Canvas.SetLeft(rectangle1, 20);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            double currentTop = Canvas.GetTop(rectangle1);
            double currentLeft = Canvas.GetLeft(rectangle1);
            if (e.Key == Key.W)
            {
                Canvas.SetTop(rectangle1, currentTop - 20);
            }
            else if (e.Key == Key.S)
            {
                Canvas.SetTop(rectangle1, currentTop + 20);
            }
            else if (e.Key == Key.A)
            {
                Canvas.SetLeft(rectangle1, currentLeft - 20);
            }
            else if (e.Key == Key.D)
            {
                Canvas.SetLeft(rectangle1, currentLeft + 20);
            }
        }
    }
}
