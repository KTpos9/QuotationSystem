using System;

namespace Zero.System
{
    /// <summary>
    /// ใช้ดึงค่าเวลาแทน class DateTime เพื่อให้สามารถกำหนดเวลาระบบเป็นเวลาใดๆ ช่วยให้สะดวกต่อการทดสอบระบบ
    /// </summary>
    public static class SystemTime
    {
        public static DateTime Now => GetNow();
        public static DateTime Today => GetNow().Date;
        public static DateTime UtcNow => GetUtcNow();

        internal static TimeSpan DateTimeOffset { get; set; } = TimeSpan.Zero;
        internal static readonly Func<DateTime> GetNow = () => DateTime.Now + DateTimeOffset;
        internal static readonly Func<DateTime> GetUtcNow = () => DateTime.UtcNow + DateTimeOffset;

        /// <summary>
        /// This method should be called when application started for setting specific system date time.
        /// </summary>
        /// <param name="specificDateTime"></param>
        public static void SetDateTime(DateTime specificDateTime)
        {
            DateTimeOffset = specificDateTime - DateTime.Now;
        }

        /// <summary>
        /// This method should be called in Unit Test TearDown.
        /// </summary>
        public static void ResetDateTime()
        {
            DateTimeOffset = TimeSpan.Zero;
        }
    }
}