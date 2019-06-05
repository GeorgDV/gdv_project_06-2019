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
        bool GameOver = false;

        public DispatcherTimer dTimer = new DispatcherTimer();

        public double XLines = 22;
        public double YLines = 22;

        public int Score = 0;
        public int DifficultyScore = 0;
        public int AppleCount = 0;
        public string AppleColor = "";

        public int TimerMsec = 300;

        List<UIElement> snakeBody = new List<UIElement>();
        List<double> bodyPartsCoordinates = new List<double>();

        public MainWindow()
        {
            InitializeComponent();
            
            double XSpace = GameBoard.Width / XLines;
            double YSpace = GameBoard.Height / YLines;

            DrawBoard(XLines, YLines, XSpace, YSpace);
            InitSnake();
            dTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, TimerMsec);
            dTimer.Start();
            MoveApple(XSpace);
            
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            double currentTop = Canvas.GetTop(snake);
            double currentLeft = Canvas.GetLeft(snake);
            if ((e.Key == Key.W || e.Key == Key.Up) &&  moveDir != "Down")
            {
                moveDir = "Up";
            }
            else if ((e.Key == Key.S || e.Key == Key.Down) && moveDir != "Up")
            {
                moveDir = "Down";
            }
            else if ((e.Key == Key.A || e.Key == Key.Left) && moveDir != "Right")
            {
                moveDir = "Left";
            }
            else if ((e.Key == Key.D || e.Key == Key.Right) && moveDir != "Left")
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

            GameOver = CheckIfGameOver(GameBoard.Width, GameBoard.Height, GameOver);
            if (GameOver == true)
            {
                dTimer.Stop();
                MessageBox.Show("Your score: " + Score, "Game over.", MessageBoxButton.OK);
                Application.Current.Shutdown();
            }
            CheckIfAppleHit();

            if (DifficultyScore >= 100)
            {
                DifficultyScore -= 100;
                if ((TimerMsec <= 50) == false)
                {
                    TimerMsec -= 25;
                }
                dTimer.Interval = new TimeSpan(0, 0, 0, 0, TimerMsec);

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

        private void MoveApple(double XSpace)
        {
            Random rnd = new Random();
            int appleY = rnd.Next(1, (int)XLines - 1);
            int appleX = rnd.Next(1, (int)YLines - 1);
            double snakeCurrTop = Canvas.GetTop(snake);
            double snakeCurrLeft = Canvas.GetLeft(snake);

            int Color = rnd.Next(12);
            if (Color <= 3) //GREEN
            {
                apple.Fill = Brushes.Green;
                apple.Stroke = Brushes.ForestGreen;
                AppleColor = "green";
            }
            else if (Color == 4) //YELLOW
            {
                apple.Fill = Brushes.Yellow;
                apple.Stroke = Brushes.Gold;
                AppleColor = "yellow";
            }
            else //RED
            {
                apple.Fill = Brushes.Red;
                apple.Stroke = Brushes.DarkRed;
                AppleColor = "red";
            }

            do
            {
                Canvas.SetTop(apple, XSpace * appleY + 5);
                Canvas.SetLeft(apple, XSpace * appleX + 5);
            }
            while ((XSpace * appleY) == snakeCurrTop && (XSpace * appleX) == snakeCurrLeft);
        }

        public bool CheckIfGameOver(double GameBoardW, double GameBoardH, bool GameOver)
        {
            double snakeCurrTop = Canvas.GetTop(snake);
            double snakeCurrLeft = Canvas.GetLeft(snake);
            if (snakeCurrLeft < 0 || snakeCurrLeft >= GameBoardW ||
                snakeCurrTop < 0 || snakeCurrTop >= GameBoardH)
            {
                GameOver = true;
            }
            else
                GameOver = false;
            return GameOver;
        }

        private void CheckIfAppleHit()
        {
            double XSpace = GameBoard.Width / XLines;
            double snakeCurrTop = Canvas.GetTop(snake);
            double snakeCurrLeft = Canvas.GetLeft(snake);
            double appleCurrTop = Canvas.GetTop(apple);
            double appleCurrLeft = Canvas.GetLeft(apple);
            if (snakeCurrTop == (appleCurrTop - 5) && snakeCurrLeft == (appleCurrLeft - 5))
            {
                if (AppleColor == "red")
                {
                    Score += 10;
                    DifficultyScore += 10;
                }
                else if (AppleColor == "green")
                {
                    Score += 20;
                    DifficultyScore += 20;
                }
                else if (AppleColor == "yellow")
                {
                    Score += 50;
                    DifficultyScore += 50;
                }

                AppleCount++;
                ScoreNumber.Content = Score;
                ApplesNumber.Content = AppleCount;

                ////TODO (SNAKE LENGTH CHANGING)
                //Rectangle bodyPart = new Rectangle();
                //bodyPart.Fill = Brushes.DodgerBlue;
                //bodyPart.Stroke = Brushes.DeepSkyBlue;
                //bodyPart.StrokeThickness = 5;
                //bodyPart.Width = 30;
                //bodyPart.Height = 30;
                //Canvas.SetZIndex(bodyPart, 10);
                //double bodyPartTop = 0;
                //double bodyPartLeft = 0;
                //if (snakeBody.Count > 0)
                //{
                //    bodyPartTop = Canvas.GetTop(snakeBody[snakeBody.Count - 1]);
                //    bodyPartLeft = Canvas.GetLeft(snakeBody[snakeBody.Count - 1]);
                //}
                //else
                //{
                //    bodyPartTop = snakeCurrTop;
                //    bodyPartLeft = snakeCurrLeft;
                //}
                
                //snakeBody.Add(bodyPart);

                //for (int i = 0; i < snakeBody.Count; i++)
                //{
                //    if (moveDir == "Up")
                //    {
                //        Canvas.SetTop(snakeBody[i], bodyPartTop + 30);
                //    }
                //    else if (moveDir == "Down")
                //    {
                //        Canvas.SetTop(snakeBody[i], bodyPartTop - 30);
                //    }
                //    else if (moveDir == "Left")
                //    {
                //        Canvas.SetLeft(snakeBody[i], bodyPartLeft + 30);
                //    }
                //    else if (moveDir == "Right")
                //    {
                //        Canvas.SetLeft(snakeBody[i], bodyPartLeft - 30);
                //    }
                //    GameBoard.Children.Add(bodyPart);
                //}

                MoveApple(XSpace);
            }
        }

    }
}
