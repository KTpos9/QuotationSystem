using System;

namespace Zero.Extension
{
    public static class DateTimeExtension
    {
        public static DateTime TruncateToYearStart(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        public static DateTime TruncateToMonthStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime TruncateToDayStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        public static DateTime TruncateToHourStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        }

        public static DateTime TruncateToMinuteStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        }

        public static DateTime TruncateToSecondStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        public static bool IsBetween(this DateTime value, DateTime start, DateTime end)
        {
            return value >= start && value <= end;
        }

        public static bool IsOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return start1 <= end2 && end1 >= start2;
        }

        public static bool IsWeekend(this DateTime value)
        {
            return value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday;
        }

        public static string ToCalendarDateFormat(this DateTime value)
        {
            return $"{value:dddd, MMMM dd, yyyy}";
        }

        public static string ToPrettyDate(this DateTime value)
        {
            return ToPrettyDate(value, DateTime.Now);
        }

        public static string ToPrettyDate(this DateTime value, DateTime now)
        {
            var elapsed = now.Subtract(value);
            var dayDiff = (int)elapsed.TotalDays;
            var secDiff = (int)elapsed.TotalSeconds;

            // out of range
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return ToCalendarDateFormat(value);
            }

            // same date
            if (dayDiff == 0)
            {
                if (secDiff < 60)
                {
                    return "Just now";
                }
                if (secDiff < 120)
                {
                    return "1 minute ago";
                }
                if (secDiff < 3600)
                {
                    return $"{Math.Floor((double)secDiff / 60)} minutes ago";
                }
                if (secDiff < 7200)
                {
                    return "1 hour ago";
                }
                if (secDiff < 86400)
                {
                    return $"{Math.Floor((double)secDiff / 3600)} hours ago";
                }
            }
            // previous days
            if (dayDiff == 1)
            {
                return "Yesterday";
            }
            if (dayDiff < 7)
            {
                return $"{dayDiff} days ago";
            }
            return $"{Math.Ceiling((double)dayDiff / 7)} weeks ago";
        }

        public static string ToPrettyPeriod(this DateTime startDate, DateTime endDate)
        {
            if (startDate.Year == endDate.Year && startDate.Month == endDate.Month && startDate.Day == endDate.Day)
            {
                return $"{startDate:MMMM d, yyyy}";
            }
            if (startDate.Year == endDate.Year && startDate.Month == endDate.Month)
            {
                return $"{startDate:MMMM d} - {endDate:d, yyyy}";
            }
            if (startDate.Year == endDate.Year)
            {
                return $"{startDate:MMMM d} - {endDate:MMMM d, yyyy}";
            }

            return $"{startDate:MMMM d, yyyy} - {endDate:MMMM d, yyyy}";
        }
    }
}