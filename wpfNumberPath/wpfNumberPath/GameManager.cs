using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using wpfNumberPath.Models;

namespace wpfNumberPath
{
    public class GameManager
    {
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        public enum GameMode
        {
            Training,
            Testing
        }

        public Difficulty CurrentDifficulty { get; private set; }
        public GameMode CurrentGameMode { get; private set; }
        public int TargetSum { get; private set; }
        public List<NumberNode> Numbers { get; private set; }
        public List<NumberNode> SelectedNumbers { get; private set; }
        public List<NumberNode> SolutionPath { get; private set; }

        private Random random = new Random();

        public GameManager()
        {
            Numbers = new List<NumberNode>();
            SelectedNumbers = new List<NumberNode>();
            SolutionPath = new List<NumberNode>();
        }

        public void InitializeGame(Difficulty difficulty, GameMode gameMode)
        {
            CurrentDifficulty = difficulty;
            CurrentGameMode = gameMode;
            Numbers.Clear();
            SelectedNumbers.Clear();
            SolutionPath.Clear();

            int gridSize = GetGridSize(difficulty);
            GenerateNumbers(gridSize);
            GenerateSolution();
        }

        private int GetGridSize(Difficulty difficulty)
        {
            return difficulty switch
            {
                Difficulty.Easy => 3,
                Difficulty.Medium => 4,
                Difficulty.Hard => 5,
                _ => 3
            };
        }

        private void GenerateNumbers(int gridSize)
        {
            int totalCells = gridSize * gridSize;
            for (int i = 0; i < totalCells; i++)
            {
                Numbers.Add(new NumberNode
                {
                    Value = random.Next(1, 10),
                    Position = new Point(i % gridSize, i / gridSize)
                });
            }
        }

        private void GenerateSolution()
        {
            int pathLength = random.Next(3, 6);
            var availableNumbers = Numbers.ToList();
            int currentSum = 0;
            int maxSum = 20;
            switch (CurrentDifficulty)
            {
                case Difficulty.Medium:
                    maxSum = 40;
                    break;
                case Difficulty.Hard:
                    maxSum = 80;
                    break;
            }
            SolutionPath.Clear();
            while (SolutionPath.Count < pathLength && availableNumbers.Count > 0)
            {
                var nextNumber = availableNumbers[random.Next(availableNumbers.Count)];
                if (currentSum + nextNumber.Value <= maxSum)
                {
                    SolutionPath.Add(nextNumber);
                    currentSum += nextNumber.Value;
                    availableNumbers.Remove(nextNumber);
                }
                else
                {
                    availableNumbers.Remove(nextNumber);
                }
            }
            TargetSum = currentSum;
        }

        public bool CheckSolution()
        {
            int sum = SelectedNumbers.Sum(n => n.Value);
            return sum == TargetSum;
        }

        public void SelectNumber(NumberNode number)
        {
            if (!SelectedNumbers.Contains(number))
            {
                SelectedNumbers.Add(number);
                number.IsSelected = true;
            }
        }

        public void DeselectNumber(NumberNode number)
        {
            if (SelectedNumbers.Contains(number))
            {
                SelectedNumbers.Remove(number);
                number.IsSelected = false;
            }
        }

        public void ClearSelection()
        {
            foreach (var number in SelectedNumbers)
            {
                number.IsSelected = false;
            }
            SelectedNumbers.Clear();
        }
    }
} 