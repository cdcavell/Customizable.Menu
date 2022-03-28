using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    public static class StringExtensions
    {
        public static bool IsValidEmail(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            try
            {
                // Normalize the domain
                value = Regex.Replace(value, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                static string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(value,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string Clean(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            string cleanString = HttpUtility.HtmlEncode(value.Replace("\r", string.Empty).Replace("\n", string.Empty));
            return string.Format("{0}", cleanString);
        }

        public static bool IsValidGuid(this string value)
        {
            Regex isGuid = new(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
            bool isValid = false;

            if (!string.IsNullOrEmpty(value))
                if (isGuid.IsMatch(value))
                    isValid = true;

            return isValid;
        }

        public static string CleanJsonResult(this string value)
        {
            return value.Replace("\\", string.Empty).Trim(new char[1] { '"' });
        }


        public static string TrimEnd(this string input, string suffixToRemove, StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            if (suffixToRemove != null && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input[..^suffixToRemove.Length];
            }

            return input;
        }


        public static string TrimFromFirstChar(this string input, char startTrim)
        {
            int index = input.IndexOf(startTrim);
            if (index > 0)
                input = input[..index];

            return input;
        }

        public static string UTF8(this string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
