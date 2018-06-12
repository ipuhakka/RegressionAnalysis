using System;

namespace RegressionAnalysis
{
    public class MathError: Exception
    {
        public string error;

        public MathError(string error_message)
        {
            error = error_message;
        }

    }
}
