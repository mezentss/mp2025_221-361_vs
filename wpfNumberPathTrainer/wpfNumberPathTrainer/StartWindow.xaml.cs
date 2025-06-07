using System.Windows;
using System.Windows.Media;

namespace wpfNumberPathTrainer
{
    public partial class StartWindow : Window
    {
        private bool isLearningMode = true;
        private DifficultyLevel selectedDifficulty = DifficultyLevel.Easy;

        public bool IsLearningMode => isLearningMode;
        public bool IsTimeMode => !isLearningMode;
        public DifficultyLevel SelectedDifficulty => selectedDifficulty;

        public StartWindow()
        {
            InitializeComponent();
            LearningModeButton.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(25, 118, 210));
            EasyButton.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(25, 118, 210));
        }

        private void LearningModeButton_Click(object sender, RoutedEventArgs e)
        {
            isLearningMode = true;
            LearningModeButton.Background = new SolidColorBrush(Color.FromRgb(25, 118, 210));
            GameModeButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
        }

        private void GameModeButton_Click(object sender, RoutedEventArgs e)
        {
            isLearningMode = false;
            GameModeButton.Background = new SolidColorBrush(Color.FromRgb(25, 118, 210));
            LearningModeButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            selectedDifficulty = DifficultyLevel.Easy;
            EasyButton.Background = new SolidColorBrush(Color.FromRgb(25, 118, 210));
            MediumButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            HardButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
        }

        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            selectedDifficulty = DifficultyLevel.Medium;
            MediumButton.Background = new SolidColorBrush(Color.FromRgb(25, 118, 210));
            EasyButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            HardButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
        }

        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            selectedDifficulty = DifficultyLevel.Hard;
            HardButton.Background = new SolidColorBrush(Color.FromRgb(25, 118, 210));
            EasyButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
            MediumButton.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
} 