using System.Collections.Generic;
using Xunit;
using Zero.Security;

namespace Zero.Test.Security
{
    public class SecurityCodeTest
    {
        [Fact]
        public void GenerateCode_ShouldReturnValidLength()
        {
            const int length = 12;
            string code = new SecurityCode().Generate(length);

            Assert.Equal(length, code.Length);
        }

        [Fact]
        public void GenerateCode_ShouldReturnValidCharacterSet()
        {
            const int length = 20;
            const string characterSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code = new SecurityCode(characterSet).Generate(length);

            Assert.Matches("^[a-zA-Z0-9]{20}$", code);
        }

        [Fact]
        public void GenerateCode_ShouldReturnUniqueValues()
        {
            int round = 100;
            int length = 4;
            var generator = new SecurityCode("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            var uniqueKeys = new List<string>();
            for (int i = 0; i < round; i++)
            {
                var result = generator.Generate(length);
                Assert.DoesNotContain(result, uniqueKeys);

                uniqueKeys.Add(result);
            }
        }
    }
}