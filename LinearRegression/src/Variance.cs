using System;
using System.Collections.Generic;
using System.Linq;

namespace RegressionAnalysis
{
    public class Variance
    {

        /// <summary>
        /// calculates the standard deviation from a double list. returns standard deviation.
        /// STD = √((Σ(x[i] - mean)^2) / (N - 1))
        /// </summary>
        /// <param name="list">List for which standard deviation is calculated.</param>
        /// <returns>Standard deviation for parameter list.</returns>
        /// <exception cref="MathError">Thrown when list count is 0 or list is null</exception>
        public static double StandardDeviation(List<double> list)
        {
            /* calculates the standard deviation from a double list. returns standard deviation.
             *  STD = √((Σ(x[i] - mean)^2) / (N - 1))*/
            if (list == null || list.Count == 0)
                throw new MathError("List is either null or has no elements");

            double meanDeviation = 0;

            //get mean value
            double mean = list.Average();

            //calculate sum of mean deviations ^2

            foreach (double d in list)
            {
                meanDeviation = meanDeviation + ((d - mean) * (d - mean)); //power of 2
            }

            //return the square root of meanDeviation / N - 1
            return Math.Sqrt((meanDeviation / (list.Count - 1)));
        }

        /// <summary>
        /// Calculates the covariance of variables x and y. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Joint variability of x and y.</returns>
        /// <<exception cref="MathError">Thrown when either list count is 0 or null, and if list are
        /// of different size</exception>
        public static double Covariance(List<double> x, List<double> y)
        {
            if (x == null || y == null)
                throw new MathError("Parameter list cannot be a null item");

            if (x.Count != y.Count)
                throw new MathError("Variable lists not the same length");

            if (x.Count == 0 && y.Count == 0)
                throw new MathError("Parameter lists have 0 items");

            double xMean = x.Average();
            double yMean = y.Average();

            double covariance = 0;

            //calc. sum of mean dif. products
            for (int i = 0; i < x.Count; i++)
            {
                covariance = covariance + ((x[i] - xMean) * (y[i] - yMean));
            }

            //divide by n - 1
            return covariance / (x.Count - 1);
        }
    }
}
