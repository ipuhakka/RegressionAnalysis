
namespace RegressionAnalysis.Exception
{
    public class MathError: System.Exception
    {
        public string error;

        public MathError(string error_message)
        {
            error = error_message;
        }

    }
}
