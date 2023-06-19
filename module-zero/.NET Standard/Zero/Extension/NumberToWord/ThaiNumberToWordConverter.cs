using System;
using Humanizer;
using Humanizer.Localisation.NumberToWords;
using System.Collections.Generic;

namespace Zero.Extension.NumberToWord
{
    public static class ThaiNumberToWordsExtension
    {
        public static string ToThaiWords(this long number)
        {
            return new ThaiNumberToWordsConverter().Convert(number);
        }

        public static string ToThaiWords(this int number)
        {
            return new ThaiNumberToWordsConverter().Convert(number);
        }

        public static string ToThaiBaht(this decimal number)
        {
            var converter = new ThaiNumberToWordsConverter();
            var amountWord = converter.Convert((long)number);

            var dec = Math.Abs(number) % 1;
            if (dec != 0)
            {
                var decimalWord = converter.Convert((long)(Math.Round(dec, 2) * 100));
                return amountWord + "บาท " + decimalWord + "สตางค์";
            }

            return amountWord + "บาทถ้วน";
        }
    }

    public class ThaiNumberToWordsConverter : INumberToWordsConverter
    {
        private static readonly string[] UnitsMap = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };

        public string Convert(long number)
        {
            return Convert(number, false);
        }

        public string ConvertToOrdinal(int number)
        {
            return Convert(number, true);
        }

        public string Convert(long number, GrammaticalGender gender)
        {
            return Convert(number);
        }

        public string ConvertToOrdinal(int number, GrammaticalGender gender)
        {
            return ConvertToOrdinal(number);
        }

        private string Convert(long number, bool isOrdinal)
        {
            if (isOrdinal)
            {
                return $"ที่{Convert(number)}";
            }

            if (number < 0)
            {
                return $"ลบ{Convert(-number)}";
            }

            var parts = new List<string>();

            if (number >= 100)
            {
                if ((number / 1000000) > 0)
                {
                    parts.Add($"{Convert(number / 1000000)}ล้าน");
                    number %= 1000000;
                }

                if ((number / 100000) > 0)
                {
                    parts.Add($"{Convert(number / 100000)}แสน");
                    number %= 100000;
                }

                if ((number / 10000) > 0)
                {
                    parts.Add($"{Convert(number / 10000)}หมื่น");
                    number %= 10000;
                }

                if ((number / 1000) > 0)
                {
                    parts.Add($"{Convert(number / 1000)}พัน");
                    number %= 1000;
                }

                if ((number / 100) > 0)
                {
                    parts.Add($"{Convert(number / 100)}ร้อย");
                    number %= 100;
                }

                if (number == 0)
                {
                    return JoinWords(parts);
                }
            }

            parts.Add(GetUnitValue(number));

            return JoinWords(parts);
        }

        private string JoinWords(List<string> parts)
        {
            var toWords = string.Join("", parts.ToArray());

            return toWords;
        }

        private string GetUnitValue(long number)
        {
            if (number > 10)
            {
                var tenUnit = number / 10;
                var unit = number % 10;

                var ten = UnitsMap[tenUnit];
                if (tenUnit == 2)
                {
                    ten = "ยี่";
                }
                if (tenUnit == 1)
                {
                    ten = "";
                }

                if (unit == 1)
                {
                    return $"{ten}สิบเอ็ด";
                }
                if (unit == 0)
                {
                    return $"{ten}สิบ";
                }
                return $"{ten}สิบ{UnitsMap[unit]}";
            }

            return UnitsMap[number];
        }
    }
}