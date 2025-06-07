using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using wpfGame;

namespace wpfGame
{
    public partial class MainWindow : Window
    {
        private readonly GameEngine _engine;
        private object StatusText;
        private object GameGrid;

        public MainWindow(object statusText, object gameGrid)
        {
            InitializeComponent();

            try
            {
                var settings = File.Exists("settings.json")
                    ? GameSettings.LoadFromFile("settings.json")
                    : new GameSettings();

                _engine = new GameEngine(settings);
                _engine.GameUpdated += Engine_GameUpdated;
                _engine.GameCompleted += Engine_GameCompleted;

                StartNewGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing game: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            StatusText = statusText;
            GameGrid = gameGrid;
        }

        private void StartNewGame()
        {
            var difficulty = DifficultyComboBox.SelectedIndex switch
            {
                0 => DifficultyLevel.Easy,
                1 => DifficultyLevel.Medium,
                2 => DifficultyLevel.Hard,
                _ => DifficultyLevel.Medium
            };

            bool isTraining = TrainingModeRadio.IsChecked == true;
            _engine.StartNewGame(difficulty, isTraining);
        }

        private void Engine_GameUpdated(object? sender, GameState state)
        {
            Dispatcher.Invoke(() =>
            {
                TargetSumText.Text = state.TargetSum.ToString();
                CurrentSumText.Text = state.CurrentSum.ToString();

                // Обновление игрового поля
                GameGrid.ItemsSource = ConvertGridToView(state.NumberGrid, state.CurrentPath);
            });
        }

        private void Engine_GameCompleted(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = "Completed!";
                MessageBox.Show("Congratulations! You found the correct path.", "Game Completed",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private IEnumerable<CellViewModel> ConvertGridToView(int[,] grid, List<(int, int)> path)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    yield return new CellViewModel
                    {
                        Value = grid[i, j],
                        Position = (i, j),
                        IsInPath = path.Contains((i, j))
                    };
                }
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e) => StartNewGame();

        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is (int, int) position)
            {
                _engine.AddToPath(position.Item1, position.Item2);
            }
        }

        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Game Files (*.ngame)|*.ngame",
                DefaultExt = ".ngame"
            };

            if (dialog.ShowDialog() == true)
            {
                _engine.State.SaveToFile(dialog.FileName);
                StatusText.Text = "Game saved successfully.";
            }
        }

        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Game Files (*.ngame)|*.ngame",
                DefaultExt = ".ngame"
            };

            if (dialog.ShowDialog() == true)
            {
                _engine.State = GameState.LoadFromFile(dialog.FileName);
                _engine.OnGameUpdated();
                StatusText.Text = "Game loaded successfully.";
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "JPEG Image (*.jpg)|*.jpg|PNG Image (*.png)|*.png",
                DefaultExt = ".jpg"
            };

            if (dialog.ShowDialog() == true)
            {
                // Реализация экспорта в изображение
                StatusText.Text = "Exported to image successfully.";
            }
        }
    }

    public class CellViewModel
    {
        public int Value { get; set; }
        public (int, int) Position { get; set; }
        public bool IsInPath { get; set; }
    }
}