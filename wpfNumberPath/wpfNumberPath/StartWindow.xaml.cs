using System.Windows;

namespace wpfNumberPath
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.SelectedDifficulty = GetSelectedDifficulty();
            mainWindow.SelectedGameMode = GetSelectedGameMode();
            mainWindow.Show();
            this.Close();
        }

        private GameManager.Difficulty GetSelectedDifficulty()
        {
            if (EasyRadio.IsChecked == true) return GameManager.Difficulty.Easy;
            if (MediumRadio.IsChecked == true) return GameManager.Difficulty.Medium;
            return GameManager.Difficulty.Hard;
        }

        private GameManager.GameMode GetSelectedGameMode()
        {
            if (TrainingRadio.IsChecked == true) return GameManager.GameMode.Training;
            return GameManager.GameMode.Testing;
        }
    }
} 