using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Text.Json;
using System.IO;
namespace Proect
{

    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan timeLeft;
        private enum TimerMode { Work, ShortBreak, LongBreak }
        private TimerMode _currentMode = TimerMode.Work;
         

        int completedPomodoros = 0;
        public MainWindow()
        {

            InitializeComponent();
            int pomodoroMinutes = int.Parse(PomodoroBox.Text);
            int shortBreakMinutes = int.Parse(ShortBreakBox.Text);
            int longBreakMinutes = int.Parse(LongBreakBox.Text);
            int cycles = int.Parse(CyclesBox.Text);
            timeLeft = TimeSpan.FromMinutes(25);
            TimerText.Text = timeLeft.ToString(@"mm\:ss");
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft.TotalSeconds > 0)
            {
                timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
                TimerText.Text = timeLeft.ToString(@"mm\:ss");
            }
            else
            {
                timer.Stop();
                MessageBox.Show("Pomodoro завершено!");
            }
            if (timeLeft == TimeSpan.Zero)
            {
                timer.Stop();

                if (_currentMode == TimerMode.Work)
                {
                    completedPomodoros++;

                    if (completedPomodoros % 4 == 0)
                    {
                        _currentMode = TimerMode.LongBreak;
                        timeLeft = TimeSpan.FromMinutes(15);
                    }
                    else
                    {
                        _currentMode = TimerMode.ShortBreak;
                        timeLeft = TimeSpan.FromMinutes(5);
                    }
                }
                else
                {
                    _currentMode = TimerMode.Work;
                    timeLeft = TimeSpan.FromMinutes(25);
                }

                TimerText.Text = timeLeft.ToString(@"mm\:ss");
            }

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timeLeft = TimeSpan.FromMinutes(25);
            TimerText.Text = timeLeft.ToString(@"mm\:ss");
        }

        private void SetWorkTime()
        {
            timeLeft = TimeSpan.FromMinutes(25);
            TimerText.Text = timeLeft.ToString(@"mm\:ss");
        }

        private void ShortBreakBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            timeLeft = TimeSpan.FromMinutes(5);

            TimerText.Text = timeLeft.ToString(@"mm\:ss");
        }
        private void LongBreakBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            timeLeft = TimeSpan.FromMinutes(15);

            TimerText.Text = timeLeft.ToString(@"mm\:ss");
        }
        public class SettingsModel
        {
            public int PomodoroMinutes { get; set; } = 25;
            public int ShortBreakMinutes { get; set; } = 5;
            public int LongBreakMinutes { get; set; } = 15;
            public int Cycles { get; set; } = 4;

            public static string FileName = "settings.json";

            public void Save()
            {
                string json = JsonSerializer.Serialize(this);
                File.WriteAllText(FileName, json);
            }

            public static SettingsModel Load()
            {
                if (!File.Exists(FileName))
                    return new SettingsModel();

                string json = File.ReadAllText(FileName);

                return JsonSerializer.Deserialize<SettingsModel>(json);
            }
        }
    }
}