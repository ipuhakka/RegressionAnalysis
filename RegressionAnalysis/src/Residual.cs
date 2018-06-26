using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RegressionAnalysis
{
    public class Residual
    {
        /// <summary>
        /// Calculates sum of squared residuals.
        /// </summary>
        /// <param name="y">Response variable for the model.</param>
        /// <param name="x">List of columns, explanatory variables.</param>
        public static double SumOfSquaredResiduals(List<double> y, List<List<double>> x)
        {
            double RSS = 0;
            List<double> coefficients = Regression.BetaEstimates(y, x);
            double alpha = coefficients[0];

            for (int i = 0; i < y.Count; i++)
            {
                int coefIndex = 1;
                double subSum = alpha;
                for (int j = 0; j < x.Count; j++)
                {
                    subSum = subSum + (coefficients[coefIndex] * x[j].ElementAt(i));
                    coefIndex++;
                }
                subSum = Math.Pow(y[i] - subSum, 2);
                RSS = RSS + subSum;
            }
            return RSS;
        }

    }
}
