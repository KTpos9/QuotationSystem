using System;
using System.Security.Cryptography;

namespace Zero.Security
{
    public class PasswordHashing
    {
        private const int SaltByteSize = 24;
        private const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash
        private const int SaltIndex = 0;
        private const int Pbkdf2Index = 1;

        private readonly int pbkdf2Iterations = 1000;

        public PasswordHashing()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pbkdf2Iterations">A high number of iterations will slow the algorithm down, which makes password cracking a lot harder. A high number of iterations is therefor recommended.</param>
        public PasswordHashing(int pbkdf2Iterations)
        {
            this.pbkdf2Iterations = pbkdf2Iterations;
        }

        public string HashPassword(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, pbkdf2Iterations, HashByteSize);
            return Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        public bool ValidatePassword(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            var split = correctHash.Split(delimiter);
            var salt = Convert.FromBase64String(split[SaltIndex]);
            var hash = Convert.FromBase64String(split[Pbkdf2Index]);

            var testHash = GetPbkdf2Bytes(password, salt, pbkdf2Iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        private byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}