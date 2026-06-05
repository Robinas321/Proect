using System;
using System.IO;
using System.Media;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
namespace Proect
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan timeLeft;
        private enum TimerMode { Work, ShortBreak, LongBreak }

        private TimerMode currentMode = TimerMode.Work;
        private int completedPomodoros = 0;
        private SettingsModel settings;

        public MainWindow()
        {
            InitializeComponent();

            settings = SettingsModel.Load();

            PomodoroBox.Text = settings.PomodoroMinutes.ToString();
            ShortBreakBox.Text = settings.ShortBreakMinutes.ToString();
            LongBreakBox.Text = settings.LongBreakMinutes.ToString();
            CyclesBox.Text = settings.Cycles.ToString();
            timeLeft = TimeSpan.FromMinutes(settings.PomodoroMinutes);
            TimerText.Text = timeLeft.ToString(@"mm\:ss");

            CompletedText.Text = $"Завершено: {completedPomodoros}";

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft.TotalSeconds > 0)
            {
                timeLeft -= TimeSpan.FromSeconds(1);
                TimerText.Text = timeLeft.ToString(@"mm\:ss");
                return;
            }

            timer.Stop();

            if (currentMode == TimerMode.Work)
            {
                completedPomodoros++;


                SystemSounds.Exclamation.Play();
                CompletedText.Text = $"Завершено: {completedPomodoros}";


                if (completedPomodoros % settings.Cycles == 0)
                {
                    currentMode = TimerMode.LongBreak;
                    timeLeft = TimeSpan.FromMinutes(settings.LongBreakMinutes);
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show("Час для довгої перерви!", "",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information,
                        MessageBoxResult.OK,
                        MessageBoxOptions.ServiceNotification
                    );

                }
                else
                {
                    currentMode = TimerMode.ShortBreak;
                    timeLeft = TimeSpan.FromMinutes(settings.ShortBreakMinutes);
                    MessageBox.Show("Час для короткої перерви!", "",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information,
                        MessageBoxResult.OK,
                        MessageBoxOptions.ServiceNotification
                    );

                }
            }
            else
            {
                currentMode = TimerMode.Work;
                timeLeft = TimeSpan.FromMinutes(settings.PomodoroMinutes);
                MessageBox.Show("Повертаємося до роботи!", "",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information,
                        MessageBoxResult.OK,
                        MessageBoxOptions.ServiceNotification
                    );

            }

            TimerText.Text = timeLeft.ToString(@"mm\:ss");
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            timer.Start();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            SaveSettings();

            currentMode = TimerMode.Work;
            timeLeft = TimeSpan.FromMinutes(settings.PomodoroMinutes);

            TimerText.Text = timeLeft.ToString(@"mm\:ss");
        }

        private void ShortBreakBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            SaveSettings();
            currentMode = TimerMode.ShortBreak;
            timeLeft = TimeSpan.FromMinutes(settings.ShortBreakMinutes);
            TimerText.Text = timeLeft.ToString(@"mm\:ss");
        }

        private void LongBreakBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            SaveSettings();
            currentMode = TimerMode.LongBreak;
            timeLeft = TimeSpan.FromMinutes(settings.LongBreakMinutes);
            TimerText.Text = timeLeft.ToString(@"mm\:ss");
        }

        private void SaveSettings()
        {
            if (int.TryParse(PomodoroBox.Text, out int pomodoro))
                settings.PomodoroMinutes = pomodoro;

            if (int.TryParse(ShortBreakBox.Text, out int shortBreak))
                settings.ShortBreakMinutes = shortBreak;

            if (int.TryParse(LongBreakBox.Text, out int longBreak))
                settings.LongBreakMinutes = longBreak;

            if (int.TryParse(CyclesBox.Text, out int cycles))
                settings.Cycles = cycles;

            settings.Save();
        }

        
    }
}