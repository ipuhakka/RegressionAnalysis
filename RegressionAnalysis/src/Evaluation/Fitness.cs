using System.Collections.Generic;
using RegressionAnalysis.ModelSelection;

namespace RegressionAnalysis.Evaluation
{
    public interface Fitness
    {
        /// <summary>
        /// Calculates how well data fits to a fitted regression line.
        /// </summary>
        /// <param name="model">Object of class model</param>
        /// <returns>Value indicating how much of the variability of y can be explained 
        /// with the model.</returns>
        double EvaluateFitness(Model model);

        /// <summary>
        /// Function checks if a new model has better fitness than the current best. Function exists
        /// because depending on the criteria, either a lower or a higher value can be the
        /// better value.
        /// </summary>
        /// <param name="currentBest">The model that is being compared against.</param>
        /// <param name="newModel">Model that is being tested on whether it has a better fitness
        /// than the current best model.</param>
        /// <returns>true if new model has better fitness, false otherwise.</returns>
        bool IsBetter(Model currentBest, Model newModel);
    }
}
