using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using wpfNumberPath.Controls;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace wpfNumberPath
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameManager gameManager;
        private List<Controls.NumberNodeControl> numberControls;
        private DispatcherTimer? levelTimer;
        private int timeLeft;
        private int levelsPassed = 0;
        private bool isGameMode => (SelectedGameMode ?? GameManager.GameMode.Training) == GameManager.GameMode.Testing;
        private bool isMouseDown = false;
        private NumberNodeControl? lastHoveredControl = null;

        public GameManager.Difficulty? SelectedDifficulty { get; set; }
        public GameManager.GameMode? SelectedGameMode { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            gameManager = new GameManager();
            numberControls = new List<Controls.NumberNodeControl>();
            InitializeGame();
            if (isGameMode)
                StartLevelTimer();
        }

        private void InitializeGame()
        {
            GameManager.Difficulty difficulty = SelectedDifficulty ?? GameManager.Difficulty.Easy;
            GameManager.GameMode gameMode = SelectedGameMode ?? GameManager.GameMode.Training;
            gameManager.InitializeGame(difficulty, gameMode);
            DrawGameBoard();
            UpdateTargetSumText();
            gameManager.ClearSelection();
            UpdateVisuals();
            UpdateLevelCounter();
            if (gameMode == GameManager.GameMode.Training)
            {
                if (levelTimer != null) levelTimer.Stop();
                TimerText.Visibility = Visibility.Collapsed;
            }
            else
            {
                StartLevelTimer();
                TimerText.Visibility = Visibility.Visible;
            }
        }

        private void DrawGameBoard()
        {
            NumbersGrid.Children.Clear();
            NumbersGrid.RowDefinitions.Clear();
            NumbersGrid.ColumnDefinitions.Clear();
            numberControls.Clear();
            LinesCanvas.Children.Clear();

            int gridSize = (int)Math.Sqrt(gameManager.Numbers.Count);
            for (int i = 0; i < gridSize; i++)
            {
                NumbersGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                NumbersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            foreach (var number in gameManager.Numbers)
            {
                var control = new Controls.NumberNodeControl();
                control.SetNode(number);
                control.Margin = new Thickness(8);
                control.MouseDown += NumberControl_MouseDown;
                control.MouseUp += NumberControl_MouseUp;
                control.MouseMove += NumberControl_MouseMove;
                Grid.SetRow(control, (int)number.Position.Y);
                Grid.SetColumn(control, (int)number.Position.X);
                NumbersGrid.Children.Add(control);
                numberControls.Add(control);
            }
        }

        private void NumberControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isMouseDown = true;
                var control = sender as NumberNodeControl;
                if (control != null)
                {
                    lastHoveredControl = control;
                    SelectNumber(control);
                }
            }
        }

        private void NumberControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && isMouseDown)
            {
                isMouseDown = false;
                lastHoveredControl = null;
                CheckPath();
            }
        }

        private void NumberControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                var control = sender as NumberNodeControl;
                if (control != null && control != lastHoveredControl)
                {
                    lastHoveredControl = control;
                    SelectNumber(control);
                }
            }
        }

        private void SelectNumber(NumberNodeControl control)
        {
            if (control == null) return;

            // Проверка на соседство
            if (!control.Node.IsSelected && gameManager.SelectedNumbers.Count > 0)
            {
                var last = gameManager.SelectedNumbers.Last();
                int dx = (int)Math.Abs(control.Node.Position.X - last.Position.X);
                int dy = (int)Math.Abs(control.Node.Position.Y - last.Position.Y);
                if (dx > 1 || dy > 1 || (dx == 0 && dy == 0))
                {
                    return;
                }
            }

            if (control.Node.IsSelected)
            {
                gameManager.DeselectNumber(control.Node);
            }
            else
            {
                gameManager.SelectNumber(control.Node);
            }

            UpdateVisuals();
        }

        private async void CheckPath()
        {
            int sum = gameManager.SelectedNumbers.Sum(n => n.Value);
            if (sum == gameManager.TargetSum)
            {
                levelsPassed++;
                await HighlightFieldAsync(Colors.LightGreen);
                InitializeGame();
            }
            else if (sum > gameManager.TargetSum)
            {
                await HighlightFieldAsync(Colors.IndianRed);
                gameManager.ClearSelection();
                UpdateVisuals();
            }
        }

        private async Task HighlightFieldAsync(Color color)
        {
            GameFieldBorder.Background = new SolidColorBrush(color);
            await Task.Delay(1000);
            GameFieldBorder.Background = Brushes.White;
        }

        private void UpdateVisuals()
        {
            foreach (var control in numberControls)
            {
                control.UpdateVisual();
            }
            UpdateFormulaText();
            DrawLines();
        }

        private void DrawLines()
        {
            LinesCanvas.Children.Clear();
            if (gameManager.SelectedNumbers.Count < 2) return;

            for (int i = 0; i < gameManager.SelectedNumbers.Count - 1; i++)
            {
                var currentNumber = gameManager.SelectedNumbers[i];
                var nextNumber = gameManager.SelectedNumbers[i + 1];
                
                var currentControl = numberControls.First(c => c.Node == currentNumber);
                var nextControl = numberControls.First(c => c.Node == nextNumber);

                // Получаем позиции элементов относительно GameFieldBorder
                var currentPos = currentControl.TransformToAncestor(GameFieldBorder)
                    .Transform(new Point(currentControl.ActualWidth / 2, currentControl.ActualHeight / 2));
                var nextPos = nextControl.TransformToAncestor(GameFieldBorder)
                    .Transform(new Point(nextControl.ActualWidth / 2, nextControl.ActualHeight / 2));

                var line = new Line
                {
                    Stroke = new SolidColorBrush(Color.FromRgb(33, 150, 243)), 
                    StrokeThickness = 4,
                    X1 = currentPos.X,
                    Y1 = currentPos.Y,
                    X2 = nextPos.X,
                    Y2 = nextPos.Y,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };

                LinesCanvas.Children.Add(line);
            }
        }

        private void UpdateFormulaText()
        {
            if (gameManager.SelectedNumbers.Count == 0)
            {
                FormulaText.Text = "???";
                FormulaText.Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180));
                return;
            }

            if (isGameMode)
            {
                FormulaText.Text = string.Join(" + ", gameManager.SelectedNumbers.Select(n => n.Value));
            }
            else
            {
                int sum = gameManager.SelectedNumbers.Sum(n => n.Value);
                FormulaText.Text = string.Join(" + ", gameManager.SelectedNumbers.Select(n => n.Value)) + " = " + sum;
            }
            FormulaText.Foreground = new SolidColorBrush(Color.FromRgb(34, 34, 34));
        }

        private void ShowSolution()
        {
            foreach (var number in gameManager.SolutionPath)
            {
                number.IsPartOfSolution = true;
            }
            UpdateVisuals();
        }

        private void UpdateTargetSumText()
        {
            TargetSumText.Text = $"Найдите путь для чисел, чтобы их сумма была равна: {gameManager.TargetSum}";
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (gameManager.CheckSolution())
            {
                MessageBox.Show("Поздравляем! Вы нашли правильный путь!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Попробуйте еще раз!", "Неверно", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            gameManager.ClearSelection();
            UpdateVisuals();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var startWindow = new StartWindow();
            startWindow.Show();
            this.Close();
        }

        private void StartLevelTimer()
        {
            if (!isGameMode) return;
            if (levelTimer != null)
            {
                levelTimer.Stop();
            }
            levelTimer = new DispatcherTimer();
            levelTimer.Interval = TimeSpan.FromSeconds(1);
            levelTimer.Tick += LevelTimer_Tick;
            timeLeft = GetLevelTime();
            levelTimer.Start();
            UpdateTimerDisplay();
        }

        private int GetLevelTime()
        {
            var diff = SelectedDifficulty ?? GameManager.Difficulty.Easy;
            if (diff == GameManager.Difficulty.Easy) return 90;
            return 60;
        }

        private void LevelTimer_Tick(object? sender, EventArgs e)
        {
            timeLeft--;
            UpdateTimerDisplay();
            if (timeLeft <= 0)
            {
                levelTimer?.Stop();
                var resultWindow = new ResultWindow(levelsPassed);
                resultWindow.Show();
                this.Close();
            }
        }

        private void UpdateTimerDisplay()
        {
            if (!isGameMode)
            {
                TimerText.Visibility = Visibility.Collapsed;
                return;
            }
            TimerText.Visibility = Visibility.Visible;
            TimerText.Text = $"⏰ {timeLeft / 60:D2}:{timeLeft % 60:D2}";
        }

        private void UpdateLevelCounter()
        {
            LevelCounterText.Text = levelsPassed.ToString();
        }

        private async void HintButton_Click(object sender, RoutedEventArgs e)
        {
            if ((SelectedGameMode ?? GameManager.GameMode.Training) == GameManager.GameMode.Training)
            {
                foreach (var node in gameManager.SolutionPath)
                    node.IsPartOfSolution = true;
                UpdateVisuals();
                await Task.Delay(3000);
                foreach (var node in gameManager.SolutionPath)
                    node.IsPartOfSolution = false;
                UpdateVisuals();
            }
            else
            {
                var first = gameManager.SolutionPath.FirstOrDefault(n => !n.IsSelected);
                if (first != null)
                {
                    first.IsPartOfSolution = true;
                    UpdateVisuals();
                    await Task.Delay(3000);
                    first.IsPartOfSolution = false;
                    UpdateVisuals();
                }
            }
        }
    }
}