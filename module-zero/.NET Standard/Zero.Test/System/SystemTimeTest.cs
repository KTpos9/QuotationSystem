using System;
using Xunit;
using Zero.System;

namespace Zero.Test.System
{
    public class SystemTimeTest
    {
        public SystemTimeTest()
        {
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void Today_ShouldReturnSameValueAsMockDateTime()
        {
            var mockDate = new DateTime(2017, 04, 15);
            SystemTime.SetDateTime(mockDate);
            AssertTimeDiffInMillisecond(mockDate, SystemTime.Today);
        }

        [Fact]
        public void Now_ShouldBeSameAsDateTimeNow()
        {
            AssertTimeDiffInMillisecond(DateTime.Now, SystemTime.Now);
        }

        [Fact]
        public void Now_ShouldReturnSmallDifferInMilliSecondFromMockDateTime()
        {
            var mockDate = new DateTime(2017, 04, 15, 9, 20, 30);
            SystemTime.SetDateTime(mockDate);
            AssertTimeDiffInMillisecond(mockDate, SystemTime.Now);
        }

        [Fact]
        public void UtcNow_ShouldBeSameAsDateTimeUtcNow()
        {
            AssertTimeDiffInMillisecond(DateTime.UtcNow, SystemTime.UtcNow);
        }

        private void AssertTimeDiffInMillisecond(DateTime t1, DateTime t2)
        {
            var diff = t1 - t2;
            Assert.True(Math.Abs(diff.TotalSeconds) < 0.01);
        }
    }
}