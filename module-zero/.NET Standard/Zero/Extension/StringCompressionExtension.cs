using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Zero.Extension
{
    /// <summary>
    /// To compress string before sending between network such as socket programming (no need for http/https)
    /// </summary>
    public static class StringCompressionExtension
    {
        public static string Compress(this string input)
        {
            return Compress(input, Encoding.UTF8);
        }

        public static string Compress(this string input, Encoding encoding)
        {
            byte[] encoded = encoding.GetBytes(input);
            byte[] compressed = Compress(encoded);
            return Convert.ToBase64String(compressed);
        }

        public static string Decompress(this string input)
        {
            return Decompress(input, Encoding.UTF8);
        }

        public static string Decompress(this string input, Encoding encoding)
        {
            byte[] compressed = Convert.FromBase64String(input);
            byte[] decompressed = Decompress(compressed);
            return encoding.GetString(decompressed);
        }

        public static byte[] Decompress(this byte[] input)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(input), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public static byte[] Compress(this byte[] input)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(input, 0, input.Length);
                }
                return memory.ToArray();
            }
        }
    }
}