using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using Zero.Security;

namespace Zero.Core.Mvc.Extensions
{
    public static class CookieExtension
    {
        private const string DefaultEncryptionKey = "249fcb9d9f5f289906310ed537581cd8";

        public static T GetCookie<T>(this HttpRequest request, string key)
        {
            return GetCookie<T>(request, key, DefaultEncryptionKey);
        }

        private static T GetCookie<T>(HttpRequest request, string key, string encryptionKey)
        {
            string cookie = request.Cookies[key];
            if (cookie == null)
            {
                return default(T);
            }
            string json = Aes256.DefaultInstance.Decrypt(cookie, encryptionKey);
            var value = JsonConvert.DeserializeObject<T>(json);
            return value;
        }

        public static void SetCookie<T>(this HttpResponse response, string key, T value, DateTime expireTime)
        {
            SetCookie(response, key, value, expireTime, DefaultEncryptionKey);
        }

        private static void SetCookie<T>(HttpResponse response, string key, T value, DateTime expireTime, string encryptionKey)
        {
            CookieOptions option = new CookieOptions { Expires = expireTime };
            var json = JsonConvert.SerializeObject(value);
            var cookieValue = Aes256.DefaultInstance.Encrypt(json, encryptionKey);
            response.Cookies.Append(key, cookieValue, option);
        }

        public static void ClearCookie(this HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }

        public static T GetCookie<T>(this HttpContext context, string key)
        {
            return GetCookie<T>(context.Request, key);
        }

        public static T GetCookie<T>(this HttpContext context, string key, string encryptionKey)
        {
            return GetCookie<T>(context.Request, key, encryptionKey);
        }

        public static void SetCookie<T>(this HttpContext context, string key, T value, DateTime expireTime)
        {
            SetCookie(context.Response, key, value, expireTime);
        }

        public static void SetCookie<T>(this HttpContext context, string key, T value, DateTime expireTime, string encryptionKey)
        {
            SetCookie(context.Response, key, value, expireTime, encryptionKey);
        }

        public static void ClearCookie(this HttpContext context, string key)
        {
            ClearCookie(context.Response, key);
        }
    }
}