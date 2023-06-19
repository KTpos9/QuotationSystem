using Xunit;
using Zero.Security;

namespace Zero.Test.Security
{
    public class Aes256Test
    {
        [Fact]
        public void Encrypt_ShouldReturnTheSameValue_WhenRoundedTripEncryption()
        {
            var key = "test-key";
            var data = "12345678";

            var encrypted = Aes256.DefaultInstance.Encrypt(data, key);
            var decrypted = Aes256.DefaultInstance.Decrypt(encrypted, key);

            Assert.Equal(decrypted, data);
        }
    }
}