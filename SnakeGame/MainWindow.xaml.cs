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
using System.Windows.Threading;
using System.Timers;

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
            //GAMEGRID
            double XLines = 22;
            double YLines = 22;
            double XSpace = GameBoard.Width / XLines;
            double YSpace = GameBoard.Height / YLines;
            double X1 = 0;
            double Y1 = 0;
            double X2 = 0;
            double Y2 = GameBoard.Height;

            for (int i = 0; i < XLines + 1; i++)
            {
                Line line = new Line();
                line.Visibility = System.Windows.Visibility.Visible;
                line.StrokeThickness = 1;
                line.Stroke = System.Windows.Media.Brushes.ForestGreen;
                line.X1 = X1;
                line.Y1 = Y1;
                line.X2 = X2;
                line.Y2 = Y2;
                GameBoard.Children.Add(line);
                X1 += XSpace;
                X2 += XSpace;
            }
            X1 = 0;
            X2 = GameBoard.Width;
            Y2 = 0;
            for (int i = 0; i < YLines + 1; i++)
            {
                Line line = new Line();
                line.Visibility = System.Windows.Visibility.Visible;
                line.StrokeThickness = 1;
                line.Stroke = System.Windows.Media.Brushes.ForestGreen;
                line.X1 = X1;
                line.Y1 = Y1;
                line.X2 = X2;
                line.Y2 = Y2;
                GameBoard.Children.Add(line);
                Y1 += YSpace;
                Y2 += YSpace;
            }
            //GAMEGRID/

            //DispatcherTimer dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            //dispatcherTimer.Start();
        }

        //private void dispatcherTimer_Tick(object sender, EventArgs e)
        //{
        //    double currentTop = Canvas.GetTop(rectangle1);
        //    double currentLeft = Canvas.GetLeft(rectangle1);
        //    Canvas.SetLeft(rectangle1, currentLeft + 25);
        //}

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double currentLeft = Canvas.GetLeft(rectangle1);
                Canvas.SetLeft(rectangle1, currentLeft + 30);
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Canvas.SetLeft(rectangle1, 30);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            double currentTop = Canvas.GetTop(rectangle1);
            double currentLeft = Canvas.GetLeft(rectangle1);
            if (e.Key == Key.W || e.Key == Key.Up)
            {
                Canvas.SetTop(rectangle1, currentTop - 30);
            }
            else if (e.Key == Key.S || e.Key == Key.Down)
            {
                Canvas.SetTop(rectangle1, currentTop + 30);
            }
            else if (e.Key == Key.A || e.Key == Key.Left)
            {
                Canvas.SetLeft(rectangle1, currentLeft - 30);
            }
            else if (e.Key == Key.D || e.Key == Key.Right)
            {
                Canvas.SetLeft(rectangle1, currentLeft + 30);
            }
        }
    }
}
