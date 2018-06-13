using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;


namespace RegressionAnalysis.Converter
{
    class CSV
    {

        /// <summary>
        /// Converts a csv-file into a Tuple containing response variable, and list of explanatory variables. Assumes that
        /// csv-file has column names in the first row.
        /// </summary>
        /// <param name="filePath">Path to the csv-file to be used.</param>
        /// <param name="responseVariable">Indicates what variable is used as the response variable.</param>
        /// <returns>Tuple with response variable, and a list of explanatory variables.</returns>
        /// /// <exception cref="ArgumentException">Thrown when no column name matches parameter
        /// responseVariable.</exception>
        /// <exception cref="FormatException">Thrown when converting any value to double fails.</exception>
        /// <exception cref="InvalidCSVError">Thrown when a row in text array has a different amount of
        /// data than the first row of the text array.</exception>
        public static Tuple<Variable, List<Variable>> ToVariableModel(string filePath, string responseVariable)
        {
            string[] allText = File.ReadAllLines(filePath);
            List<List<string>> columns;
            try
            {
                columns = ToColumns(allText);
            }
            catch (InvalidCSVError e)
            {
                Console.WriteLine(e.error);
                throw e;
            }

            try
            {
                return CreateTuple(columns, responseVariable);
            }
            catch (ArgumentException e)
            {
                throw e;
            }           
            catch (FormatException f)
            {
                throw f;
            }
        }

        /// <summary>
        /// Splits string array into column lists
        /// </summary>
        /// <param name="text">Array, where a row contains single observation data, or variable names.</param>
        /// <returns>List containing lists for every column in a csv-file.</returns>
        /// <exception cref="InvalidCSVError">Thrown when a row in text array has a different amount of
        /// data than the first row of the text array.</exception>
        private static List<List<string>> ToColumns(string[] text)
        {
            int length = text[0].Split(',').Length;
            List<List<string>> columns = new List<List<string>>();

            for (int j = 0; j < length; j++)
            {
                columns.Add(new List<string>());
            }

            int rowCount = 1;
            foreach (string s in text)
            {
                var data = s.Split(',');

                if (data.Length != length)
                    throw new InvalidCSVError("Row " + rowCount + " contained different amount of data than expected" );

                for (int i = 0; i < length; i++)
                {
                    columns[i].Add(data[i].Replace("\"", "").Trim());                   
                }
                rowCount++;
            }
            return columns;
        }

        /// <summary>
        /// Creates the tuple value to be returned by CSVToVariableModel.
        /// </summary>
        /// <param name="columns">Lists of columns, each first row contains name for the variable.</param>
        /// <param name="responseVariable">string indicating what variable is the response variable.</param>
        /// <returns>Tuple containing response variable, and a list of explanatory variables.</returns>
        /// <exception cref="ArgumentException">Thrown when no column name matches parameter
        /// responseVariable.</exception>
        /// <exception cref="FormatException">Thrown when converting any value to double fails.</exception>
        private static Tuple<Variable, List<Variable>> CreateTuple(List<List<string>> columns, string responseVariable)
        {
            Variable response = null;
            List<Variable> explanatory = new List<Variable>();

            for (int i = 0; i < columns.Count; i++)
            {
                string name = columns[i][0];
                columns[i].RemoveAt(0);
                List<double> values;

                try
                {
                    values = ValuesToDouble(columns[i]);
                }
                catch (FormatException e)
                {
                    throw e;
                }

                if (name.Equals(responseVariable))
                    response = new Variable(name, values);
                else
                    explanatory.Add(new Variable(name, values));
            }

            if (response != null)
                return Tuple.Create(response, explanatory);
            else
                throw new ArgumentException("No response variable named " + responseVariable + " was found");
        }

        /// <summary>
        /// Converts a list of strings into a list of doubles.
        /// </summary>
        /// <param name="column">List of strings, each string is a double value.</param>
        /// <returns>List of doubles, column converted into double values.</returns>
        /// <exception cref="FormatException">Thrown if any double conversion fails.</exception>
        private static List<double> ValuesToDouble(List<string> column)
        {
            CultureInfo culture = CultureInfo.InvariantCulture;

            List<double> converted = new List<double>();
            int lineCount = 1;
            foreach(string s in column)
            {            
                try
                {
                    converted.Add(Convert.ToDouble(s, culture));
                }
                catch (FormatException)
                {
                    throw new FormatException("File contained data that can't be converted into double values: line " + lineCount + ", string: " + s);
                }

                lineCount++;
            }
            return converted;
        }
    }
}
