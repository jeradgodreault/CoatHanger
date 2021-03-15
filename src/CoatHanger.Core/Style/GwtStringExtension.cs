using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("UnitTests")]
namespace CoatHanger.Core.Style
{
    public static class GwtStringExtension
    {
        public static string ToFormattedGwt(this string value, string removeContextWord)
        {
            value = value.Trim();

            if (value.StartsWith(removeContextWord, StringComparison.InvariantCultureIgnoreCase))
            {
                value = value.Remove(0, removeContextWord.Length);
            }

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static string ToGivenFormat(this string given)
        {
            return ToFormattedGwt(given, "given ");
        }

        public static string ToWhenFormat(this string when)
        {
            return ToFormattedGwt(when, "when ");
        }

        public static string ToAndFormat(this string and)
        {         
            return ToFormattedGwt(and, "and ");
        }

        public static string ToThenFormat(this string then)
        {
            return ToFormattedGwt(then, "then ");
        }

    }
}
