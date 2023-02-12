using Codeed.Framework.Commons.Enums;
using System;
using System.Threading.Tasks;

namespace System
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
            {
                return null;
            }

            return datetime.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssK");
        }

        public static DateTimeOffset Round(this DateTimeOffset datetime, TimeSpan interval)
        {
            return new DateTimeOffset((datetime.Ticks + interval.Ticks / 2 - 1) / interval.Ticks * interval.Ticks, datetime.Offset);
        }

        public static DateOnly ToDateOnly(this DateTimeOffset datetime)
        {
            return DateOnly.FromDateTime(datetime.Date);
        }

        public static DateOnly ToDateOnly(this DateTime datetime)
        {
            return DateOnly.FromDateTime(datetime);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var sunday = dt.StartOfWeek();
            return sunday.AddDays((int)startOfWeek).Date;
        }

        public static DateTime StartOfWeek(this DateTime dt)
        {
            return dt.AddDays(-1 * (int)dt.DayOfWeek).Date;
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static bool IsBusinessDay(this DateTime date)
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

        public static (DateTime, DateTime) GetMonthInterval(this DateTime date)
        {
            var startDate = new DateTime(date.Year, date.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            return (startDate, endDate);
        }

        public static (DateTime, DateTime) GetWeekInterval(this DateTime date)
        {
            var startDate = date.StartOfWeek();
            var endDate = date.AddDays(6);
            return (startDate, endDate);
        }

        public static (DateTime, DateTime) GetDayInterval(this DateTime date)
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
    }
}
