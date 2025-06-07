using wpfGame;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        // Обработка аргументов командной строки
        if (args.Length > 0)
        {
            HandleConsoleMode(args);
            return;
        }

        // Запуск GUI приложения
        StartGuiApplication();
    }

    static void HandleConsoleMode(string[] args)
    {
        var settings = LoadSettings();
        var engine = new GameEngine(settings);

        if (args[0] == "--training")
        {
            var difficulty = ParseDifficulty(args.Length > 1 ? args[1] : "medium");
            engine.StartNewGame(difficulty, true);
            DisplayGame(engine.State);
        }
        else if (args[0] == "--challenge")
        {
            var difficulty = ParseDifficulty(args.Length > 1 ? args[1] : "medium");
            engine.StartNewGame(difficulty, false);
            DisplayGame(engine.State);

            // В реальном приложении здесь была бы интерактивная игра
        }
        else if (args[0] == "--export" && args.Length > 1)
        {
            // Экспорт данных
            ExportGameState(args[1]);
        }
    }

    static DifficultyLevel ParseDifficulty(string diff)
    {
        return diff.ToLower() switch
        {
            "easy" => DifficultyLevel.Easy,
            "medium" => DifficultyLevel.Medium,
            "hard" => DifficultyLevel.Hard,
            _ => DifficultyLevel.Medium
        };
    }

    static GameSettings LoadSettings()
    {
        try
        {
            return File.Exists("settings.json")
                ? GameSettings.LoadFromFile("settings.json")
                : new GameSettings();
        }
        catch
        {
            return new GameSettings();
        }
    }

    static void DisplayGame(GameState state)
    {
        Console.WriteLine($"Target Sum: {state.TargetSum}");
        Console.WriteLine("Number Grid:");

        for (int i = 0; i < state.NumberGrid.GetLength(0); i++)
        {
            for (int j = 0; j < state.NumberGrid.GetLength(1); j++)
            {
                Console.Write($"{state.NumberGrid[i, j],4}");
            }
            Console.WriteLine();
        }

        if (state.IsTrainingMode && state.IsCompleted)
        {
            Console.WriteLine("\nSolution Path:");
            foreach (var (i, j) in state.CurrentPath)
            {
                Console.WriteLine($"({i}, {j}): {state.NumberGrid[i, j]}");
            }
        }
    }

    static void ExportGameState(string filePath)
    {
        // Реализация экспорта
    }

    static void StartGuiApplication()
    {
        // В реальном приложении здесь был бы запуск WPF/WinForms приложения
        Console.WriteLine("Starting GUI application...");
        Process.Start("NumberPathTrainer.UI.exe");
    }
}