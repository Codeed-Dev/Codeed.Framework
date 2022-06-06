using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string value) where T : struct
        {
            if (Enum.TryParse(value, out T result))
                return result;
            else
                return default;
        }

        public static bool IsValidEnum<T>(this string value) where T : struct
        {
            return Enum.TryParse(value, out T result);
        }

        public static string ToSHA256(this string str)
        {
            var inputBytes = Encoding.UTF8.GetBytes(str);
            var sha256Provider = SHA256.Create();
            var hashedBytes = sha256Provider.ComputeHash(inputBytes);
            return Encoding.UTF8.GetString(hashedBytes);
        }

        public static IEnumerable<string> SplitBy(this string str, char split)
        {
            if (string.IsNullOrEmpty(str))
                return Enumerable.Empty<string>();

            return str.Split(split).Where(s => !string.IsNullOrEmpty(s));
        }

        public static IEnumerable<string> SplitByCommas(this string str)
        {
            return str.SplitBy(',');
        }

        public static string TrimLeftWords(this string str, Func<string, bool> condition)
        {
            var words = str.Split(new char[] { ' ', '\n' });
            var firstWord = words.FirstOrDefault(condition);
            var startIndex = Array.IndexOf(words, firstWord);
            var newWords = words.Skip(startIndex);
            return string.Join(" ", newWords);
        }
    }
}
