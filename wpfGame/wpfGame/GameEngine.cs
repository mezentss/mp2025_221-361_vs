using System;
using System.Collections.Generic;
using wpfGame;

namespace wpfGame
{
    public class GameEngine
    {
        private readonly Random _random = new();
        public GameState State { get; private set; }
        public GameSettings Settings { get; private set; }

        public event EventHandler<GameState>? GameUpdated;
        public event EventHandler? GameCompleted;

        public GameEngine(GameSettings settings)
        {
            Settings = settings;
            State = new GameState();
        }

        public void StartNewGame(DifficultyLevel difficulty, bool isTrainingMode)
        {
            int size = difficulty switch
            {
                DifficultyLevel.Easy => 3,
                DifficultyLevel.Medium => 5,
                DifficultyLevel.Hard => 7,
                _ => 5
            };

            State = new GameState
            {
                NumberGrid = GenerateGrid(size, difficulty),
                IsTrainingMode = isTrainingMode,
                IsCompleted = false
            };

            // Установка целевой суммы
            State.TargetSum = CalculateTargetSum(State.NumberGrid, difficulty);

            if (isTrainingMode)
            {
                State.CurrentPath = PathFinder.FindPath(State.NumberGrid, State.TargetSum);
                State.IsCompleted = true;
            }

            OnGameUpdated();
        }

        private int[,] GenerateGrid(int size, DifficultyLevel difficulty)
        {
            var grid = new int[size, size];
            int min = difficulty == DifficultyLevel.Hard ? -10 : 1;
            int max = difficulty switch
            {
                DifficultyLevel.Easy => 10,
                DifficultyLevel.Medium => 20,
                DifficultyLevel.Hard => 30,
                _ => 20
            };

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = _random.Next(min, max);
                }
            }

            return grid;
        }

        private int CalculateTargetSum(int[,] grid, DifficultyLevel difficulty)
        {
            // Генерация целевой суммы на основе сложности
            int pathLength = difficulty switch
            {
                DifficultyLevel.Easy => 3,
                DifficultyLevel.Medium => 5,
                DifficultyLevel.Hard => 7,
                _ => 5
            };

            // В реальной реализации здесь должен быть поиск существующего пути
            return 42; // Примерное значение
        }

        public void AddToPath(int row, int col)
        {
            if (State.IsCompleted) return;

            var cell = (row, col);
            if (!State.CurrentPath.Contains(cell))
            {
                State.CurrentPath.Add(cell);
                State.CurrentSum += State.NumberGrid[row, col];

                if (PathFinder.ValidatePath(State.CurrentPath, State.NumberGrid, State.TargetSum))
                {
                    State.IsCompleted = true;
                    OnGameCompleted();
                }

                OnGameUpdated();
            }
        }

        protected virtual void OnGameUpdated()
        {
            GameUpdated?.Invoke(this, State);
        }

        protected virtual void OnGameCompleted()
        {
            GameCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}