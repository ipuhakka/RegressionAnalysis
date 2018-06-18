using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using RegressionAnalysis;
using RegressionAnalysis.Exception;

namespace RegressionAnalysisTest
{
    [TestFixture]
    class Regression_tests
    {
        List<double> testList;
        List<double> testList2;
        List<double> testList3;

        [OneTimeSetUp]
        public void SetUp()
        {
            testList = new List<double>();
            testList.Add(2.2);
            testList.Add(3.5);
            testList.Add(3);
            testList.Add(14);

            testList2 = new List<double>();
            testList2.Add(3);
            testList2.Add(15.2);
            testList2.Add(1.1);
            testList2.Add(2);

            testList3 = new List<double>();
            testList3.Add(1);
            testList3.Add(2);
            testList3.Add(3);
            testList3.Add(4);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            testList = null;
            testList2 = null;
            testList3 = null;
        }

        [Test]
        public void test_BetaEstimate_different_Counts()
        {
            /*MathError with error message "Variable lists not the same length" should be thrown.
             Since only one list is of length 0 this should produce different lengths error */
            List<double> list1 = new List<double>();
            MathError err = Assert.Throws<MathError>(() => Regression.BetaEstimate(testList, list1));
            Assert.AreEqual("Variable lists not the same length", err.error);
        }

        [Test]
        public void test_BetaEstimate_null_list_throws_MathError()
        {
            /*If either of parameter lists are null, MathError should be thrown
             with error "Parameter list cannot be a null item"*/

            MathError err = Assert.Throws<MathError>(() => Regression.BetaEstimate(testList, null));
            Assert.AreEqual("Parameter list cannot be a null item", err.error);
        }

        [Test]
        public void test_BetaEstimate_0_Count_throws_MathError()
        {
            /*If both list are of 0 length, MathError should be thrown with error "Parameter lists have 0 items"*/
            List<double> list1 = new List<double>();
            List<double> list2 = new List<double>();
            MathError err = Assert.Throws<MathError>(() => Regression.BetaEstimate(list1, list2));
            Assert.AreEqual("Parameter lists have 0 items", err.error);

            list1 = null;
            list2 = null;
        }

        [Test]
        public void test_BetaEstimate_returns_correct_result()
        {
            /*B1-coefficient for linear model where testList is the response variable and testList2
             is the explanatory variable lm(testList ~ testList2) is -0.2259 (Calculated with R).
             That means that for every unit testList2 value is elevated, testList value declines by 
             -0,2259. Result rounded to 4 decimals in the test*/

            Assert.AreEqual(-0.2259, Math.Round(Regression.BetaEstimate(testList, testList2), 4));
        }

        [Test]
        public void test_BetaEstimates_returns_correct_values()
        {
            /*BetaEstimates for model lm(testlist ~ testlist2 + testlist3)
             should be: B0 = -3.0551, B1=0.0005, B2 = 3.4909. Calculated with R, lm-function.*/

            List<List<double>> X = new List<List<double>>() { testList2, testList3};

            List<double> expected = new List<double>() { -3.0551, 0.0005, 3.4909 };
            List<double> results = Regression.BetaEstimates(testList, X);

            results = results.Select(y => Math.Round(y, 4)).ToList();
            Assert.AreEqual(expected, results);

        }

        [Test]
        public void test_BetaEstimates_returns_correct_values_3_lists()
        {
            /*lm(testList ~ a + b + c). R calculated estimates:
                B0=-204.7130 B1=-0.3261 B2=114.4783 B3=5.4565 */

            List<double> a = new List<double>() { 1, 2, 3.5, 3.2 };
            List<double> b = new List<double>() { 1, 1.1, 1.2, 1.3 };
            List<double> c = new List<double>() { 17, 15.2, 13.1, 13.0 };

            List<List<double>> X = new List<List<double>>() { a, b, c };

            List<double> expected = new List<double>() { -204.7130, -0.3261, 114.4783, 5.4565 };
            List<double> results = Regression.BetaEstimates(testList, X);

            results = results.Select(y => Math.Round(y, 4)).ToList();
            Assert.AreEqual(expected, results);
        }

        [Test]
        public void test_BetaEstimates_different_Count_lists_throws_ArgumentException()
        {
            List<double> a = new List<double>() { 1, 2};
            List<double> b = new List<double>() { 1, 1.1, 1.2, 1.3 };
            List<double> c = new List<double>() { 17, 15.2, 13.1, 13.0 };

            List<List<double>> X = new List<List<double>>() { a, b, c };

            Assert.Throws<ArgumentException>(() => Regression.BetaEstimates(testList, X));
        }

        [Test]
        public void test_BetaEstimates_null_List_throws_ArgumentException()
        {
            /*Tests that both y and x null parameters are caught.*/
            List<double> a = new List<double>() { 1, 1.1, 1.2, 1.3 };
            List<double> b = new List<double>() { 17, 15.2, 13.1, 13.0 };

            List<List<double>> X = new List<List<double>>() { null, a, b };
            Assert.Throws<MathError>(() => Regression.BetaEstimates(testList, X));

            X.RemoveAt(0);
            Assert.Throws<MathError>(() => Regression.BetaEstimates(null, X));
        }

        [Test]
        public void test_AlphaEstimateSingle_null_list_throws_MathError()
        {
            Assert.Throws<MathError>(() => Regression.AlphaEstimateSingle(null, testList));
        }

        [Test]
        public void test_AlphaEstimateSingle_different_counts_throws_MathError()
        {
            testList.Add(1);
            Assert.Throws<MathError>(() => Regression.AlphaEstimateSingle(testList2, testList));
            testList.RemoveAt(testList.Count - 1);
        }

        [Test]
        public void test_AlphaEstimateSingle_return_correct_value()
        {
            /*lm(testList ~ testList2) returns β0 = 6.8778. Calculated with R*/

            Assert.AreEqual(6.8778, Math.Round(Regression.AlphaEstimateSingle(testList, testList2), 4));
            Assert.AreNotEqual(6.8779, Math.Round(Regression.AlphaEstimateSingle(testList, testList2), 4));
        }
    }
}
