using System.Collections.Generic;

namespace RegressionAnalysis.Evaluation
{
    public interface Fitness
    {
        /// <summary>
        /// Calculates how well data fits to a fitted regression line.
        /// </summary>
        /// <param name="y">List containing response variable.</param>
        /// <param name="x">List contains a list for each explanatory variable in the model.</param>
        /// <returns>Value indicating how much of the variability of y can be explained 
        /// with the model.</returns>
        double EvaluateFitness(List<double> y, List<List<double>> x);
    }
}
