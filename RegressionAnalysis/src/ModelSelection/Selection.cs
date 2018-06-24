using System;
using System.Collections.Generic;
using System.Threading;
using RegressionAnalysis.Evaluation;
using RegressionAnalysis.Exception;


namespace RegressionAnalysis.ModelSelection
{
    /// <summary>
    /// Class is used to select the most suitable model out of all possible combinations of
    /// explanatory variables, based on a fitness value. Class divides models into subgroups, and each
    /// subgroups maximum fitness is found in its own thread. This provides a somewhat better performance
    /// with large data sets with more than 6 explanatory variables.
    /// </summary>
    public class Selection
    {
        private const int MODELS_PER_THREAD = 32;
        private List<Model> models;
        private List<Model> bestModels;
        private Model baseModel;
        public Fitness fitness { get; set; }

        /// <summary>
        /// Constructor for creating Selection-object for when data is already in program memory.
        /// </summary>
        /// <param name="x">List of explanatory variables.</param>
        /// <param name="y">Response variable.</param>
        /// <param name="fitness_param">Implementation of Fitness interface.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Selection(Model model, Fitness fitness_param)
        {
            if (model == null || fitness_param == null)
                throw new ArgumentNullException("Constructor doesn't accept null parameters.");

            models = new List<Model>();
            bestModels = new List<Model>();
            baseModel = model.Clone();
            fitness = fitness_param;
        }

        /// <summary>
        /// Function selects the best fit model from explanatory varibles xVars, based on fitness attribute
        /// fitness.
        /// </summary>
        /// <returns>A model object which has the highest fitness value.</returns>
        /// <exception cref="MathError">Thrown when evaluating fitness has failed, due to
        /// different length variable lists, or invalid matrix type.</exception>
        public Model SelectBestFit()
        {
            SetCombinations(baseModel.getXVars());
            LaunchThreads();
            try
            {
                return BestFit();
            }
            catch (MathError e)
            {
                throw e;
            } 
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
                    models.Add(new Model(baseModel.getYVar(), combination));
            }

        }

        /// <summary>
        /// Launches all threads to calculate their own groups max fitness value. Threads are used 
        /// for better performance on larger lists.
        /// </summary>
        private void LaunchThreads()
        {
            int threadCount = Convert.ToInt32(Math.Ceiling(models.Count / (double)MODELS_PER_THREAD));
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < threadCount; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(BestSubFit));
                threads.Add(t);
                t.Start(i);
            }

            foreach (Thread t in threads)
            {
                t.Join();
            } 

            return;
        }

        /// <summary>
        /// Finds the best fit from the list containing best models.
        /// </summary>
        /// <returns>Model with maximum fitness.</returns>
        /// <exception cref="MathError">Thrown when evaluating fitness for models has failed.</exception>
        private Model BestFit()
        {
            if (bestModels.Count <= 0)
                throw new MathError("Calculating fitness values for models failed. Either different " +
                    "length variables where given, or matrix constructed from a model was invalid.");

            Model maxFitness = bestModels[0];

            foreach (Model model in bestModels)
            {
                if (fitness.IsBetter(maxFitness, model))
                    maxFitness = model;
            }
            return maxFitness;
        }

        /// <summary>
        /// Adds the best model from subgroup of models to bestModels. 
        /// </summary>
        /// <param name="obj">Integer, used to calculate what subgroup of variable models fitness values are calculated.</param>
        /// <exception cref="MathError">Thrown when parameter lists given where of different length.</exception>
        /// <exception cref="ArgumentException">Thrown when fitting least square points on a created matrix fails.</exception>
        private void BestSubFit(object obj)
        {
            int start = (int)obj * MODELS_PER_THREAD;
            int end = start + MODELS_PER_THREAD;

            if (end >= models.Count)
                end = models.Count;

            Model maxFitness = models[start];

            for (int i = start; i < end; i++)
            {
                models[i].fitness = fitness.EvaluateFitness(models[i]);

                if (fitness.IsBetter(maxFitness, models[i]))
                    maxFitness = models[i];
            }
            bestModels.Add(maxFitness);
        }
    }
}
