using System;
using System.Text;

namespace Delights.Modules.Notes
{
    public static class Utils
    {
        public static string CountWordsString(string str)
        {
            int len = str.Length;
            if (len < 1000)
            {
                return len.ToString();
            }
            else
            {
                return $"{(len / 1000.0).ToString("f1")}k";
            }
        }

        public static string ReadTimeString(string str)
        {
            const int WordPerMinute = 500;
            var span = TimeSpan.FromMinutes(str.Length / (double)WordPerMinute);

            int time = (int)span.TotalMinutes;
            if (time <= 1)
            {
                return "1 minute";
            }
            else
            {
                return $"{time} minutes";
            }
        }
    }
}
