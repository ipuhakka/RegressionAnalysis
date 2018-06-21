using System;
using RegressionAnalysis.ModelSelection;

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

        /// <summary>
        /// Prints model and its fitness value to the console.
        /// </summary>
        /// <param name="model"></param>
        public static void Model(Model model)
        {
            Console.Write("Model: " + model.getYVar().name + " ");
            foreach (Variable v in model.getXVars())
            {
                Console.Write(v.name + " ");
            }

            Console.Write("fitness: " + model.fitness + "\n");
        }
    }
}
