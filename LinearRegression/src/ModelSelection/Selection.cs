using System;
using System.Collections.Generic;
using System.Linq;


namespace RegressionAnalysis
{
    /// <summary>
    /// Class is used to select the most suitable model out of all possible combinations of
    /// explanatory variables, based on a fitness value.
    /// </summary>
    class Selection
    {
        private List<Model> models;
        private Variable yVar;
        private List<Variable> xVars;
        public Fitness fitness { get; set; }

        /// <summary>
        /// Constructor for creating Selection-object for when data is already in program memory.
        /// </summary>
        /// <param name="x">List of explanatory variables.</param>
        /// <param name="y">Response variable.</param>
        /// <param name="fitness_param">Implementation of Fitness interface.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Selection(Variable y, List<Variable> x, Fitness fitness_param)
        {
            if (x == null || y == null || fitness_param == null)
                throw new ArgumentNullException("Constructor doesn't accept null parameters.");

            models = new List<Model>();
            yVar = y;
            xVars = x.ToList();
            fitness = fitness_param;
        }

        /// <summary>
        /// Function selects the best fit model from explanatory varibles xVars, based on fitness attribute
        /// fitness.
        /// </summary>
        /// <returns>A model object which has the highest fitness value.</returns>
        public Model SelectBestFit()
        {
            SetCombinations(xVars);
            return BestFit();
        }

        /// <summary>
        /// Adds a model for each possible combination of explanatory variables. 
        /// </summary>
        /// <param name="variables"></param>
        /// <returns>Number of model combinations created.</returns>
        private void SetCombinations(List<Variable> variables)
        {
            double count = Math.Pow(2, variables.Count);

            for (int i = 0; i < count; i++)
            {
                string str = Convert.ToString(i, 2).PadLeft(variables.Count, '0');
                List<Variable> combination = new List<Variable>();
                for (int j = 0; j < str.Length; j++)
                {                  
                    if (str[j] == '1')
                    {
                        combination.Add(variables[j]);
                    }
                }
                if (combination.Count > 0)
                    models.Add(new Model(yVar, combination));
            }

        }

        /// <summary>
        /// Calculates fitness value for a model and returns the model with max fitness.
        /// </summary>
        /// <returns>Model with maximum fitness.</returns>
        private Model BestFit()
        {
            Model maxFitness = models[0];

            foreach (Model model in models)
            {
                model.fitness = fitness.EvaluateFitness(model.getYVar().values, model.getXVariableLists());

                if (model.fitness > maxFitness.fitness)
                    maxFitness = model;
            }
            return maxFitness;
        }
    }
}
