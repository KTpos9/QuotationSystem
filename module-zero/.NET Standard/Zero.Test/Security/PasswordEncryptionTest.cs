using System.Linq;
using Xunit;
using Zero.Security;

namespace Zero.Test.Security
{
    public class PasswordEncryptionTest
    {
        private readonly string[] messages = { "", "a", "abcdefg", "123456", "Hello World", "1234567890987654321", "This is test message." };
        private readonly string[] passwords = { "password", "p@ssw0rd", "q1w2e3r4t5y6" };

        [Fact]
        public void Encrypt_ShouldReturnEncryptedValue()
        {
            var encrypted = PasswordEncryption.Encrypt(messages[0], passwords[0]);
            Assert.NotEqual(messages[0], encrypted);
        }

        [Fact]
        public void Decrypt_ShouldBeSameValue_AfterEncryptAndDecrypt()
        {
            var password = passwords[0];
            for (int i = 0; i < messages.Length; i++)
            {
                var encrypted = PasswordEncryption.Encrypt(messages[i], password);
                var decrypted = PasswordEncryption.Decrypt(encrypted, password);
                Assert.Equal(decrypted, messages[i]);
            }
        }

        [Fact]
        public void Decrypt_ShouldBeSameValue_WhenPasswordIsVariance()
        {
            var message = messages.Last();
            for (int i = 0; i < passwords.Length; i++)
            {
                var encrypted = PasswordEncryption.Encrypt(message, passwords[i]);
                var decrypted = PasswordEncryption.Decrypt(encrypted, passwords[i]);
                Assert.Equal(decrypted, message);
            }
        }

        [Fact]
        public void Hash_ShouldChangeValue()
        {
            var message = messages.Last();
            var result = PasswordEncryption.Hash(message, passwords.Last());
            Assert.NotEqual(message, result);
        }

        [Fact]
        public void Hash_ShouldReturnSameValue_For2Times()
        {
            var message = messages.Last();
            var result = PasswordEncryption.Hash(message, passwords.Last());
            Assert.Equal(result, PasswordEncryption.Hash(message, passwords.Last()));
        }
    }
}