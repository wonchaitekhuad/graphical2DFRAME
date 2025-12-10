using System;
using System.Globalization;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    /// <summary>
    /// Test class to demonstrate decimal parsing with both dot and comma separators
    /// </summary>
    public class TestParsingHelpers
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Testing InputParsingHelpers ===\n");
            
            // Test cases with different formats
            string[] testInputs = {
                "1.23",    // Dot separator
                "1,23",    // Comma separator
                "123.456", // Dot with more decimals
                "123,456", // Comma with more decimals
                "-45.67",  // Negative with dot
                "-45,67",  // Negative with comma
                "0.0225",  // Small value with dot
                "0,0225",  // Small value with comma
                "2e8",     // Scientific notation
                "4.21875e-5", // Scientific with decimals
                "invalid"  // Invalid input
            };
            
            Console.WriteLine("Testing TryParseDouble:");
            Console.WriteLine("------------------------");
            foreach (string input in testInputs)
            {
                if (InputParsingHelpers.TryParseDouble(input, out double result))
                {
                    Console.WriteLine($"✓ '{input}' => {result}");
                }
                else
                {
                    Console.WriteLine($"✗ '{input}' => Failed to parse");
                }
            }
            
            Console.WriteLine("\nTesting ParseDoubleOrDefault:");
            Console.WriteLine("----------------------------");
            foreach (string input in testInputs)
            {
                double result = InputParsingHelpers.ParseDoubleOrDefault(input, -999.0);
                Console.WriteLine($"'{input}' => {result}");
            }
            
            Console.WriteLine("\nTesting FormatDouble:");
            Console.WriteLine("--------------------");
            double[] values = { 1.23, 123.456, 0.0225, 200000000.0, 0.000042188 };
            foreach (double value in values)
            {
                string formatted = InputParsingHelpers.FormatDouble(value);
                Console.WriteLine($"{value} => '{formatted}'");
            }
            
            Console.WriteLine("\n=== Test Complete ===");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
