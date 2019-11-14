using System;

namespace Service.Extensions
{
    public static class StringExtension
    {
        public static bool CaseInsensitiveContains(this string text, string value)
        {
            const StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase;
            return text.IndexOf(value, stringComparison) >= 0;
        }
    }
}
