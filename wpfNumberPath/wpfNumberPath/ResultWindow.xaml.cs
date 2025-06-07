using System.Windows;

namespace wpfNumberPath
{
    public partial class ResultWindow : Window
    {
        public ResultWindow(int levelsPassed)
        {
            InitializeComponent();
            LevelsPassedText.Text = $"Пройдено уровней: {levelsPassed}";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var startWindow = new StartWindow();
            startWindow.Show();
            this.Close();
        }
    }
} 