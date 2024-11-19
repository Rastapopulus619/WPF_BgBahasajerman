using System;
using System.Collections.Generic;

namespace Bgb_DataAccessLibrary.Helpers.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Colorizes a single string and writes it to the console.
        /// </summary>
        /// <param name="text">The text to colorize.</param>
        /// <param name="color">The color to use for the text.</param>
        public static void Colorize(this string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a multicolored line to the console using tuples.
        /// </summary>
        /// <param name="coloredSegments">An array of tuples containing text and their corresponding colors.</param>
        public static void ColorizeLine(params (string Text, ConsoleColor Color)[] coloredSegments)
        {
            foreach (var (text, color) in coloredSegments)
            {
                Console.ForegroundColor = color;
                Console.Write(text);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Enables fluent chaining for building multicolored strings.
        /// </summary>
        public class FluentColorizer
        {
            private readonly List<(string Text, ConsoleColor Color)> _segments = new();

            /// <summary>
            /// Appends a new text segment with a specified color.
            /// </summary>
            public FluentColorizer Append(string text, ConsoleColor color)
            {
                _segments.Add((text, color));
                return this;
            }

            /// <summary>
            /// Writes the multicolored string to the console.
            /// </summary>
            public void WriteLine()
            {
                foreach (var (text, color) in _segments)
                {
                    Console.ForegroundColor = color;
                    Console.Write(text);
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Starts a fluent colorizer for chaining multicolored strings.
        /// </summary>
        /// <param name="text">The initial text.</param>
        /// <param name="color">The color of the initial text.</param>
        /// <returns>A FluentColorizer for chaining.</returns>
        public static FluentColorizer ColorizeMulti(this string text, ConsoleColor color)
        {
            var colorizer = new FluentColorizer();
            return colorizer.Append(text, color);
        }
    }
}
/*
 
How It Works
    Single-String Coloring

        Use the Colorize extension method for simple, one-off colored strings.

        "TestString".Colorize(ConsoleColor.Green);

    Multicolored Line

            Use the ColorizeLine method to easily output a multicolored line.   
        
            ColorizeLine(
                ("[DI] Resolved:", ConsoleColor.Magenta),
                ($"{serviceType.Name} -> ", ConsoleColor.DarkGray),
                ($"{resolvedInstance.GetType().Name}", ConsoleColor.White),
                ("| HashCode: ", ConsoleColor.DarkGray),
                ($"{resolvedInstance.GetHashCode()}", ConsoleColor.DarkYellow));

    Chained Multicolored Strings

            Use the fluent Colorize method for chaining multicolored segments:
        
            "Part1".Colorize(ConsoleColor.Red)
                .Append("Part2", ConsoleColor.Blue)
                .Append("Part3", ConsoleColor.Yellow)
                .WriteLine();


 * */
