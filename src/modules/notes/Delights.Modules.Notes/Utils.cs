using System;
using System.Text;

namespace Delights.Modules.Notes
{
    public static class Utils
    {
        public static string ToFriendlyString(this DateTimeOffset value)
        {
            TimeSpan tspan = DateTimeOffset.Now - value;
            StringBuilder sb = new StringBuilder();
            if (tspan.TotalDays > 60)
            {
                sb.Append(value.ToString("F"));
            }
            else if (tspan.TotalDays > 30)
            {
                sb.Append("1 month ago");
            }
            else if (tspan.TotalDays > 14)
            {
                sb.Append("2 weeks ago");
            }
            else if (tspan.TotalDays > 7)
            {
                sb.Append("1 week ago");
            }
            else if (tspan.TotalDays > 1)
            {
                sb.Append($"{(int)Math.Floor(tspan.TotalDays)} days ago");
            }
            else if (tspan.TotalHours > 1)
            {
                sb.Append($"{(int)Math.Floor(tspan.TotalHours)} hours ago");
            }
            else if (tspan.TotalMinutes > 1)
            {
                sb.Append($"{(int)Math.Floor(tspan.TotalMinutes)} minutes ago");
            }
            else if (tspan.TotalSeconds > 1)
            {
                sb.Append($"{(int)Math.Floor(tspan.TotalSeconds)} seconds ago");
            }
            else
            {
                sb.Append("just");
            }
            return sb.ToString();
        }

        public static string ToFriendlyString(this TimeSpan value)
        {
            StringBuilder sb = new StringBuilder();
            bool haspre = false;
            if (value.Days > 0)
            {
                sb.Append(string.Format("{0} d", value.Days));
                haspre = true;
            }
            if (value.Hours > 0)
            {
                if (haspre) sb.Append(' ');
                sb.Append(string.Format("{0} h", value.Hours));
                haspre = true;
            }
            if (value.Minutes > 0)
            {
                if (haspre) sb.Append(' ');
                sb.Append(string.Format("{0} min", value.Minutes));
                haspre = true;
            }
            if (value.Seconds > 0)
            {
                if (haspre) sb.Append(' ');
                sb.Append(string.Format("{0} s", value.Seconds));
                haspre = true;
            }
            if (value.Milliseconds > 0)
            {
                if (haspre) sb.Append(' ');
                sb.Append(string.Format("{0} ms", value.Milliseconds));
            }
            else
            {
                if (!haspre) sb.Append(string.Format("{0} ms", value.Milliseconds));
            }
            return sb.ToString();
        }

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
