using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace wpfTetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TetrisField? field = null;
        private Tetromino? current = null;
        private Tetromino? next = null;
        private DispatcherTimer? timer = null;
        private int score = 0;
        private bool isPaused = false;
        private readonly int cellSize = 30;

        public MainWindow()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            field = new TetrisField();
            score = 0;
            ScoreText.Text = "0";
            next = GetRandomTetromino();
            SpawnTetromino();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(400);
            timer.Tick += GameTick;
            Draw();
        }

        private Tetromino GetRandomTetromino()
        {
            var values = Enum.GetValues(typeof(TetrominoType));
            return new Tetromino((TetrominoType)values.GetValue(new Random().Next(values.Length)));
        }

        private void SpawnTetromino()
        {
            if (next == null || field == null) return;
            current = next;
            current.X = 3;
            current.Y = 0;
            next = GetRandomTetromino();
            if (!field.CanPlace(current, current.X, current.Y))
            {
                timer?.Stop();
                MessageBox.Show($"Игра окончена! Ваш счёт: {score}", "Тетрис");
            }
        }

        private void GameTick(object? sender, EventArgs e)
        {
            if (isPaused) return;
            MoveCurrent(0, 1);
        }

        private void MoveCurrent(int dx, int dy)
        {
            if (field == null || current == null) return;
            if (field.CanPlace(current, current.X + dx, current.Y + dy))
            {
                current.X += dx;
                current.Y += dy;
            }
            else if (dy == 1)
            {
                field.PlaceTetromino(current);
                int lines = field.ClearLines();
                if (lines > 0)
                {
                    score += lines * 100;
                    ScoreText.Text = score.ToString();
                }
                SpawnTetromino();
            }
            Draw();
        }

        private void RotateCurrent()
        {
            if (current == null || field == null) return;
            var oldShape = (int[,])current.Shape.Clone();
            current.Rotate();
            if (!field.CanPlace(current, current.X, current.Y))
                current.Shape = oldShape;
            Draw();
        }

        private void Draw()
        {
            GameCanvas.Children.Clear();
            if (field == null) return;
            // Рисуем поле
            for (int y = 0; y < field.Height; y++)
            {
                for (int x = 0; x < field.Width; x++)
                {
                    if (field.Field[x, y] != null)
                        DrawCell(x, y, field.Field[x, y]!.Value);
                }
            }
            // Рисуем текущую фигуру
            if (current != null)
            {
                var shape = current.Shape;
                for (int y = 0; y < shape.GetLength(0); y++)
                {
                    for (int x = 0; x < shape.GetLength(1); x++)
                    {
                        if (shape[y, x] == 1)
                            DrawCell(current.X + x, current.Y + y, current.Color);
                    }
                }
            }
            // Рисуем сетку
            for (int i = 0; i <= field.Width; i++)
            {
                var line = new Line
                {
                    X1 = i * cellSize,
                    Y1 = 0,
                    X2 = i * cellSize,
                    Y2 = field.Height * cellSize,
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 1
                };
                GameCanvas.Children.Add(line);
            }
            for (int i = 0; i <= field.Height; i++)
            {
                var line = new Line
                {
                    X1 = 0,
                    Y1 = i * cellSize,
                    X2 = field.Width * cellSize,
                    Y2 = i * cellSize,
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 1
                };
                GameCanvas.Children.Add(line);
            }
        }

        private void DrawCell(int x, int y, Color color)
        {
            var rect = new Rectangle
            {
                Width = cellSize - 2,
                Height = cellSize - 2,
                Fill = new SolidColorBrush(color),
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                RadiusX = 5,
                RadiusY = 5
            };
            Canvas.SetLeft(rect, x * cellSize + 1);
            Canvas.SetTop(rect, y * cellSize + 1);
            GameCanvas.Children.Add(rect);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (timer == null || !timer.IsEnabled || isPaused || field == null || current == null) return;
            switch (e.Key)
            {
                case Key.Left:
                    MoveCurrent(-1, 0);
                    break;
                case Key.Right:
                    MoveCurrent(1, 0);
                    break;
                case Key.Down:
                    MoveCurrent(0, 1);
                    break;
                case Key.Up:
                    RotateCurrent();
                    break;
                case Key.Space:
                    while (field.CanPlace(current, current.X, current.Y + 1))
                        current.Y++;
                    MoveCurrent(0, 1);
                    break;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (timer == null)
                InitGame();
            timer?.Start();
            isPaused = false;
            Focus();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            isPaused = !isPaused;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            timer?.Stop();
            InitGame();
            timer?.Start();
            isPaused = false;
            Focus();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.KeyDown += Window_KeyDown;
        }
    }
}