using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfGame
{
    public class GameState
    {
        public int[,] NumberGrid { get; set; }
        public List<(int, int)> CurrentPath { get; set; } = new();
        public int TargetSum { get; set; }
        public int CurrentSum { get; set; }
        public bool IsTrainingMode { get; set; }
        public bool IsCompleted { get; set; }

        public void SaveToFile(string filePath)
        {
            // Реализация сохранения состояния игры
        }

        public static GameState LoadFromFile(string filePath)
        {
            // Реализация загрузки состояния игры
            return new GameState();
        }
    }
}
