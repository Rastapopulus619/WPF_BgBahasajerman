using System;
using System.Collections.Generic;

namespace Bgb_DataAccessLibrary.Helpers.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Colorizes a single string with foreground and/or background colors and writes it to the console.
        /// </summary>
        /// <param name="text">The text to colorize.</param>
        /// <param name="foregroundColor">The color to use for the text (foreground).</param>
        /// <param name="backgroundColor">The color to use for the background.</param>
        public static void Colorize(this string text, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            if (foregroundColor.HasValue)
            {
                Console.ForegroundColor = foregroundColor.Value;
            }

            if (backgroundColor.HasValue)
            {
                Console.BackgroundColor = backgroundColor.Value;
            }

            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a multicolored line to the console using tuples, allowing both foreground and background color.
        /// </summary>
        /// <param name="coloredSegments">An array of tuples containing text, foreground color, and background color.</param>
        public static void ColorizeLine(params (string Text, ConsoleColor? ForegroundColor, ConsoleColor? BackgroundColor)[] coloredSegments)
        {
            foreach (var (text, foregroundColor, backgroundColor) in coloredSegments)
            {
                // Use a local variable to handle the defaulting of backgroundColor
                var effectiveBackgroundColor = backgroundColor ?? null;

                if (foregroundColor.HasValue)
                {
                    Console.ForegroundColor = foregroundColor.Value;
                }

                if (effectiveBackgroundColor.HasValue)
                {
                    Console.BackgroundColor = effectiveBackgroundColor.Value;
                }

                Console.Write(text);
            }
            Console.ResetColor();
            Console.WriteLine();
        }
        // Overloaded method to handle just text and foreground color, background is automatically null
        public static void ColorizeLine(params (string Text, ConsoleColor? ForegroundColor)[] coloredSegments)
        {
            ColorizeLine(coloredSegments.Select(segment => (segment.Text, segment.ForegroundColor, (ConsoleColor?)null)).ToArray());
        }


        /// <summary>
        /// Enables fluent chaining for building multicolored strings with both foreground and background colors.
        /// </summary>
        public class FluentColorizer
        {
            private readonly List<(string Text, ConsoleColor? ForegroundColor, ConsoleColor? BackgroundColor)> _segments = new();

            /// <summary>
            /// Appends a new text segment with specified foreground and/or background colors.
            /// </summary>
            public FluentColorizer Append(string text, ConsoleColor? foregroundColor = ConsoleColor.Gray, ConsoleColor? backgroundColor = ConsoleColor.Black)
            {
                _segments.Add((text, foregroundColor, backgroundColor));
                return this;
            }

            /// <summary>
            /// Writes the multicolored string to the console.
            /// </summary>
            public void WriteLine()
            {
                foreach (var (text, foregroundColor, backgroundColor) in _segments)
                {
                    if (foregroundColor.HasValue)
                    {
                        Console.ForegroundColor = foregroundColor.Value;
                    }

                    if (backgroundColor.HasValue)
                    {
                        Console.BackgroundColor = backgroundColor.Value;
                    }

                    Console.Write(text);
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Starts a fluent colorizer for chaining multicolored strings with both foreground and background colors.
        /// </summary>
        /// <param name="text">The initial text.</param>
        /// <param name="color">The foreground color of the initial text.</param>
        /// <param name="backgroundColor">The background color of the initial text.</param>
        /// <returns>A FluentColorizer for chaining.</returns>
        public static FluentColorizer ColorizeMulti(this string text, ConsoleColor? color = null, ConsoleColor? backgroundColor = null)
        {
            var colorizer = new FluentColorizer();
            return colorizer.Append(text, color, backgroundColor);
        }
    }
}
/*
 
Usage Examples:

    Single Colorization (either foreground or background, or both):

        "Hello, World!".Colorize(ConsoleColor.Green); // Only foreground
        "Hello, World!".Colorize(null, ConsoleColor.Blue); // Only background
        "Hello, World!".Colorize(ConsoleColor.Yellow, ConsoleColor.Red); // Both foreground and background

    Multicolored Line:

        StringExtensionMethods.ColorizeLine(
            ("[INFO] ", ConsoleColor.Cyan, null),
            ("Hello, World", ConsoleColor.White, ConsoleColor.DarkBlue)
        );

    Fluent Chaining:

        "Part1".Colorize(ConsoleColor.Red)
            .Append("Part2", ConsoleColor.Blue)
            .Append("Part3", ConsoleColor.Yellow, ConsoleColor.Gray)
            .WriteLine();


 * */
