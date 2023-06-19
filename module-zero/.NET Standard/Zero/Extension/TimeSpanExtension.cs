using System;

namespace Zero.Extension
{
    public static class TimeSpanExtension
    {
        public static TimeSpan TimeOnly(this TimeSpan timeSpan)
        {
            return timeSpan.Days == 0 ? timeSpan : timeSpan.Subtract(new TimeSpan(timeSpan.Days, 0, 0, 0, 0));
        }

        public static TimeSpan? TimeOnly(this TimeSpan? timeSpan)
        {
            return timeSpan == null ? (TimeSpan?)null : TimeOnly(timeSpan.Value);
        }

        public static TimeSpan AddDateOffset(this TimeSpan timeSpan, bool dateOffset)
        {
            return dateOffset ? timeSpan.Add(new TimeSpan(1, 0, 0, 0)) : timeSpan;
        }

        public static TimeSpan? AddDateOffset(this TimeSpan? timeSpan, bool dateOffset)
        {
            return timeSpan != null && dateOffset ? timeSpan.Value.Add(new TimeSpan(1, 0, 0, 0)) : timeSpan;
        }
    }
}