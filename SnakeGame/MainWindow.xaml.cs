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

        public double XLines = 22;
        public double YLines = 22;

        public DispatcherTimer dTimer = new DispatcherTimer();

        public int Score = 0;
        public int DifficultyScore = 0;
        public int AppleCount = 0;
        public string AppleColor = "";

        public int TimerMsec = 275;

        List<UIElement> bodyParts = new List<UIElement>();
        List<double> bodyPartsYX = new List<double>();

        public bool AppleIsOnSnake = false;

        public MainWindow()
        {
            InitializeComponent();
            
            double XSpace = GameBoard.Width / XLines;
            double YSpace = GameBoard.Height / YLines;

            DrawBoard(XLines, YLines, XSpace, YSpace);
            InitSnake();
            AppleChange(XSpace);
            dTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 0, 0, 0, TimerMsec);
            dTimer.Start();            
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
            double XSpace = GameBoard.Width / XLines;
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
            System.Threading.Thread.Sleep(5);
            SnakeBodyChanges();
            System.Threading.Thread.Sleep(5);
            CheckIfAppleHit();
            System.Threading.Thread.Sleep(5);

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

        private void AppleChange(double XSpace)
        {
            System.Threading.Thread.Sleep(5);
            Random rnd = new Random();
            int appleY = rnd.Next(1, (int)XLines - 1);
            int appleX = rnd.Next(1, (int)YLines - 1);
            double snakeCurrTop = Canvas.GetTop(snake);
            double snakeCurrLeft = Canvas.GetLeft(snake);

            if (AppleIsOnSnake == false)
            {
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
            }

            CheckIfAppleOnSnake(appleX, appleY, XSpace);           
        }

        private void CheckIfAppleOnSnake(int appleX, int appleY, double XSpace)
        {
            double snakeCurrTop = Canvas.GetTop(snake);
            double snakeCurrLeft = Canvas.GetLeft(snake);
            int bodyPartIndex = 0;

            Canvas.SetTop(apple, XSpace * appleY + 5);
            Canvas.SetLeft(apple, XSpace * appleX + 5);

            System.Threading.Thread.Sleep(5);
            for (int i = 0; i < bodyPartsYX.Count; i += 2)
            {
                if ((appleY * XSpace == bodyPartsYX[i]) && (appleX * XSpace == bodyPartsYX[i + 1]) && (bodyParts[bodyPartIndex].Visibility == Visibility.Visible))
                {
                    AppleIsOnSnake = true;
                    AppleChange(XSpace);
                }
                else
                {
                    AppleIsOnSnake = false;
                }
                bodyPartIndex++;
            }

        }

        public bool CheckIfGameOver(double GameBoardW, double GameBoardH, bool GameOver)
        {
            double snakeCurrTop = Canvas.GetTop(snake);
            double snakeCurrLeft = Canvas.GetLeft(snake);

            foreach (UIElement body in bodyParts)
            {
                double bodyPartTop = Canvas.GetTop(body);
                double bodyPartLeft = Canvas.GetLeft(body);

                if ((snakeCurrTop == bodyPartTop) && (snakeCurrLeft == bodyPartLeft) && (body.Visibility == Visibility.Visible))
                {
                    GameOver = true;
                }
            }

            if (GameOver != true)
            {
                if (snakeCurrLeft < 0 || snakeCurrLeft >= GameBoardW ||
                snakeCurrTop < 0 || snakeCurrTop >= GameBoardH)
                {
                    GameOver = true;
                }
                else
                {
                    GameOver = false;
                }
            }

            return GameOver;
        }

        private void SnakeBodyChanges()
        {
            double XSpace = GameBoard.Width / XLines;
            double snakeCurrTop = Canvas.GetTop(snake);
            double snakeCurrLeft = Canvas.GetLeft(snake);

            Rectangle bodyPart = new Rectangle();
            bodyPart.Width = XSpace;
            bodyPart.Height = XSpace;
            bodyPart.Fill = Brushes.DodgerBlue;
            bodyPart.Stroke = Brushes.DeepSkyBlue;
            bodyPart.StrokeThickness = 5;
            bodyPart.RadiusX = 10;
            bodyPart.RadiusY = 10;
            Panel.SetZIndex(bodyPart, 9);
            Canvas.SetTop(bodyPart, snakeCurrTop);
            Canvas.SetLeft(bodyPart, snakeCurrLeft);
            GameBoard.Children.Add(bodyPart);

            bodyParts.Add(bodyPart);
            bodyPartsYX.Add(snakeCurrTop);
            bodyPartsYX.Add(snakeCurrLeft);

            foreach (UIElement body in bodyParts)
            {
                body.Visibility = Visibility.Collapsed;
            }

            int lastBodyPart = bodyParts.Count - 1;
            for (int i = 0; i <= AppleCount; i++)
            {
                bodyParts[lastBodyPart].Visibility = Visibility.Visible;
                lastBodyPart--;
            }
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

                System.Threading.Thread.Sleep(5);
                AppleChange(XSpace);
            }
        }

    }
}
