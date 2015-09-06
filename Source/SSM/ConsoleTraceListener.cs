using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SSM
{
    /// <summary>
    /// Directs tracing or debugging output to the standard error stream with
    /// configurable colors per category.
    /// </summary>
    public class ConsoleTraceListener : TextWriterTraceListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleTraceListener"/>
        /// class.
        /// </summary>
        public ConsoleTraceListener() : base(Console.Error)
        {
            ColorMap = new Dictionary<string, ConsoleColor>();
            ColorMap.Add("Critical", ConsoleColor.Red);
            ColorMap.Add("Error", ConsoleColor.Red);
            ColorMap.Add("Warning", ConsoleColor.Yellow);
            ColorMap.Add("Information", ConsoleColor.Gray);
            ColorMap.Add("Verbose", ConsoleColor.DarkGray);
        }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey, TValue}"/> that maps category
        /// names to a <see cref="ConsoleColor"/> value that determines the
        /// foreground color used to write messages in that category.
        /// </summary>
        public Dictionary<string, ConsoleColor> ColorMap { get; }

        /// <summary>
        /// Writes a category name and a message to the console, using the
        /// foreground color specified in the <see cref="ColorMap"/> property.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">
        /// A category name used to organize the output.
        /// </param>
        public override void Write(string message, string category)
        {
            var color = ConsoleColor.Gray;
            if (ColorMap.TryGetValue(category, out color))
            {
                Console.ForegroundColor = color;
                base.Write(message ?? string.Empty);
                Console.ResetColor();
            }
            else
            {
                base.Write(message, category);
            }
        }

        /// <summary>
        /// Writes a category name and a message to the console, followed by a
        /// line terminator, using the foreground color specified in the <see
        /// cref="ColorMap"/> property.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">
        /// A category name used to organize the output.
        /// </param>
        public override void WriteLine(string message, string category)
        {
            var color = ConsoleColor.Gray;
            if (ColorMap.TryGetValue(category, out color))
            {
                Console.ForegroundColor = color;
                base.WriteLine(message ?? string.Empty);
                Console.ResetColor();
            }
            else
            {
                base.WriteLine(message, category);
            }
        }
    }
}
