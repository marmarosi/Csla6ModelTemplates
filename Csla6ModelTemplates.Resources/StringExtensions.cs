using System;
using System.Diagnostics;
using System.Globalization;

namespace Csla6ModelTemplates.Resources
{
    /// <summary>
    /// This class provides helper methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Formats the current string using the specified arguments.
        /// </summary>
        /// <param name="string">The string to format.</param>
        /// <param name="args">The arguments used to format.</param>
        /// <returns>The formatted string.</returns>
        [DebuggerStepThrough]
        public static string With(this string @string, params object[] args)
        {
            return @string.With(CultureInfo.CurrentUICulture, args);
        }

        /// <summary>
        /// Formats the current string using the specified format provider and arguments.
        /// </summary>
        /// <param name="string">The string to format.</param>
        /// <param name="provider">An object that provides culture specific formatting information.</param>
        /// <param name="args">The arguments used to format.</param>
        /// <returns>The formatted string.</returns>
        [DebuggerStepThrough]
        public static string With(this string @string, IFormatProvider provider, params object[] args)
        {
            if (string.IsNullOrEmpty(@string))
                return @string;

            if (provider == null)
                provider = CultureInfo.CurrentUICulture;

            return string.Format(provider, @string, args);
        }

        /// <summary>
        /// Cuts off the end of the string.
        /// </summary>
        /// <param name="string">The string to truncate.</param>
        /// <param name="length">The number of the characters to cut off from the end of the string.</param>
        /// <returns>The truncated string.</returns>
        [DebuggerStepThrough]
        public static string CutEnd(this string @string, int length)
        {
            return length < @string.Length ?
                @string.Substring(0, @string.Length - length) :
                @string;
        }
    }
}
