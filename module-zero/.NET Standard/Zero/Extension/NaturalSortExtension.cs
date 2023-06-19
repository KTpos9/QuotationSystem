using System.Text.RegularExpressions;

namespace Zero.Extension
{
    public static class NaturalSortExtension
    {
        private static readonly Regex NumericRegex = new Regex("([0-9]+)", RegexOptions.Compiled);

        public static string ToNaturalSortString(this string original, int paddingDigits = 6)
        {
            if (string.IsNullOrWhiteSpace(original)) return string.Empty;

            // pad all groups of numbers with zeros and combine back into a sort-friendly string
            // VE-1A 42100 => VE-000001A 042100
            // 15 => 000015
            return NumericRegex.Replace(original, match => match.Value.PadLeft(paddingDigits, '0'));
        }
    }
}