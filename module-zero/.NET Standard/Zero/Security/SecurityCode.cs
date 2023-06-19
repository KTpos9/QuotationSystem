using System.Security.Cryptography;
using System.Text;

namespace Zero.Security
{
    /// <summary>
    /// ใช้สร้างชุดเลขแบบสุ่ม เช่น code สำหรับ reset password, default password
    /// </summary>
    public class SecurityCode
    {
        private readonly RNGCryptoServiceProvider randomer;
        private readonly char[] characters;

        public SecurityCode()
            : this("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890")
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="characters">ชุดตัวอักษรที่ใช้สุ่ม</param>
        public SecurityCode(string characters)
        {
            randomer = new RNGCryptoServiceProvider();
            this.characters = characters.ToCharArray();
        }

        public string Generate(int length)
        {
            byte[] data = new byte[length];
            randomer.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(length);
            foreach (byte b in data)
            {
                result.Append(characters[b % characters.Length]);
            }
            return result.ToString();
        }
    }
}