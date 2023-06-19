using System.Security.Cryptography;
using System.Text;
using Zero.Extension;
using Zero.Validation;

namespace Zero.Security
{
    public class Sha256
    {
        public string ComputeHash(string value)
        {
            ThrowIf.Null(value);
            using (var hashing = SHA256.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(value);
                var hashBytes = hashing.ComputeHash(inputBytes);

                return hashBytes.ToHex();
            }
        }
    }
}