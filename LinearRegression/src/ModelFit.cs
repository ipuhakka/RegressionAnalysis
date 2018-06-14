using System;
using System.Collections.Generic;
using MathNet.Numerics;

namespace RegressionAnalysis
{
    class ModelFit
    {

        /// <summary>
        /// Calculates adjusted coefficient of determination value for a fitted model.
        /// </summary>
        /// <param name="y">List containing the response values.</param>
        /// <param name="x">List containing lists for all explanatory variables.</param>
        /// <returns>Coefficient of determination adjusted with number of explanatory variables
        /// and observations.</returns>
        /// <exception cref="MathError">Thrown when parameter lists are of different length.</exception>
        /// <exception cref="ArgumentException">Thrown when matrix created from parameters is not valid.</exception>
        public static double AdjustedR2(List<double> y, List<List<double>> x)
        {
            List<double> fitted;
            int n = y.Count;
            int k = x.Count;

            try
            {
                fitted = FittedValues(y, x);
            }
            catch (MathError e)
            {
                Console.WriteLine(e.StackTrace);
                throw e;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            double R2 = GoodnessOfFit.CoefficientOfDetermination(fitted, y);

            return 1 - (((1 - R2) * (n - 1)) / (n - k - 1));
        }

        /// <summary>
        /// Calculates fitted values for response value y based on model containing variables listed in
        /// x.
        /// </summary>
        /// <param name="y">List containing the response values.</param>
        /// <param name="x">List containing lists for all explanatory variables.</param>
        /// <returns>List of the fitted value for y based on the model.</returns>
        /// <exception cref="MathError">Thrown when parameter lists are of different length.</exception>
        /// <exception cref="ArgumentException">Thrown when matrix created from parameters is not valid to fit least square points.</exception>
        public static List<double> FittedValues(List<double> y, List<List<double>> x)
        {
            List<double> yFitted = new List<double>();

            foreach (List<double> list in x)
            {
                if (list.Count != y.Count)
                    throw new MathError("List sizes are of different length");
            }
            try
            {
                double[] coefficients = GetCoefficients(y, x);
                return CalculateFittedValues(coefficients, y, x);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Calculates the fitted values for a model.
        /// </summary>
        /// <param name="coefficients">OLS estimates for model variables.</param>
        /// <param name="y">Actual response values.</param>
        /// <param name="x">List of lists containing explanatory varible data.</param>
        /// <returns>List of fitted values based on the model where each list in x is explanatory variable for
        /// response variable y.</returns>
        private static List<double> CalculateFittedValues(double[] coefficients, List<double> y, List<List<double>> x)
        {
            List<double> yFitted = new List<double>();
            int varIndex = 0;
            foreach (double d in y)
            {
                double yFit = coefficients[0];
                int coefIndex = 1;
                foreach (List<double> variable in x)
                {
                    yFit = yFit + (coefficients[coefIndex] * variable[varIndex]);
                    coefIndex++;
                }
                varIndex++;
                yFitted.Add(yFit);
            }

            return yFitted;
        }

        /// <summary>
        /// Uses MathNets Fit.MultiDim to calculate least squared coefficients for model.
        /// </summary>
        /// <param name="y">Response variable</param>
        /// <param name="x">Explanatory variables for the model. List contains data for one
        /// variable.</param>
        /// <returns>OLS-coefficients for the model.</returns>
        /// <exception cref="ArgumentException">Thrown when fitting the least square points fails.</exception>
        private static double[] GetCoefficients(List<double> y, List<List<double>> x)
        {
            double[][] columns = Matrix.InvertVariableList(x);

            double[] coefficients;
            try
            {
                coefficients = Fit.MultiDim(columns, y.ToArray(), true);
            }
            catch (ArgumentException e)
            {
                throw e;
            }

            return coefficients;
        }

    }
}
