using System.Text.Json;

namespace wpfGame
{
    public class GameSettings
    {
        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;
        public string FontFamily { get; set; } = "Consolas";
        public int GridSize { get; set; } = 5;

        public void SaveToFile(string filePath)
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText(filePath, json);
        }

        public static GameSettings LoadFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<GameSettings>(json) ?? new GameSettings();
        }
    }
}