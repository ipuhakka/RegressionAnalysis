﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RegressionAnalysis.ModelSelection
{
    public class Variable
    {
        public string name { get; }
        [JsonIgnore]
        public List<double> values { get; }

        public Variable(string name_par, List<double> values_par)
        {
            if (name_par == null || values_par == null)
                throw new ArgumentNullException("Variable object can't have null parameters.");

            name = name_par;
            values = values_par;
        }
    }
}
