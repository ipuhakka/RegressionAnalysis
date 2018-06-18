
namespace RegressionAnalysis.Exception
{
    public class InvalidCSVError: System.Exception
    {
        public string error;

        public InvalidCSVError(string err_message)
        {
            error = err_message;
        }
    }
}
