using System;
using Xunit;
using Zero.Security;

namespace Zero.Test.Security
{
    public class HashingTest
    {
        [Fact]
        public void ToMd5_ShouldChangeValue()
        {
            Hashing hashing = new Hashing();
            var message = "This is test message";
            var result = hashing.ToMd5(message);
            Assert.NotEqual(message, result);
        }

        [Fact]
        public void ToMd5_ShouldThrow_WhenNull()
        {
            Hashing hashing = new Hashing();
            string message = null;
            Assert.Throws<ArgumentNullException>(() => hashing.ToMd5(message));
        }

        [Fact]
        public void ToSha256_ShouldChangeValue()
        {
            Hashing hashing = new Hashing();
            var message = "This is test message";
            var result = hashing.ToSha256(message);
            Assert.NotEqual(message, result);
        }

        [Fact]
        public void ToSha256_ShouldThrow_WhenNull()
        {
            Hashing hashing = new Hashing();
            string message = null;
            Assert.Throws<ArgumentNullException>(() => hashing.ToSha256(message));
        }
    }
}