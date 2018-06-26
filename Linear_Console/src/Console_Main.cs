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
        public static void Start()
        {
            string input = "";

            while (!input.Equals("q"))
            {
                Console.WriteLine("1. Read a file in\n2. Assign response variable\n3. Change fitness criteria\n4. Model analysis\n\nType 'q' for quitting");
                input = Console.ReadLine();
                handleInput(input);
            }
        }

        /// <summary>
        /// starts a function based on user given input.
        /// </summary>
        /// <param name="input">user given input.</param>
        /// <returns>-1 on error, 0 on success.</returns>
        private static void handleInput(string input)
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
                    setFitness();
                    break;
                case "4":
                    selectAnalysisMethod();
                    break;
                case "q":
                    return;

                default:
                    Write.Error("Unexpected input");
                    return;
            }

            return;
        }

        private static void setFitness()
        {
            Console.WriteLine("1. Adjusted R2 (Default)\n2. Akaike information criteria");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    fitness = new AdjustedR2();
                    Write.Success("Set adjusted R2 as fitness criteria");
                    break;
                case "2":
                    fitness = new AIC();
                    Write.Success("Set AIC as fitness criteria");
                    break;
                default:
                    Write.Error("Unexpected input");
                    break;
            }
        }

        /// <summary>
        /// Prints variable names, sets response variable based on user input.
        /// </summary>
        private static void setResponse()
        {
            if (filepath == null)
            {
                Write.Error("Please select a file before choosing response variable");
                variables = null;
                return;
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
                return;
            }
            else
                Write.Error("Variable '" + resp + "' not found");

            return;
        }

        private static void selectAnalysisMethod()
        {
            Console.WriteLine("1. Backward elimination\n2. Full model analysis");
            string result = Console.ReadLine();

            switch (result)
            {
                case "1":
                    DoBackwardElimination();
                    break;
                case "2":
                    DoModelSelection();
                    break;
                default:
                    Write.Error("Unexpected input");
                    break;
            }
        }

        private static void DoModelSelection()
        {
            Model model, results;
            if (filepath == null)
            {
                Write.Error("No file read in");
                return;
            }

            if (responseVariable == null)
            {
                Write.Error("No response variable selected");
                return;
            }

            model = CSV.ToModel(filepath, responseVariable);
            FullSelection sel = new FullSelection(model, fitness);
            results = sel.SelectBestFit();
            Write.Model(results);
        }

        private static void DoBackwardElimination()
        {
            Model model, results;
            if (filepath == null)
            {
                Write.Error("No file read in");
                return;
            }

            if (responseVariable == null)
            {
                Write.Error("No response variable selected");
                return;
            }

            model = CSV.ToModel(filepath, responseVariable);
            BackwardElimination be = new BackwardElimination();
            try
            {
                results = be.FindBestModel(model, fitness);
                Write.Model(results);
                return;
            }
            catch (MathError)
            {
                Write.Error("Matrix constructed from csv-file was of incorrect type");
                return;
            }
        }
    }
}
