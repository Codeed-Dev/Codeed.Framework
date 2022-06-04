using System;

namespace Codeed.Framework.Commons.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTimeOffset datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, 1);
        }

        public static string ToRFC3339(this DateTimeOffset datetime)
        {
            return datetime.ToString("o");
        }

        public static string ToRFC3339(this DateTimeOffset? datetime)
        {
            if (datetime == null)
                return null;

            return datetime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssK");
        }

        public static DateTimeOffset Round(this DateTimeOffset datetime, TimeSpan interval)
        {
            return new DateTimeOffset((datetime.Ticks + interval.Ticks / 2 - 1) / interval.Ticks * interval.Ticks, datetime.Offset);
        }
    }
}
