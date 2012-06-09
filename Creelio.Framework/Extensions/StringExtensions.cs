namespace Creelio.Framework.Core.Extensions.StringExtensions
{
    using System;
    using System.Text;
    using Creelio.Framework.Core.Extensions.MaybeMonad;

    public static class StringExtensions
    {
        public static string[] Split(this string s, StringSplitOptions options, params char[] separator)
        {
            return s.ThrowIfNull(_ => new NullReferenceException("The string you are attempting to split is null."))
                    .Split(separator, options);
        }

        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            else
            {
                var sb = new StringBuilder();
                bool firstWord = true;
                bool startOfWord = false;

                for (int ii = 0; ii < s.Length; ii++)
                {
                    char c = s[ii];

                    if (char.IsWhiteSpace(c))
                    {
                        startOfWord = true;
                    }
                    else if (firstWord)
                    {
                        c = char.ToLower(c);
                        firstWord = false;
                        sb.Append(c);
                    }
                    else if (startOfWord)
                    {
                        c = char.ToUpper(c);
                        startOfWord = false;
                        sb.Append(c);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }

                return sb.ToString();
            }
        }

        public static string ToDoubleQuotedString<T>(this T obj)
        {
            return ToDoubleQuotedString(obj, o => o.ToString());
        }

        public static string ToDoubleQuotedString<T>(this T obj, Func<T, string> toString)
        {
            return string.Format("\"{0}\"", obj == null ? string.Empty : toString(obj));
        }

        public static string ToSingleQuotedString<T>(this T obj)
        {
            return ToSingleQuotedString(obj, o => o.ToString());
        }

        public static string ToSingleQuotedString<T>(this T obj, Func<T, string> toString)
        {
            toString.ThrowIfNull(_ => new ArgumentNullException("toString"));
            return string.Format("'{0}'", obj == null ? string.Empty : toString(obj));
        }
    }
}
