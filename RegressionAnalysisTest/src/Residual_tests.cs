using System;
using System.Collections.Generic;
using RegressionAnalysis;
using NUnit.Framework;

namespace RegressionAnalysisTest
{
    [TestFixture]
    class Residual_tests
    {
        [Test]
        public void test_SumOfSquaredResiduals()
        {
            /*Calculated with R, result is 0.19753.*/
            List<double> y = new List<double>() { 1, 2.2, 3.1, 2.5 };
            List<double> x1 = new List<double>() { 177, 175, 183, 167 };
            List<double> x2 = new List<double>() { 3, 5, 6, 9 };
            List<List<double>> x = new List<List<double>>() { x1, x2 };

            Assert.AreEqual(0.19753, Math.Round(Residual.SumOfSquaredResiduals(y, x), 5));
        }
    }
}
