using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegressionAnalysis
{
    class InvalidCSVError: Exception
    {
        public string error;

        public InvalidCSVError(string err_message)
        {
            error = err_message;
        }
    }
}
