using System;
using System.Collections.Generic;

namespace RegressionAnalysis
{
    public class AdjustedR2: Fitness
    {
        /// <summary>
        /// Calculates adjusted coefficient of determination value for a fitted model.
        /// </summary>
        /// <param name="y">List containing the response values.</param>
        /// <param name="x">List containing lists for all explanatory variables.</param>
        /// <returns>Coefficient of determination adjusted with number of explanatory variables
        /// and observations.</returns>
        /// <exception cref="MathError">Thrown when parameter lists are of different length.</exception>
        /// <exception cref="ArgumentException">Thrown when matrix created from parameters is not valid to fit least square points.</exception>
        public double EvaluateFitness(List<double> y, List<List<double>> x)
        {
            try
            {
                return ModelFit.AdjustedR2(y, x);
            }
            catch (MathError e)
            {
                throw e;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

    }
}
