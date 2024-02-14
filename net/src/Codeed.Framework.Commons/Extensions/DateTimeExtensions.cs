using Codeed.Framework.Commons.Enums;

namespace System
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset FirstDayOfMonth(this DateTimeOffset datetime)
        {
            return new DateTimeOffset(datetime.Year, datetime.Month, 1, 0, 0, 0, datetime.Offset);
        }

        public static DateTime FirstDayOfMonth(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, 1, 0, 0, 0);
        }

        public static string ToRFC3339(this DateTimeOffset datetime)
        {
            return datetime.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
        }

        public static string ToRFC3339(this DateTimeOffset? datetime)
        {
            if (datetime is null)
            {
                return string.Empty;
            }

            return datetime.Value.ToRFC3339();
        }

        public static DateTimeOffset Round(this DateTimeOffset datetime, TimeSpan interval)
        {
            return new DateTimeOffset((datetime.Ticks + interval.Ticks / 2 - 1) / interval.Ticks * interval.Ticks, datetime.Offset);
        }

        public static DateTime Round(this DateTime datetime, TimeSpan interval)
        {
            return new DateTime((datetime.Ticks + interval.Ticks / 2 - 1) / interval.Ticks * interval.Ticks, datetime.Kind);
        }

        public static DateOnly ToDateOnly(this DateTimeOffset datetime)
        {
            return DateOnly.FromDateTime(datetime.Date);
        }

        public static DateOnly ToDateOnly(this DateTime datetime)
        {
            return DateOnly.FromDateTime(datetime);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek dayOfWeek)
        {
            var sunday = dt.StartOfWeek();
            return sunday.AddDays((int)dayOfWeek);
        }

        public static DateTimeOffset StartOfWeek(this DateTimeOffset dt, DayOfWeek dayOfWeek)
        {
            var sunday = dt.StartOfWeek();
            return sunday.AddDays((int)dayOfWeek);
        }

        public static DateTime StartOfWeek(this DateTime dt)
        {
            return dt.AddDays(-1 * (int)dt.DayOfWeek).Date;
        }

        public static DateTimeOffset StartOfWeek(this DateTimeOffset dt)
        {
            return dt.AddDays(-1 * (int)dt.DayOfWeek).StartOfDay();
        }

        public static DateTime EndOfWeek(this DateTime dt)
        {
            return dt.StartOfWeek().AddDays(7).AddMilliseconds(-1);
        }

        public static DateTimeOffset EndOfWeek(this DateTimeOffset dt)
        {
            return dt.StartOfWeek().AddDays(7).AddMilliseconds(-1);
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTimeOffset StartOfMonth(this DateTimeOffset dt)
        {
            return new DateTimeOffset(dt.Year, dt.Month, 1, 0, 0, 0, dt.Offset);
        }

        public static DateTime EndOfMonth(this DateTime dt)
        {
            return dt.StartOfMonth().AddMonths(1).AddMilliseconds(-1);
        }

        public static DateTimeOffset EndOfMonth(this DateTimeOffset dt)
        {
            return dt.StartOfMonth().AddMonths(1).AddMilliseconds(-1);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTimeOffset StartOfDay(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Offset);
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTimeOffset EndOfDay(this DateTimeOffset date)
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, 23, 59, 59, 999, date.Offset);
        }

        public static bool IsBusinessDay(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Sunday &&
                   date.DayOfWeek != DayOfWeek.Saturday;
        }

        public static bool IsBusinessDay(this DateTimeOffset date)
        {
            return date.DayOfWeek != DayOfWeek.Sunday &&
                   date.DayOfWeek != DayOfWeek.Saturday;
        }

        public static bool IsBusinessDay(this DateOnly date)
        {
            return date.DayOfWeek != DayOfWeek.Sunday &&
                   date.DayOfWeek != DayOfWeek.Saturday;
        }

        public static DateTime NextBusinessDay(this DateTime date)
        {
            var businessDay = date.AddDays(
                date.DayOfWeek == DayOfWeek.Sunday ? 1 :
                date.DayOfWeek == DayOfWeek.Saturday ? 2 : 0);

            return businessDay;
        }

        public static DateTimeOffset NextBusinessDay(this DateTimeOffset date)
        {
            var businessDay = date.AddDays(
                date.DayOfWeek == DayOfWeek.Sunday ? 1 :
                date.DayOfWeek == DayOfWeek.Saturday ? 2 : 0);

            return businessDay;
        }

        public static DateOnly NextBusinessDay(this DateOnly date)
        {
            return date.ToDateTime(TimeOnly.MinValue).NextBusinessDay().ToDateOnly();
        }

        public static (DateTime, DateTime) GetIntervalByUnitOfTime(this DateTime date, UnitOfTime unitOfTime)
        {
            switch (unitOfTime)
            {
                case UnitOfTime.Week:
                    return GetWeekInterval(date);
                case UnitOfTime.Month:
                    return GetMonthInterval(date);
                case UnitOfTime.Year:
                    return GetYearInterval(date);
                case UnitOfTime.Day:
                    return GetDayInterval(date);
                default:
                    throw new NotImplementedException();
            }
        }

        public static void ForeachDates(this (DateTime, DateTime) dates, UnitOfTime interval, Action<DateTime, DateTime> action)
        {
            var (startDate, endDate) = dates;
            for (DateTime dateTime = startDate.GetStartDate(UnitOfTime.Day); dateTime <= endDate; dateTime = NextDate(dateTime, interval))
            {
                action(dateTime, NextDate(dateTime, interval).StartOfDay().AddMilliseconds(-1));
            }
        }

        public static async Task ForeachDatesAsync(this (DateTime, DateTime) dates, UnitOfTime interval, Func<DateTime, DateTime, Task> action)
        {
            var (startDate, endDate) = dates;
            for (DateTime dateTime = startDate.GetStartDate(UnitOfTime.Day); dateTime <= endDate; dateTime = NextDate(dateTime, interval))
            {
                await action(dateTime, NextDate(dateTime, interval).StartOfDay().AddMilliseconds(-1));
            }
        }

        private static DateTime GetStartDate(this DateTime date, UnitOfTime interval)
        {
            var (firstDay, _) = date.GetIntervalByUnitOfTime(interval);
            return firstDay;
        }

        public static (DateTime, DateTime) GetYearInterval(this DateTime date)
        {
            var startDate = new DateTime(date.Year, 1, 1);
            var endDate = new DateTime(date.Year, 12, 31);
            return (startDate, endDate);
        }

        public static (DateTimeOffset, DateTimeOffset) GetYearInterval(this DateTimeOffset date)
        {
            var startDate = new DateTime(date.Year, 1, 1);
            var endDate = new DateTime(date.Year, 12, 31);
            return (startDate, endDate);
        }

        public static (DateTime, DateTime) GetMonthInterval(this DateTime date)
        {
            var startDate = date.StartOfMonth();
            var endDate = startDate.AddMonths(1).AddMilliseconds(-1);
            return (startDate, endDate);
        }

        public static (DateTimeOffset, DateTimeOffset) GetMonthInterval(this DateTimeOffset date)
        {
            var startDate = date.StartOfMonth();
            var endDate = startDate.AddMonths(1).AddMilliseconds(-1);
            return (startDate, endDate);
        }

        public static (DateTime, DateTime) GetWeekInterval(this DateTime date)
        {
            var startDate = date.StartOfWeek();
            var endDate = startDate.AddDays(6).EndOfDay();
            return (startDate, endDate);
        }

        public static (DateTimeOffset, DateTimeOffset) GetWeekInterval(this DateTimeOffset date)
        {
            var startDate = date.StartOfWeek();
            var endDate = startDate.AddDays(6).EndOfDay();
            return (startDate, endDate);
        }

        public static (DateTime, DateTime) GetDayInterval(this DateTime date)
        {
            var startDate = date.StartOfDay();
            var endDate = date.EndOfDay();
            return (startDate, endDate);
        }

        public static (DateTimeOffset, DateTimeOffset) GetDayInterval(this DateTimeOffset date)
        {
            var startDate = date.StartOfDay();
            var endDate = date.EndOfDay();
            return (startDate, endDate);
        }

        public static DateTime NextDate(this DateTime dateTime, UnitOfTime unitOfTime)
        {
            switch (unitOfTime)
            {
                case UnitOfTime.Week:
                    return dateTime.AddDays(7);
                case UnitOfTime.Month:
                    return dateTime.AddMonths(1);
                case UnitOfTime.Year:
                    return dateTime.AddYears(1);
                case UnitOfTime.Day:
                    return dateTime.AddDays(1);
                default:
                    throw new NotImplementedException();
            }
        }

        public static DateTimeOffset NextDate(this DateTimeOffset dateTime, UnitOfTime unitOfTime)
        {
            switch (unitOfTime)
            {
                case UnitOfTime.Week:
                    return dateTime.AddDays(7);
                case UnitOfTime.Month:
                    return dateTime.AddMonths(1);
                case UnitOfTime.Year:
                    return dateTime.AddYears(1);
                case UnitOfTime.Day:
                    return dateTime.AddDays(1);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
