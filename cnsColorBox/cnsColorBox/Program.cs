using System;
using System.Collections.Generic;
using System.Linq;

class ColorBoxGame
{
    static Random random = new Random();
    static Dictionary<char, ConsoleColor> colorMap = new Dictionary<char, ConsoleColor>()
    {
        {'1', ConsoleColor.Red},
        {'2', ConsoleColor.Green},
        {'3', ConsoleColor.Blue},
        {'4', ConsoleColor.Yellow},
        {'5', ConsoleColor.Magenta},
        {'6', ConsoleColor.Cyan},
        {'7', ConsoleColor.DarkYellow},
        {'8', ConsoleColor.Gray},
        {'9', ConsoleColor.DarkGreen}
    };

    static int currentLevel = 1;
    static int score = 0;

    static void Main()
    {
        try
        {
            Console.CursorVisible = false;

            while (true)
            {
                PlayLevel();
                currentLevel++;
                Console.WriteLine($"\nУровень {currentLevel - 1} пройден! Нажмите Enter для продолжения...");
                WaitForEnter();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            WaitForEnter();
        }
    }

    static void WaitForEnter()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (input == "") break;
        }
    }

    static void PlayLevel()
    {
        int size = 3 + currentLevel / 2;
        char[,] map = GenerateMap(size, size, Math.Min(3 + currentLevel / 3, 6));
        var colorCounts = GetColorCounts(map);

        while (colorCounts.Count > 0)
        {
            Console.Clear();
            DisplayGameInfo();
            DisplayMap(map);
            DisplayColorStats(colorCounts);

            Console.Write("\nВыберите цвет (1-9) и нажмите Enter: ");
            char input = GetValidInput(colorCounts);

            if (colorCounts.ContainsKey(input) && colorCounts.First().Key == input)
            {
                score += 10 * currentLevel;
                colorCounts[input]--;

                if (colorCounts[input] == 0)
                    colorCounts.Remove(input);

                UpdateMap(ref map, input);
            }
            else
            {
                score = Math.Max(0, score - 5);
                Console.Beep();
                Console.WriteLine("Неверно! -5 очков");
                System.Threading.Thread.Sleep(1000);
            }
        }
    }

    static char[,] GenerateMap(int rows, int cols, int colorVariants)
    {
        char[,] map = new char[rows, cols];
        var elements = PrepareElements(rows * cols, colorVariants);

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                map[i, j] = elements[i * cols + j];

        return map;
    }

    static List<char> PrepareElements(int total, int variants)
    {
        var counts = CalculateCounts(total, variants);
        var elements = new List<char>();

        for (int i = 0; i < variants; i++)
            elements.AddRange(Enumerable.Repeat(colorMap.Keys.ElementAt(i), counts[i]));

        return Shuffle(elements);
    }

    static int[] CalculateCounts(int total, int variants)
    {
        var counts = new List<int>();
        int remaining = total;

        for (int i = 0; i < variants - 1; i++)
        {
            int max = remaining - (variants - i - 1);
            counts.Add(random.Next(1, max));
            remaining -= counts.Last();
        }
        counts.Add(remaining);

        return counts.OrderBy(x => random.Next()).ToArray();
    }

    static List<char> Shuffle(List<char> list)
    {
        return list.OrderBy(x => random.Next()).ToList();
    }

    static Dictionary<char, int> GetColorCounts(char[,] map)
    {
        var counts = new Dictionary<char, int>();

        foreach (char c in map)
            if (counts.ContainsKey(c)) counts[c]++;
            else counts[c] = 1;

        return counts.OrderByDescending(x => x.Value)
                    .ToDictionary(p => p.Key, p => p.Value);
    }

    static void DisplayGameInfo()
    {
        Console.WriteLine($"Уровень: {currentLevel}  Счет: {score}\n");
    }

    static void DisplayMap(char[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != ' ')
                {
                    Console.ForegroundColor = colorMap[map[i, j]];
                    Console.Write("■ ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write("  ");
                }
            }
            Console.WriteLine();
        }
    }

    static void DisplayColorStats(Dictionary<char, int> counts)
    {
        Console.WriteLine("\nОсталось:");
        foreach (var pair in counts)
        {
            Console.ForegroundColor = colorMap[pair.Key];
            Console.Write($"{pair.Key}:{pair.Value}  ");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    static char GetValidInput(Dictionary<char, int> counts)
    {
        while (true)
        {
            string input = Console.ReadLine()?.Trim();
            if (input?.Length == 1 && char.IsDigit(input[0]) && counts.ContainsKey(input[0]))
                return input[0];

            Console.WriteLine("Некорректный ввод. Попробуйте еще раз:");
        }
    }

    static void UpdateMap(ref char[,] map, char color)
    {
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                if (map[i, j] == color)
                    map[i, j] = ' ';
    }
}