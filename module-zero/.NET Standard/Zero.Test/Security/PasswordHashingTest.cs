using Xunit;
using Zero.Security;

namespace Zero.Test.Security
{
    public class PasswordHashingTest
    {
        [Fact]
        public void HashPassword_ShouldChangeValue()
        {
            PasswordHashing hashing = new PasswordHashing();
            var password = "some password";
            var hash = hashing.HashPassword(password);
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void HashPassword_HighIteration_ShouldBeValid()
        {
            PasswordHashing hashing = new PasswordHashing(100000);
            var password = "some password";
            var hash = hashing.HashPassword(password);
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void ValidatePassword_ShouldBeValid()
        {
            PasswordHashing hashing = new PasswordHashing();
            var password = "some password";
            var hash = hashing.HashPassword(password);

            var result = hashing.ValidatePassword(password, hash);
            Assert.True(result);
        }

        [Fact]
        public void ValidatePassword_WrongPassword_ShouldNotBeValid()
        {
            PasswordHashing hashing = new PasswordHashing();
            var password = "some password";
            var hash = hashing.HashPassword(password);

            var result = hashing.ValidatePassword("wrong password", hash);
            Assert.False(result);
        }
    }
}