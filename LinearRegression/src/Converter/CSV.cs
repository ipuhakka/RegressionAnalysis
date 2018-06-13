using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace RegressionAnalysis.Converter
{
    class CSV
    {
        
        public static Tuple<Variable, List<Variable>> CSVToVariableModel(string filePath, string responseVariable)
        {
            string[] allText = File.ReadAllLines(filePath);
            List<List<string>> columns;
            try
            {
                columns = ToColumns(allText);
            }
            catch (InvalidCSVError e)
            {
                Console.WriteLine(e.StackTrace);
                throw e;
            }

            return CreateTuple(columns, responseVariable);            
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
            int length = text[0].Length;
            List<List<string>> columns = new List<List<string>>();

            int rowCount = 1;
            foreach (string s in text)
            {
                var data = s.Split(',');

                if (data.Length != length)
                    throw new InvalidCSVError("Row " + rowCount + " contained different amount of data than expected" );

                for (int i = 0; i < length; i++)
                {
                    columns[i].Add(data[i]);                   
                }
                rowCount++;
            }
            return columns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column">List of strings, each string is a double value.</param>
        /// <returns>List of doubles, column converted into double values.</returns>
        /// <exception cref="FormatException">Thrown if any double conversion fails.</exception>
        private static List<double> ValuesToDouble(List<string> column)
        {
            List<double> converted = new List<double>();
            int lineCount = 1;
            foreach(string s in column)
            {
                try
                {
                    converted.Add(Convert.ToDouble(s));
                }
                catch (FormatException)
                {
                    throw new FormatException("File contained data that can't be converted into double values: line " + lineCount);
                }
                lineCount++;
            }
            return converted;
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

                if (columns[i][0].Equals(responseVariable))
                    response = new Variable(name, values);
                else
                    explanatory.Add(new Variable(name, values)); 
            }

            if (response != null)
                return Tuple.Create(response, explanatory);
            else
                throw new ArgumentException("No response variable named " + responseVariable + " was found");
        }
    }
}
