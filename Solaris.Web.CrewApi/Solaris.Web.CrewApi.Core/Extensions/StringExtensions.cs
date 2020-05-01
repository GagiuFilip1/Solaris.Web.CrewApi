using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Solaris.Web.CrewApi.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToMySqlLikeSyntax(this string value)
        {
            return $"%{value}%";
        }

        public static bool IsValidEmailFormat(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                value = Regex.Replace(value, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                return Regex.IsMatch(value,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool HasOnlyLetters(this string value)
        {
            return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^[a-zA-Z]+$");
        }

        public static bool HasOnlyNumbers(this string value)
        {
            return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^[0-9]+$");
        }

        public static bool HasOnlyNumbersAndLetters(this string value)
        {
            return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^[a-zA-Z0-9]+$");
        }

        private static string DomainMapper(Match match)
        {
            var idn = new IdnMapping();

            var domainName = idn.GetAscii(match.Groups[2].Value);

            return match.Groups[1].Value + domainName;
        }
    }
}