using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Zero.Extension
{
    public static class CsvExtension
    {
        public static StringBuilder ToCsvContent<T>(this List<T> records) where T : class, new()
        {
            StringBuilder content = new StringBuilder();

            var propertyInfos = GetPropertyInfos<T>();
            content.WriteCsvHeader(propertyInfos);
            content.WriteCsvContent(propertyInfos, records);
            return content;
        }

        private static PropertyInfo[] GetPropertyInfos<T>() where T : class, new()
        {
            T obj = new();
            return obj.GetType().GetProperties();
        }

        private static StringBuilder WriteCsvHeader(this StringBuilder content, PropertyInfo[] propertyInfos)
        {
            content.AppendLine(string.Join(',', propertyInfos.Select(p => p.Name)));
            return content;
        }

        private static StringBuilder WriteCsvContent<T>(this StringBuilder content, PropertyInfo[] propertyInfos, List<T> records) where T : class, new()
        {
            foreach (var record in records)
            {
                content.Append(record.GetRowContent(propertyInfos));
                content.AppendLine();
            }
            return content;
        }

        private static string GetRowContent<T>(this T record, PropertyInfo[] propertyInfos) where T : class, new()
        {
            return string.Join(",", propertyInfos.Select(p => GetValue(record, p)));
        }

        private static string GetValue<T>(T record, PropertyInfo propertyInfo) where T : class, new()
        {
            var obj = propertyInfo.GetValue(record);
            if (obj == null || DBNull.Value.Equals(obj))
                return string.Empty;

            var type = propertyInfo.PropertyType;
            if (type == typeof(string))
                return "\"" + ((string)obj).Replace("\"", "\"\"") + "\"";
            if (type == typeof(bool))
                return ((bool)obj) == true ? "true" : "false";
            if (type == typeof(DateTime))
            {
                var date = (DateTime)obj;
                if (date.TimeOfDay == TimeSpan.Zero)
                    return date.ToString("yyyy-MM-dd");
                return date.ToString("yyyy-MM-dd HH:mm");
            }

            return obj.ToString();
        }
    }
}