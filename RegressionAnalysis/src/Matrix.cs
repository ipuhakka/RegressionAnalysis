using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace RegressionAnalysis
{
    public class Matrix
    {

        /// <summary>
        /// Creates a matrix from parameter columns given in a list of lists. 
        /// Each list is one column.
        /// </summary>
        /// <param name="columns"></param>
        /// <returns>Matrix object constructed from columns.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when list lengths arent positive.</exception>
        /// <exception cref="ArgumentException">Thrown when columns can't be converted into a matrix. </exception>
        public static Matrix<double> Convert(List<List<double>> columns)
        {
            var M = Matrix<double>.Build;
            var matrix = M.DenseOfColumns(columns);
            return matrix;
        }

        /// <summary>
        /// This function converts a list of lists, where each list holds data for one variable, into
        /// a jagged array where innerarray holds data points for a single observation. 
        /// </summary>
        /// <param name="x">List of double lists, each containing data for a single variable.</param>
        /// <returns>double[][] which holds a single observations data points in an inner array.</returns>
        /// <exception cref="ArgumentException">Thrown when lists in parameter x are of different length.</exception>
        public static double[][] InvertVariableList(List<List<double>> x)
        {
            double[][] columns = new double[x[0].Count][];

            int column = x.Count;
            int count = x[0].Count;

            foreach (List<double> list in x)
            {
                if (list.Count != count)
                    throw new ArgumentException("Different length lists");
            }

            for (int j = 0; j < count; j++)
            {
                columns[j] = new double[column];
                for (int k = 0; k < x.Count; k++)
                {
                    columns[j][k] = x[k][j];
                }
            }

            return columns;
        }

    }
}
