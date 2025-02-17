using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OOFScheduling
{
    internal static class Extensions
    {
        internal static string CleanReplyMessage(this string input)
        {
            return Regex.Replace(input, @"\r\n|\n\r|\n|\r", "\r\n");
        }

        private static string RemoveHTMLRegex(this string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
        internal static string RemoveHTML(this string input)
        {
            var result = RemoveHTMLRegex(input);
            return WebUtility.HtmlDecode(result);
        }

        public static T GetPrivateField<T>(this object obj, string name)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            FieldInfo field = type.GetField(name, flags);
            return (T)field.GetValue(obj);
        }

    }

    public static class StringExtensions
    {
        internal static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }
    }

    public static class EnumHelper
    {
        /// <summary>
        /// Converts the Enum values to strings using the (generally valid) assumption
        /// the removed spaces are to make it work at the code level
        /// but re-adding them makes it human-readable
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Enum value with spaces in between where capitals were</returns>
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            //regex from https://stackoverflow.com/questions/4488969/split-a-string-by-capital-letters
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            string[] result = r.Split(value.ToString());
            string resultFinal = result[0];
            for (int i = 1; i < result.Count(); i++)
            {
                resultFinal = resultFinal + " ";
                resultFinal += result[i].ToString();
            }

            return resultFinal;
        }
    }
}
