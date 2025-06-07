using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace wpfNumberPathTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random random = new Random();
        private List<int> numbers = new List<int>();
        private int targetSum;
        private List<Button> gridButtons = new List<Button>();
        private List<(int row, int col)> selectedPath = new List<(int row, int col)>();
        private bool isDrawing = false;
        private int currentPathLength;
        private int totalCorrectAnswers;
        private bool isLearningMode;
        private bool isTimeMode;
        private DispatcherTimer gameTimer;
        private int remainingTime = 60; // 60 секунд на игру
        private DifficultyLevel SelectedDifficulty;
        private const int gridSize = 4; // Размер сетки 4x4

        public MainWindow(bool isLearningMode, bool isTimeMode, DifficultyLevel difficulty)
        {
            MessageBox.Show("MainWindow создан!");
            InitializeComponent();
            this.isLearningMode = isLearningMode;
            this.isTimeMode = isTimeMode;
            SelectedDifficulty = difficulty;

            if (isTimeMode)
            {
                InitializeTimer();
            }

            StartNewGame();
        }

        private void InitializeTimer()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
            UpdateTimerDisplay();
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            remainingTime--;
            UpdateTimerDisplay();

            if (remainingTime <= 0)
            {
                gameTimer.Stop();
                MessageBox.Show($"Время вышло! Правильных ответов: {totalCorrectAnswers}", "Игра окончена", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        private void UpdateTimerDisplay()
        {
            if (TimeTextBlock != null)
            {
                TimeTextBlock.Text = $"Осталось времени: {remainingTime} сек";
            }
        }

        private void StartNewGame()
        {
            GameFieldGrid.Children.Clear();
            gridButtons.Clear();
            selectedPath.Clear();
            LinesCanvas.Children.Clear();
            UpdateSelectedNumbersDisplay();

            numbers = GenerateNumbers();

            switch (SelectedDifficulty)
            {
                case DifficultyLevel.Easy:
                    currentPathLength = random.Next(2, 4);
                    break;
                case DifficultyLevel.Medium:
                    currentPathLength = random.Next(3, 5);
                    break;
                case DifficultyLevel.Hard:
                    currentPathLength = random.Next(4, 6);
                    break;
            }

            targetSum = GenerateTargetSum();
            DisplayNumbersGrid();
            UpdateInfoDisplay();
        }

        private void DisplayNumbersGrid()
        {
            for (int i = 0; i < gridSize * gridSize; i++)
            {
                int row = i / gridSize;
                int col = i % gridSize;
                var btn = new Button
                {
                    Content = numbers[i].ToString(),
                    Style = (Style)FindResource("CalculatorButtonStyle"),
                    Tag = (row, col),
                    Background = new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold
                };
                btn.PreviewMouseLeftButtonDown += NumberButton_MouseLeftButtonDown;
                Grid.SetRow(btn, row);
                Grid.SetColumn(btn, col);
                GameFieldGrid.Children.Add(btn);
                gridButtons.Add(btn);
            }
        }

        private void NumberButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Button;
            var (row, col) = ((int, int))btn.Tag;

            // Если это первая клетка в пути
            if (selectedPath.Count == 0)
            {
                selectedPath.Add((row, col));
                btn.Background = Brushes.LightSkyBlue;
                UpdateSelectedNumbersDisplay();
                DrawPathLine();
                return;
            }

            // Проверяем, является ли клетка соседней с последней выбранной
            var lastCell = selectedPath.Last();
            if (!IsNeighbor(lastCell, (row, col)))
            {
                MessageBox.Show("Выберите соседнюю клетку!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверяем, не выбрана ли уже эта клетка
            if (selectedPath.Contains((row, col)))
            {
                MessageBox.Show("Эта клетка уже выбрана!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Добавляем клетку в путь
            selectedPath.Add((row, col));
            btn.Background = Brushes.LightSkyBlue;
            UpdateSelectedNumbersDisplay();
            DrawPathLine();

            // Проверяем, достигнута ли нужная длина пути
            if (selectedPath.Count == currentPathLength)
            {
                var nums = selectedPath.Select(idx => numbers[idx.row * gridSize + idx.col]).ToList();
                int sum = nums.Sum();

                if (sum == targetSum)
                {
                    totalCorrectAnswers++;
                    UpdateStatsDisplay();
                    MessageBox.Show("Правильно! Начинаем новую игру.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    StartNewGame();
                }
                else
                {
                    MessageBox.Show($"Неправильно! Сумма: {sum}. Попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    selectedPath.Clear();
                    UpdateSelectedNumbersDisplay();
                    DrawPathLine();

                    // Сбрасываем цвет всех кнопок
                    foreach (var b in gridButtons)
                        b.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                }
            }
        }

        private void NumberButton_MouseEnter(object sender, MouseEventArgs e)
        {
            // Этот метод больше не используется
        }

        private void NumberButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Этот метод больше не используется
        }

        private bool IsNeighbor((int row, int col) a, (int row, int col) b)
        {
            int dr = Math.Abs(a.row - b.row);
            int dc = Math.Abs(a.col - b.col);
            return (dr <= 1 && dc <= 1) && (dr + dc > 0);
        }

        private void DrawPathLine()
        {
            LinesCanvas.Children.Clear();
            if (selectedPath.Count < 2) return;

            var polyline = new Polyline
            {
                Stroke = Brushes.DeepSkyBlue,
                StrokeThickness = 6,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round
            };

            foreach (var tuple in selectedPath)
            {
                int row = tuple.row;
                int col = tuple.col;
                var btn = gridButtons.First(button => {
                    var tag = (ValueTuple<int, int>)button.Tag;
                    return tag.Item1 == row && tag.Item2 == col;
                });

                // Получаем координаты центра кнопки относительно Canvas
                var transform = btn.TransformToAncestor(LinesCanvas);
                var centerPoint = transform.Transform(new Point(btn.ActualWidth / 2, btn.ActualHeight / 2));
                polyline.Points.Add(centerPoint);
            }

            LinesCanvas.Children.Add(polyline);
        }

        private void UpdateSelectedNumbersDisplay()
        {
            if (SelectedNumbersTextBlock != null)
            {
                var nums = selectedPath.Select(idx => numbers[idx.row * gridSize + idx.col]).ToList();
                int sum = nums.Sum();
                SelectedNumbersTextBlock.Text = $"Выбранные числа: {string.Join(", ", nums)} (Сумма: {sum})";
            }
        }

        private void UpdateInfoDisplay()
        {
            if (InfoTextBlock != null)
            {
                if (isLearningMode)
                {
                    InfoTextBlock.Text = $"Найдите путь из {currentPathLength} чисел, сумма которых равна {targetSum}";
                }
                else if (isTimeMode)
                {
                    InfoTextBlock.Text = $"Найдите путь из {currentPathLength} чисел, сумма которых равна {targetSum} (Осталось времени: {remainingTime} сек)";
                }
                else
                {
                    InfoTextBlock.Text = $"Найдите путь из {currentPathLength} чисел, сумма которых равна {targetSum}";
                }
            }
        }

        private void UpdateStatsDisplay()
        {
            if (StatsTextBlock != null)
            {
                StatsTextBlock.Text = $"Правильных ответов: {totalCorrectAnswers}";
            }
        }

        private void CheckAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPath.Count != currentPathLength)
            {
                MessageBox.Show($"Выберите ровно {currentPathLength} числа!", "Неверное количество чисел", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedPath.Sum(idx => numbers[idx.row * gridSize + idx.col]) == targetSum)
            {
                totalCorrectAnswers++;
                UpdateStatsDisplay();
                MessageBox.Show("Правильно! Начинаем новую игру.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                StartNewGame();
            }
            else
            {
                int sum = selectedPath.Sum(idx => numbers[idx.row * gridSize + idx.col]);
                MessageBox.Show($"Неправильно! Сумма: {sum}. Попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                selectedPath.Clear();
                UpdateSelectedNumbersDisplay();
                DrawPathLine();
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private List<int> GenerateNumbers()
        {
            var result = new List<int>();
            int minNumber, maxNumber;

            switch (SelectedDifficulty)
            {
                case DifficultyLevel.Easy:
                    minNumber = 1;
                    maxNumber = 20;
                    break;
                case DifficultyLevel.Medium:
                    minNumber = 10;
                    maxNumber = 50;
                    break;
                case DifficultyLevel.Hard:
                    minNumber = 20;
                    maxNumber = 100;
                    break;
                default:
                    minNumber = 1;
                    maxNumber = 20;
                    break;
            }

            // Генерируем 16 случайных чисел
            while (result.Count < gridSize * gridSize)
            {
                int number = random.Next(minNumber, maxNumber + 1);
                if (!result.Contains(number))
                {
                    result.Add(number);
                }
            }

            return result;
        }

        private int GenerateTargetSum()
        {
            // Генерируем все возможные комбинации чисел заданной длины
            var combinations = GetCombinations(numbers, currentPathLength);
            
            // Выбираем случайную комбинацию, сумма которой не превышает максимально допустимую
            int maxSum = SelectedDifficulty switch
            {
                DifficultyLevel.Easy => 50,
                DifficultyLevel.Medium => 100,
                DifficultyLevel.Hard => 200,
                _ => 50
            };

            var validCombinations = combinations.Where(c => c.Sum() <= maxSum).ToList();
            if (validCombinations.Count == 0)
            {
                return GenerateNumbers().Take(currentPathLength).Sum();
            }

            return validCombinations[random.Next(validCombinations.Count)].Sum();
        }

        private List<List<int>> GetCombinations(List<int> numbers, int length)
        {
            var result = new List<List<int>>();
            GetCombinationsRecursive(numbers, length, 0, new List<int>(), result);
            return result;
        }

        private void GetCombinationsRecursive(List<int> numbers, int length, int start, List<int> current, List<List<int>> result)
        {
            if (current.Count == length)
            {
                result.Add(new List<int>(current));
                return;
            }

            for (int i = start; i < numbers.Count; i++)
            {
                current.Add(numbers[i]);
                GetCombinationsRecursive(numbers, length, i + 1, current, result);
                current.RemoveAt(current.Count - 1);
            }
        }
    }
}