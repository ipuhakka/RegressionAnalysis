using System;
using System.Collections.Generic;
using RegressionAnalysis.Converter;
using RegressionAnalysis.ModelSelection;
using RegressionAnalysis.Evaluation;
using RegressionAnalysis.Exception;

namespace Linear_Console
{
    public class Console_Main
    {
        public static string responseVariable { get; set; }
        public static List<string> variables { get; set; }
        public static string filepath { get; set; }
        private static Fitness fitness = new AdjustedR2();

        /// <summary>
        /// Main loop for the console application. Takes user inputs in a loop and handles it accordingly. 
        /// </summary>
        /// <returns>0 on succesful quit, -1 on error.</returns>
        public static void Start()
        {
            string input = "";

            while (!input.Equals("q"))
            {
                Console.WriteLine("1. Read a file in\n2. Assign response variable\n3. Backward elimination analysis\n\nType 'q' for quitting");
                input = Console.ReadLine();
                handleInput(input);
            }
        }

        /// <summary>
        /// starts a function based on user given input.
        /// </summary>
        /// <param name="input">user given input.</param>
        /// <returns>-1 on error, 0 on success.</returns>
        private static int handleInput(string input)
        {
            switch (input)
            {
                case "1":
                    ReadFile.Read();
                    break;
                case "2":
                    setResponse();
                    break;
                case "3":
                    DoBackwardElimination();
                    break;
                case "q":
                    return 0;

                default:
                    Write.Error("Unexpected input");
                    return -1;
            }

            return 0;
        }

        /// <summary>
        /// Prints variable names, sets response variable based on user input.
        /// </summary>
        /// <returns>0 if input variable is found, -1 otherwise.</returns>
        private static int setResponse()
        {
            if (filepath == null)
            {
                Write.Error("Please select a file before choosing response variable");
                variables = null;
                return -1;
            }

            Console.Write("Varibles: ");
            foreach(string s in variables)
            {
                Console.Write(s + " ");
            }
            Console.Write("\n");
            Console.Write("Write variable name to choose variable as response: ");
            string resp = Console.ReadLine();

            if (variables.Contains(resp))
            {
                responseVariable = resp;
                Write.Success(resp + " set as response variable");
                return 0;
            }
            else
                Write.Error("Variable '" + resp + "' not found");

            return -1;
        }

        private static int DoBackwardElimination()
        {
            Model model, results;
            if (filepath == null || responseVariable == null)
            {
                Write.Error("Please read file and set response variable before completing analysis");
                return -1;
            }


            model = CSV.ToModel(filepath, responseVariable);
            BackwardElimination be = new BackwardElimination();
            try
            {
                results = be.FindBestModel(model, fitness);

                Console.Write("Model: " + results.getYVar().name + " ");
                foreach (Variable v in results.getXVars())
                {
                    Console.Write(v.name + " ");
                }

                Console.Write("fitness: " + results.fitness + "\n");
                return 0;
            }
            catch (MathError)
            {
                Write.Error("Matrix constructed from csv-file was of incorrect type");
                return -1;
            }
        }
    }
}
