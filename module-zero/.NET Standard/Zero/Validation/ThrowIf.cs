using System;
using System.Collections.Generic;
using System.IO;

namespace Zero.Validation
{
    public static class ThrowIf
    {
        public static void Null<T>(T model) where T : class
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
        }

        public static void Null<T>(IEnumerable<T> model) where T : class
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
        }

        public static void Null<T>(T model, string fieldName)
        {
            if (model == null)
            {
                string message = $"Instance of {fieldName} is null.";
                throw new ArgumentException(message);
            }
        }

        public static void NullOrEmpty(string model)
        {
            if (string.IsNullOrEmpty(model))
            {
                throw new ArgumentException(nameof(model));
            }
        }

        public static void NullOrEmpty(string model, string fieldName)
        {
            if (string.IsNullOrEmpty(model))
            {
                string message = $"{fieldName} is null or empty";
                throw new ArgumentException(message);
            }
        }

        public static void NullOrWhiteSpace(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
        }

        public static void NullOrWhiteSpace(string model, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                string message = $"{fieldName} is null or white space";
                throw new ArgumentException(message);
            }
        }

        public static void NotExist(DirectoryInfo directory)
        {
            if (directory.Exists == false)
            {
                string message = $"Can't find directory {directory.FullName}";
                throw new DirectoryNotFoundException(message);
            }
        }

        public static void NotExist(FileInfo file)
        {
            if (file.Exists == false)
            {
                string message = $"Can't find file {file.FullName}";
                throw new FileNotFoundException(message);
            }
        }

        public static void NotExistDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath) == false)
            {
                string message = $"Can't find directory {directoryPath}";
                throw new DirectoryNotFoundException(message);
            }
        }

        public static void NotExist(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                string message = $"Can't find file {filePath}";
                throw new FileNotFoundException(message);
            }
        }
    }
}