using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using Zero.Core.Mvc.Models.DataTables;

namespace Zero.Core.Mvc.Extensions
{
    public static class SessionExtension
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static T GetCriteria<T>(this ISession session, string key, Func<T> createDefaultObject) where T : class, new()
        {
            if (session.GetString(key) != null)
            {
                return JsonConvert.DeserializeObject<T>(session.GetString(key));
            }

            return createDefaultObject();
        }

        public static void SaveCriteria<T>(this ISession session, string key, T criteria) where T : class, new()
        {
            session.SetString(key, criteria != null ? JsonConvert.SerializeObject(criteria) : null);
        }

        public static DataTableOptionModel GetDataTableOption(this ISession session, string key)
        {
            if (session.GetString(key) != null)
            {
                return JsonConvert.DeserializeObject<DataTableOptionModel>(session.GetString(key + ".DataTableOption"));
            }

            return new DataTableOptionModel();
        }

        public static void SaveDataTableOption(this ISession session, string key, DataTableOptionModel option)
        {
            session.SetString(key + ".DataTableOption", option != null ? JsonConvert.SerializeObject(option) : null);
        }
    }
}