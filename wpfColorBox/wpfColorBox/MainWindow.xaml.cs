using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorBoxWPF
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private Dictionary<char, SolidColorBrush> colorMap;
        private char[,] gameMap;
        private int currentLevel = 1;
        private int score = 0;
        private Dictionary<char, int> colorCounts;
        private int correctAnswers = 0;
        private int wrongAnswers = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeColorMap();
            StartNewGame();
        }

        private void InitializeColorMap()
        {
            colorMap = new Dictionary<char, SolidColorBrush>
            {
                {'1', Brushes.Red},
                {'2', Brushes.Green},
                {'3', Brushes.Blue},
                {'4', Brushes.Yellow},
                {'5', Brushes.Magenta},
                {'6', Brushes.Cyan},
                {'7', Brushes.Orange},
                {'8', Brushes.Purple},
                {'9', Brushes.Lime}
            };
        }

        private void StartNewGame()
        {
            gameGrid.Children.Clear();
            gameGrid.Rows = 0;
            gameGrid.Columns = 0;

            int size = 3 + currentLevel / 2;
            gameMap = GenerateGameMap(size, size, Math.Min(3 + currentLevel / 3, 6));

            colorCounts = CountColors(gameMap);
            CreateGameButtons(gameMap);

            lblLevel.Text = $"Уровень: {currentLevel}";
            lblScore.Text = $"Счет: {score}";
            lblCorrect.Text = $"Правильно: {correctAnswers}";
            lblWrong.Text = $"Ошибки: {wrongAnswers}";
        }

        private char[,] GenerateGameMap(int rows, int cols, int colorVariants)
        {
            var map = new char[rows, cols];
            var elements = PrepareGameElements(rows * cols, colorVariants);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    map[i, j] = elements.Dequeue();

            return map;
        }

        private Queue<char> PrepareGameElements(int totalCells, int colorVariants)
        {
            var colorCounts = CalculateColorDistribution(totalCells, colorVariants);
            var colors = colorMap.Keys.Take(colorVariants).ToArray();
            var elements = new List<char>();

            for (int i = 0; i < colorVariants; i++)
                elements.AddRange(Enumerable.Repeat(colors[i], colorCounts[i]));

            return new Queue<char>(elements.OrderBy(_ => random.Next()));
        }

        private int[] CalculateColorDistribution(int totalCells, int variants)
        {
            var counts = new List<int>();
            int remaining = totalCells;

            for (int i = 0; i < variants - 1; i++)
            {
                int max = remaining - (variants - i - 1);
                counts.Add(random.Next(1, max));
                remaining -= counts.Last();
            }

            counts.Add(remaining);
            return counts.OrderBy(_ => random.Next()).ToArray();
        }

        private Dictionary<char, int> CountColors(char[,] map)
        {
            var counts = new Dictionary<char, int>();

            foreach (char c in map)
                if (counts.ContainsKey(c)) counts[c]++;
                else counts[c] = 1;

            return counts.OrderByDescending(x => x.Value)
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private void CreateGameButtons(char[,] map)
        {
            gameGrid.Rows = map.GetLength(0);
            gameGrid.Columns = map.GetLength(1);

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    var button = new Button
                    {
                        Background = colorMap[map[i, j]],
                        Tag = new Point(i, j),
                        Margin = new Thickness(2)
                    };

                    button.Click += GameButton_Click;
                    gameGrid.Children.Add(button);
                }
            }
        }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            Point position = (Point)button.Tag;
            char colorChar = gameMap[(int)position.X, (int)position.Y];

            // Проверяем, является ли этот цвет самым распространенным
            if (colorCounts.Any() && colorCounts.First().Key == colorChar)
            {
                // Удаляем все кнопки этого цвета
                foreach (var child in gameGrid.Children.OfType<Button>().ToList())
                {
                    Point btnPos = (Point)child.Tag;
                    if (gameMap[(int)btnPos.X, (int)btnPos.Y] == colorChar)
                    {
                        gameGrid.Children.Remove(child);
                    }
                }

                // Обновляем счет
                score += 10 * currentLevel * colorCounts[colorChar];
                colorCounts.Remove(colorChar);
                correctAnswers++;
                lblCorrect.Text = $"Правильно: {correctAnswers}";

                if (!colorCounts.Any())
                {
                    currentLevel++;
                    // MessageBox.Show($"Уровень {currentLevel - 1} пройден!", "Поздравляем"); // Убрано по просьбе пользователя
                    StartNewGame();
                }
                else
                {
                    lblScore.Text = $"Счет: {score}";
                }
            }
            else
            {
                score = Math.Max(0, score - 5);
                wrongAnswers++;
                lblScore.Text = $"Счет: {score}";
                lblWrong.Text = $"Ошибки: {wrongAnswers}";
            }
        }
    }
}