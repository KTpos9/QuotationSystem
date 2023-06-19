using System;
using System.Security.Cryptography;
using System.Text;

namespace Zero.Security
{
    /// <summary>
    /// ใช้เข้ารหัส password
    /// </summary>
    public static class PasswordEncryption
    {
        public static string Encrypt(string text, string key)
        {
            // Get the bytes of the string
            byte[] textToBeEncrypted = Encoding.UTF8.GetBytes(text);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(key);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = Aes256.DefaultInstance.Encrypt(textToBeEncrypted, passwordBytes);
            return Convert.ToBase64String(bytesEncrypted);
        }

        public static string Decrypt(string text, string key)
        {
            // Get the bytes of the string
            byte[] textToBeDecrypted = Convert.FromBase64String(text);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(key);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = Aes256.DefaultInstance.Decrypt(textToBeDecrypted, passwordBytes);
            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        /// <summary>
        /// สร้าง Hash password ที่ไม่สามารถถอดค่ากลับได้
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Hash(string password, string salt)
        {
            // Get the bytes of the string
            byte[] passwordToBeHashed = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            passwordToBeHashed = SHA256.Create().ComputeHash(passwordToBeHashed);

            byte[] bytesEncrypted = Aes256.DefaultInstance.Encrypt(passwordToBeHashed, saltBytes);
            return Convert.ToBase64String(bytesEncrypted);
        }
    }
}