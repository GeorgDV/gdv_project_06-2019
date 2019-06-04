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
        string moveDir = "Right";

        public MainWindow()
        {
            InitializeComponent();
            double XLines = 22;
            double YLines = 22;
            double XSpace = GameBoard.Width / XLines;
            double YSpace = GameBoard.Height / YLines;
            DrawBoard(XLines, YLines, XSpace, YSpace);
            InitSnake();
            GenerateApple(XLines, YLines, XSpace);
            DispatcherTimer dTimer = new DispatcherTimer();
            dTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dTimer.Start();

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            double currentTop = Canvas.GetTop(snake);
            double currentLeft = Canvas.GetLeft(snake);
            if (e.Key == Key.W || e.Key == Key.Up)
            {
                moveDir = "Up";
            }
            else if (e.Key == Key.S || e.Key == Key.Down)
            {
                moveDir = "Down";
            }
            else if (e.Key == Key.A || e.Key == Key.Left)
            {
                moveDir = "Left";
            }
            else if (e.Key == Key.D || e.Key == Key.Right)
            {
                moveDir = "Right";
            }
        }

        public void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            double currentTop = Canvas.GetTop(snake);
            double currentLeft = Canvas.GetLeft(snake);
            if (moveDir == "Up")
            {
                Canvas.SetTop(snake, currentTop - 30);
            }
            else if (moveDir == "Down")
            {
                Canvas.SetTop(snake, currentTop + 30);
            }
            else if (moveDir == "Right")
            {
                Canvas.SetLeft(snake, currentLeft + 30);
            }
            else if (moveDir == "Left")
            {
                Canvas.SetLeft(snake, currentLeft - 30);
            }
        }

        private void DrawBoard(double XLines, double YLines, double XSpace, double YSpace)
        {
            //GAMEGRID
            double startTop;
            double startLeft;

            double X1 = 0;
            double Y1 = 0;
            double X2 = 0;
            double Y2 = GameBoard.Height;

            startTop = -60;
            for (int i = 0; i < XLines / 2; i++)
            {
                startTop += XSpace * 2;
                startLeft = 0;
                for (int j = 0; j < YLines / 2; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Height = YSpace;
                    r.Width = XSpace;
                    r.Fill = Brushes.LightGreen;
                    Canvas.SetTop(r, startTop);
                    Canvas.SetLeft(r, startLeft);
                    startLeft += XSpace * 2;
                    GameBoard.Children.Add(r);
                }
            }

            startTop = -30;
            for (int i = 0; i < YLines / 2; i++)
            {
                startTop += XSpace * 2;
                startLeft = 30;
                for (int j = 0; j < YLines / 2; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Height = YSpace;
                    r.Width = XSpace;
                    r.Fill = Brushes.LightGreen;
                    Canvas.SetTop(r, startTop);
                    Canvas.SetLeft(r, startLeft);
                    startLeft += XSpace * 2;
                    GameBoard.Children.Add(r);
                }
            }

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
        }

        private void InitSnake()
        {
            snake.Height = 30;
            snake.Width = 30;
            Canvas.SetTop(snake, GameBoard.Height / 2);
            Canvas.SetLeft(snake, GameBoard.Width / 2);
        }

        private void GenerateApple(double XLines, double YLines, double XSpace)
        {
            Random rnd = new Random();
            int appleY = rnd.Next((int)XLines);
            int appleX = rnd.Next((int)YLines);
            double snakeCurrTop = Canvas.GetTop(snake);
            double snakeCurrLeft = Canvas.GetLeft(snake);
            do
            {
                Canvas.SetTop(apple, XSpace * appleY + 5);
                Canvas.SetLeft(apple, XSpace * appleX + 5);
            }
            while ((XSpace * appleY) == snakeCurrTop && (XSpace * appleX) == snakeCurrLeft);
        }


    }
}
