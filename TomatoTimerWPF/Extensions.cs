using System;
using System.Windows;

namespace TomatoTimerWPF
{
    static class Extensions
    {
        public static Visibility ToVisibility(this bool visible)
        {
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public static string ToFormat(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static TimeSpan Milliseconds(this int ms)
        {
            return new TimeSpan(0, 0, 0, 0, ms);
        }

        public static TimeSpan Seconds(this int seconds)
        {
            return new TimeSpan(0, 0, seconds);
        }

        public static TimeSpan Minutes(this int minutes)
        {
            return new TimeSpan(0, minutes, 0);
        }

        public static bool IsNegativeOrZero(this TimeSpan span)
        {
            return span.TotalMilliseconds <= 0;
        }
    }
}
