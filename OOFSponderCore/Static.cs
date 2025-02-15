using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            //TODO: fix this VERY hacky code
            //need to add special code for ContactsOnly to add a space
            if (value.ToString() == ExternalAudienceScope.ContactsOnly.ToString())
            {
                return "Contacts Only";
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
