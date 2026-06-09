using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using static Proect.MainWindow;
namespace Proect
{
    public class StatisticsModel
    {
        public Dictionary<string, int> DailyPomodoros { get; set; }
            = new Dictionary<string, int>();

        public void AddPomodoro()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            if (!DailyPomodoros.ContainsKey(today))
                DailyPomodoros[today] = 0;

            DailyPomodoros[today]++;
        }

        public static StatisticsModel Load()
        {
            string file = "statistics.json";

            if (!File.Exists(file))
                return new StatisticsModel();

            return JsonSerializer.Deserialize<StatisticsModel>(
                File.ReadAllText(file))
                ?? new StatisticsModel();
        }

        public void Save()
        {
            File.WriteAllText(
                "statistics.json",
                JsonSerializer.Serialize(this,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
        }
    }
}