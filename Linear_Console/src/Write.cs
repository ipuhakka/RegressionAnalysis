using System;

namespace Linear_Console
{
    class Write
    {
        /// <summary>
        /// Writes a message to console with red color to highlight message, then sets
        /// color back to default.
        /// </summary>
        /// <param name="message">string to be printed to the user.</param>
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a message to console with green color to highlight message, then sets
        /// color back to default.
        /// </summary>
        /// <param name="message">string to be printed to the user.</param>
        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

    }
}
