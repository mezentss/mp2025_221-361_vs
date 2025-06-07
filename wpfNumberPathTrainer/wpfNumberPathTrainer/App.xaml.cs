using System.Configuration;
using System.Data;
using System.Windows;

namespace wpfNumberPathTrainer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var settingsWindow = new StartWindow();
            if (settingsWindow.ShowDialog() == true)
            {
                try
                {
                    var mainWindow = new MainWindow(
                        settingsWindow.IsLearningMode,
                        settingsWindow.IsTimeMode,
                        settingsWindow.SelectedDifficulty
                    );
                    mainWindow.Show();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Ошибка при запуске игрового окна:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown();
                }
            }
            else
            {
                Shutdown();
            }
        }
    }
}
