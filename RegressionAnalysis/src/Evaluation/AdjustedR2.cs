using System.Collections.Generic;
using RegressionAnalysis.ModelSelection;

namespace RegressionAnalysis.Evaluation
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
        public double EvaluateFitness(Model m)
        {
            return ModelFit.AdjustedR2(m.getYVar().values, m.getXVariableLists());
        }

        /// <summary>
        /// Returns true when new model has a higher fitness value (as higher adjusted R2 value
        /// is better.)
        /// </summary>
        /// <param name="currentBest">The model that is being compared against.</param>
        /// <param name="newModel">Model that is being tested on whether it has a higher fitness
        /// than the current best model.</param>
        /// <returns>true if new model has a higher adjusted R2 value, false if not.</returns>
        public bool IsBetter(Model currentBest, Model newModel)
        {
            if (newModel.fitness > currentBest.fitness)
                return true;
            else
                return false;
        }

    }
}
