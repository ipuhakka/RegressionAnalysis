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
    }
}
