using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace wfaColorBox
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        private Dictionary<char, Color> colorMap;
        private char[,] gameMap;
        private int currentLevel = 1;
        private int score = 0;
        private Dictionary<char, int> colorCounts;
        private int correctAnswers = 0;
        private int incorrectAnswers = 0;

        public Form1()
        {
            InitializeComponent();
            InitializeColorMap();
            StartNewGame();
        }

        private void InitializeColorMap()
        {
            colorMap = new Dictionary<char, Color>
            {
                {'1', Color.Red},
                {'2', Color.Green},
                {'3', Color.Blue},
                {'4', Color.Yellow},
                {'5', Color.Magenta},
                {'6', Color.Cyan},
                {'7', Color.Orange},
                {'8', Color.Purple},
                {'9', Color.Lime}
            };
        }

        private void StartNewGame()
        {
            panelGame.Controls.Clear();

            int size = 3 + currentLevel / 2;
            gameMap = GenerateGameMap(size, size, Math.Min(3 + currentLevel / 3, 6));

            colorCounts = CountColors(gameMap);
            CreateGameButtons(gameMap);

            lblLevel.Text = $"Уровень: {currentLevel}";
            lblScore.Text = $"Счет: {score}";
            lblCorrect.Text = $"Правильно: {correctAnswers}";
            lblIncorrect.Text = $"Ошибки: {incorrectAnswers}";
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
            int buttonSize = Math.Min(panelGame.Width / map.GetLength(1), panelGame.Height / map.GetLength(0)) - 5;
            int totalHeight = map.GetLength(0) * (buttonSize + 5) - 5;
            int yOffset = panelGame.Height - totalHeight;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    var button = new Button
                    {
                        Width = buttonSize,
                        Height = buttonSize,
                        Left = j * (buttonSize + 5),
                        Top = yOffset + i * (buttonSize + 5),
                        BackColor = colorMap[map[i, j]],
                        Tag = new Point(i, j), // Сохраняем позицию кнопки в массиве
                        FlatStyle = FlatStyle.Flat
                    };

                    button.FlatAppearance.BorderSize = 0;
                    button.Click += GameButton_Click;
                    panelGame.Controls.Add(button);
                }
            }
        }

        private void GameButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            Point position = (Point)button.Tag;
            char colorChar = gameMap[position.X, position.Y];

            // Проверяем, является ли этот цвет самым распространенным
            if (colorCounts.Any() && colorCounts.First().Key == colorChar)
            {
                // Удаляем все кнопки этого цвета
                foreach (Control control in panelGame.Controls)
                {
                    if (control is Button btn)
                    {
                        Point btnPos = (Point)btn.Tag;
                        if (gameMap[btnPos.X, btnPos.Y] == colorChar)
                        {
                            btn.Visible = false;
                        }
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
                incorrectAnswers++;
                lblScore.Text = $"Счет: {score}";
                lblIncorrect.Text = $"Ошибки: {incorrectAnswers}";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblCorrect.Text = $"Правильно: {correctAnswers}";
            lblIncorrect.Text = $"Ошибки: {incorrectAnswers}";
        }
    }
}