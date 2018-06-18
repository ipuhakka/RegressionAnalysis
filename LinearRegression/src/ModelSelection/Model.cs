using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace RegressionAnalysis.ModelSelection
{
    public class Model
    {
        [JsonProperty]
        public double fitness { get; set; }
        [JsonProperty]
        private Variable yVar;
        [JsonProperty]
        private List<Variable> xVars;

        /// <summary>
        /// Constructor for creating a Model-object. 
        /// </summary>
        /// <param name="responseVar">Name for model response variable.</param>
        /// <param name="explNames">List of names for explanatory variables in the model. Should
        /// be given in the same order as explanatory variable data is in the explData variable.</param>
        /// <param name="responseData">Values for reponse variable.</param>
        /// <param name="explData">Values for explanatory variables in the model. One list contains
        /// all observations for single variable.</param>
        /// <exception cref="ArgumentNullException">Thrown when any given parameter is null.</exception>
        public Model(Variable y, List<Variable> x)
        {
            if (y == null || x == null)
                throw new ArgumentNullException("Null values not allowed");

            fitness = 0.0;
            yVar = y;
            xVars = x.ToList();
        }

        public List<Variable> getXVars() { return xVars; }
        public Variable getYVar() { return yVar; }

        /// <summary>
        /// Creates and returns double lists for each variable in xVars.
        /// </summary>
        /// <returns>List of lists, each containing data for one variable.</returns>
        public List<List<double>> getXVariableLists()
        {
            List<List<double>> lists = new List<List<double>>();

            foreach(Variable x in xVars)
            {
                lists.Add(x.values);
            }

            return lists;
        }
    }
}
