using System;
using System.Globalization;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    /// <summary>
    /// Provides robust culture-aware parsing methods for numeric input.
    /// Handles both comma and dot decimal separators to support international users.
    /// </summary>
    public static class InputParsingHelpers
    {
        /// <summary>
        /// Attempts to parse a string to a double using multiple culture strategies.
        /// First tries CurrentCulture, then InvariantCulture, then swaps comma/dot and retries.
        /// </summary>
        /// <param name="s">The string to parse</param>
        /// <param name="value">The parsed double value if successful</param>
        /// <returns>True if parsing succeeded, false otherwise</returns>
        public static bool TryParseDouble(string s, out double value)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                value = 0.0;
                return false;
            }

            // Remove spaces
            s = s.Trim();

            // Try parsing with current culture
            if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out value))
            {
                return true;
            }

            // Try parsing with invariant culture (dot as decimal separator)
            if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }

            // Try replacing comma with dot and parse with invariant culture
            string dotVersion = s.Replace(',', '.');
            if (double.TryParse(dotVersion, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }

            // Try replacing dot with comma and parse with current culture (if current culture uses comma)
            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
            {
                string commaVersion = s.Replace('.', ',');
                if (double.TryParse(commaVersion, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture, out value))
                {
                    return true;
                }
            }

            value = 0.0;
            return false;
        }

        /// <summary>
        /// Parses a string to a double or throws an ArgumentException if parsing fails.
        /// </summary>
        /// <param name="s">The string to parse</param>
        /// <param name="paramName">Optional parameter name for the exception message</param>
        /// <returns>The parsed double value</returns>
        /// <exception cref="ArgumentException">Thrown when parsing fails</exception>
        public static double ParseDoubleOrThrow(string s, string paramName = null)
        {
            if (TryParseDouble(s, out double value))
            {
                return value;
            }

            string message = paramName != null
                ? $"Unable to parse '{s}' as a decimal number for parameter '{paramName}'."
                : $"Unable to parse '{s}' as a decimal number.";

            throw new ArgumentException(message, paramName);
        }

        /// <summary>
        /// Parses a string to a double or returns a default value if parsing fails.
        /// </summary>
        /// <param name="s">The string to parse</param>
        /// <param name="defaultValue">The default value to return if parsing fails</param>
        /// <returns>The parsed double value or the default value</returns>
        public static double ParseDoubleOrDefault(string s, double defaultValue = 0.0)
        {
            if (TryParseDouble(s, out double value))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// Formats a double value to a string using the current culture.
        /// </summary>
        /// <param name="value">The value to format</param>
        /// <param name="decimalPlaces">Number of decimal places (default: 3)</param>
        /// <returns>Formatted string</returns>
        public static string FormatDouble(double value, int decimalPlaces = 3)
        {
            string format = "F" + decimalPlaces;
            return value.ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Formats a double value to a string using invariant culture (for file I/O).
        /// </summary>
        /// <param name="value">The value to format</param>
        /// <returns>Formatted string with dot as decimal separator</returns>
        public static string FormatDoubleInvariant(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
