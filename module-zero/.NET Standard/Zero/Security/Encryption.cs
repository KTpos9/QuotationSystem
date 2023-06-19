using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Zero.Extension;

namespace Zero.Security
{
    /// <summary>
    /// To encrypt a value with key and salt
    /// value is UTF8, key and salt are ASCII
    /// </summary>
    public class Encryption
    {
        private readonly int keySize = 256;
        private readonly int blockSize = 128;
        private readonly int iterations = 1000;
        private readonly Encoding encoding = Encoding.UTF8;

        public Encryption()
        {
        }

        /// <summary>
        /// Specific Key Size, Block Size and Iterations
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="blockSize"></param>
        /// <param name="iterations">Number of Iterations will be affected to performance.</param>
        public Encryption(int keySize, int blockSize, int iterations)
        {
            this.keySize = keySize;
            this.blockSize = blockSize;
            this.iterations = iterations;
        }

        /// <summary>
        /// Serialize, compress and encrypt model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public string Encrypt<T>(T model, string key, string salt)
        {
            var json = JsonConvert.SerializeObject(model);
            return Encrypt(json, key, salt);
        }

        /// <summary>
        /// Decrypt, decompress, Deserialize to be Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="key"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public T Decrypt<T>(string field, string key, string salt)
        {
            var json = Decrypt(field, key, salt);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Encrypt(string text, string key, string salt)
        {
            byte[] textInBytes = encoding.GetBytes(text);
            byte[] passwordBytes = Encoding.ASCII.GetBytes(key);
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);

            byte[] compressed = textInBytes.Compress();
            byte[] bytesEncrypted = Encrypt(compressed, passwordBytes, saltBytes);
            return Convert.ToBase64String(bytesEncrypted);
        }

        public string Decrypt(string text, string key, string salt)
        {
            byte[] compressed = Convert.FromBase64String(text);
            byte[] passwordBytes = Encoding.ASCII.GetBytes(key);
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);

            byte[] bytesDecrypted = Decrypt(compressed, passwordBytes, saltBytes);
            byte[] decompressed = bytesDecrypted.Decompress();
            return encoding.GetString(decompressed);
        }

        public byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] password, byte[] salt)
        {
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                using (var algorithm = Aes.Create())
                {
                    algorithm.KeySize = keySize;
                    algorithm.BlockSize = blockSize;

                    var key = new Rfc2898DeriveBytes(password, salt, iterations);
                    algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
                    algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

                    algorithm.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    result = ms.ToArray();
                }
            }

            return result;
        }

        public byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] salt)
        {
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                using (var algorithm = Aes.Create())
                {
                    algorithm.KeySize = keySize;
                    algorithm.BlockSize = blockSize;

                    var key = new Rfc2898DeriveBytes(passwordBytes, salt, iterations);
                    algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
                    algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

                    algorithm.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    result = ms.ToArray();
                }
            }

            return result;
        }
    }
}