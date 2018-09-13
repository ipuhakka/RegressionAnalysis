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
                Console.Write("Ready\n>");
                input = Console.ReadLine();
                parseInput(input);
            }
        }

        /// <summary>
        /// Reads what the user inputted, and completes appropriate action based on it.
        /// Commands are separated from parameters with "->", and parameters are separated from
        /// each other with ','.
        /// </summary>
        /// <param name="input"></param>
        private static void parseInput(string input)
        {
            string[] array = input.Split(new string[] { "->" }, StringSplitOptions.None);

            switch (array[0].ToLower().Trim())
            {
                case "read":
                    ReadFile.Read(array[1].Trim());
                    break;
                case "set fitness":
                    setFitness(array[1].Trim());
                    break;
                case "set response":
                    setResponse(array[1].Trim());
                    break;
                case "print variables":
                    printVariables();
                    break;
                case "analyze":
                    selectAnalysisMethod();
                    break;
                case "q":
                    return;
                default:
                    Write.Error("Command not recognized");
                    break;
            }
        }

        private static void setFitness(string input)
        {
            switch (input)
            {
                case "AdjR2":
                    fitness = new AdjustedR2();
                    Write.Success("Set adjusted R2 as fitness criteria");
                    break;
                case "AIC":
                    fitness = new AIC();
                    Write.Success("Set AIC as fitness criteria");
                    break;
                default:
                    Write.Error("Unexpected input");
                    Write.Error("Possible paramameters: AIC, AdjR2");
                    break;
            }
        }

        private static void printVariables()
        {
            Console.Write("Varibles: ");
            foreach (string s in variables)
            {
                Console.Write(s + " ");
            }
            Console.Write("\n");
        }

        /// <summary>
        /// Prints variable names, sets response variable based on user input.
        /// </summary>
        private static void setResponse(string resp)
        {
            if (filepath == null)
            {
                Write.Error("Please select a file before choosing response variable");
                variables = null;
                return;
            }

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
