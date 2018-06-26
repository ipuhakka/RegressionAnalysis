using System;
using RegressionAnalysis.ModelSelection;


namespace RegressionAnalysis.Evaluation
{
    public class AIC: Fitness
    {
        /// <summary>
        /// returns the akaike information criteria value for model m.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when any variable in the model has
        /// different number of obesrvations.</exception>
        public double EvaluateFitness(Model m)
        {
            return ModelFit.AIC(m.getYVar().values, m.getXVariableLists());
        }

        /// <summary>
        /// Returns true when newModel has a lower AIC-value. 
        /// </summary>
        /// <returns>True if newModel has lower fitness value, else false.</returns>
        public bool IsBetter(Model currentBest, Model newModel)
        {
            if (currentBest.fitness > newModel.fitness)
                return true;

            return false;
        }

    }
}
