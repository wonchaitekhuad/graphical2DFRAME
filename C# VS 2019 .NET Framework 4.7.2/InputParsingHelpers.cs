using System;
using System.Globalization;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    /// <summary>
    /// Helper class for parsing numeric input with tolerance for different culture formats.
    /// Supports both dot (.) and comma (,) as decimal separators.
    /// </summary>
    public static class InputParsingHelpers
    {
        /// <summary>
        /// Attempts to parse a string to double using multiple culture formats.
        /// First tries CurrentCulture, then InvariantCulture, and finally by replacing comma with dot.
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

            // Trim whitespace
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

            // Try replacing comma with dot and parsing again
            string normalized = s.Replace(',', '.');
            if (double.TryParse(normalized, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }

            value = 0.0;
            return false;
        }

        /// <summary>
        /// Parses a string to double with culture tolerance, or throws an exception if parsing fails.
        /// </summary>
        /// <param name="s">The string to parse</param>
        /// <param name="paramName">Optional parameter name for error message</param>
        /// <returns>The parsed double value</returns>
        /// <exception cref="FormatException">Thrown when the string cannot be parsed as a double</exception>
        public static double ParseDoubleOrThrow(string s, string paramName = null)
        {
            if (TryParseDouble(s, out double value))
            {
                return value;
            }

            string errorMsg = string.IsNullOrEmpty(paramName)
                ? $"Unable to parse '{s}' as a numeric value."
                : $"Unable to parse '{s}' as a numeric value for {paramName}.";

            throw new FormatException(errorMsg);
        }

        /// <summary>
        /// Parses a string to double with culture tolerance, or returns a default value if parsing fails.
        /// </summary>
        /// <param name="s">The string to parse</param>
        /// <param name="defaultValue">The default value to return if parsing fails (default is 0.0)</param>
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
        /// Formats a double value as string using InvariantCulture for consistent serialization.
        /// </summary>
        /// <param name="value">The double value to format</param>
        /// <returns>String representation using dot as decimal separator</returns>
        public static string FormatDouble(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats a double value as string with specified format using InvariantCulture.
        /// </summary>
        /// <param name="value">The double value to format</param>
        /// <param name="format">The format string (e.g., "0.00", "F2")</param>
        /// <returns>String representation using dot as decimal separator</returns>
        public static string FormatDouble(double value, string format)
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}
