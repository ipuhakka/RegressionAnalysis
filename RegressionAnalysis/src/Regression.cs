using System;
using System.Linq;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using RegressionAnalysis.Exception;

namespace RegressionAnalysis
{
    public class Regression
    {
        /// <summary>
        /// Calculates the estimated regression coefficient for variable x which explains variable y.
        /// Beta estimate is cov(x, y) / var(x). Used to calculate single explanatory variable coefficient.
        /// </summary>
        /// <param name="y">List containing values of response variable.</param>
        /// <param name="x">List containing values of explanatory variable.</param>
        /// <returns>Double, an estimate for how much y changes with one unit change in x.</returns>
        /// <exception cref="MathError">Thrown when either list count is 0 or null, and if list are
        /// of different size </exception>
        public static double BetaEstimate(List<double> y, List<double> x) 
        {
            if (x == null || y == null)
                throw new MathError("Parameter list cannot be a null item");

            if (x.Count != y.Count)
                throw new MathError("Variable lists not the same length");

            if (x.Count == 0 && y.Count == 0)
                throw new MathError("Parameter lists have 0 items");

            double cov = Variance.Covariance(x, y);
            double xVariance = Math.Pow(Variance.StandardDeviation(x), 2.0);

            return cov / xVariance;
        }

        /// <summary>
        /// Calculates β0-parameter for simple linear regression model.
        /// </summary>
        /// <param name="y">List containing values of response variable.</param>
        /// <param name="x">List containing values of explanatory variable.</param>
        /// <returns>double, represents value of y in a state where x is 0.</returns>
        public static double AlphaEstimateSingle(List<double> y, List<double> x)
        {
            if (y == null || x == null)
                throw new MathError("Null parameters not accepted");

            if (y.Count != x.Count)
                throw new MathError("Parameter lists of different size");

            return y.Average() - (BetaEstimate(y, x) * x.Average());
        }

        /// <summary>
        /// Returns beta coefficient estimates for a model where y contains the response variable,
        /// and x contains all explanatory variables. One list contains a column. Adds a column
        /// of 1's as the first column. MathNet LinearRegression package contains readily made
        /// methods for this function, this function is done as a practice. 
        /// </summary>
        /// <param name="y">List containing values of response variable.</param>
        /// <param name="x">List containing all columns that are explanatory variables in the model.</param>
        /// <returns>Vector of coefficient estimates converted into a double list.</returns>
        /// <exception cref="ArgumentException">Thrown when lists are of different size.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when list is not of positive length.</exception>
        /// <exception cref="MathError">Thrown if parameters contain null lists</exception>
        public static List<double> BetaEstimates(List<double> y, List<List<double>> x)
        {
            List<double> betas = new List<double>();
            List<double> ones = new List<double>();

            if (y == null)
                throw new MathError("Null list in parameter y");

            foreach (List<double> list in x)
            {
                if (list == null)
                    throw new MathError("Null list in parameter x");
            }

            int length = x[0].Count;
            for (int i = 0; i < length; i++)
            {
                ones.Add(1);
            }

            x.Insert(0, ones);

            Matrix<double> X = Matrix.Convert(x);
            Vector<double> yVector = Vector<double>.Build.DenseOfArray(y.ToArray());
            Vector<double> betaVector = (X.Transpose() * X).Inverse() * (X.Transpose() * yVector);
            x.RemoveAt(0);

            double[] betaArray = betaVector.ToArray();
            return betaArray.ToList();
        }
    }
}
