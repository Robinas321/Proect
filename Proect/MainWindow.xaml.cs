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

namespace Proect
{

    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan timeLeft;
        private enum TimerMode { Work, ShortBreak, LongBreak }
        private TimerMode _currentMode = TimerMode.Work;

        public MainWindow()
        {

            InitializeComponent();

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
    }
}