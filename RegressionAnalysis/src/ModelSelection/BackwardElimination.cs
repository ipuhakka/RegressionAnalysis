using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using RegressionAnalysis.Evaluation;
using RegressionAnalysis.Exception;

namespace RegressionAnalysis.ModelSelection
{
    public class BackwardElimination
    {
        private Fitness criteria;

        /// <summary>
        /// Function finds the best model according to an evaluation criteria. Search starts from 
        /// full model, narrows the model by one, and if any n-1 sized submodel provides
        /// improvement, it's chosen as top model and search iterates again starting from that 
        /// submodel.
        /// </summary>
        /// <param name="fullModel">Model containing all explanatory variables.</param>
        /// <param name="fitness">Object of the class which is used as an evaluation criteria.</param>
        /// <returns>Model that best explains the response variability according to the fitness
        /// criteria.</returns>
        /// <exception cref="MathError">Thrown when parameter lists are of different length.</exception>
        /// <exception cref="ArgumentException">Thrown when matrix created from parameter fullModel is not valid to fit least square points.</exception>
        public Model FindBestModel(Model fullModel, Fitness fitness)
        {
            /*Calculate fitness for full model, go through all n - 1, if any is better than 
             * original, select the best and iterate over again. Stop when a better model
             * was not found in the next round. */
            if (fitness == null || fullModel == null || fullModel.getXVars().Count == 0)
                throw new MathError("Insufficient parameters");

            Model bestModel = fullModel;
            criteria = fitness;
            bestModel.fitness = fitness.EvaluateFitness(bestModel);

            int n = fullModel.getXVars().Count;

            while (n > 0)
            {
                Model prunedModel = bestSubModel(bestModel);

                if (prunedModel.fitness < bestModel.fitness)
                    break;
                else
                    bestModel = prunedModel;
                n--;
            }
            return bestModel;
        }

        /// <summary>
        /// returns the model with best fitness, where one variable has been dropped from 
        /// the parameter model.
        /// </summary>
        /// <param name="model">Model from which the reduction of single variable
        /// is conducted.</param>
        /// <returns>Best fitness model which has one variable dropped from the parameter model.</returns>
        private Model bestSubModel(Model model)
        {
            int n = model.getXVars().Count;
            List<Model> models = new List<Model>();
            Model bestModel;

            for (int i = 0; i < n; i++)
            {
                models.Add(model.Clone());
                models[i].getXVars().RemoveAt(i);
                models[i].fitness = criteria.EvaluateFitness(models[i]);
            }
            bestModel = models[0];

            foreach (Model m in models)
            {
                if (m.fitness > bestModel.fitness)
                    bestModel = m;
            }
            return bestModel;
        }

    }
}
