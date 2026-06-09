using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace Proect
{
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow(MainWindow.StatisticsModel stats)
        {
            InitializeComponent();
            DrawChart(stats);
        }

        private void DrawChart(MainWindow.StatisticsModel stats)    
        {
            ChartCanvas.Children.Clear();

            if (stats.DailyPomodoros.Count == 0)
                return;

            double width = 500;
            double height = 250;

            int max =
                stats.DailyPomodoros.Values.Max();

            double x = 30;

            foreach (var day in stats.DailyPomodoros)
            {
                double barHeight =
                    (double)day.Value / max * height;

                Rectangle rect = new Rectangle
                {
                    Width = 40,
                    Height = barHeight,
                    Fill = Brushes.Green
                };

                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, height - barHeight);

                ChartCanvas.Children.Add(rect);

                TextBlock txt = new TextBlock
                {
                    Text = day.Value.ToString()
                };

                Canvas.SetLeft(txt, x);
                Canvas.SetTop(txt, height - barHeight - 20);

                ChartCanvas.Children.Add(txt);

                x += 60;
            }
        }
    }
}