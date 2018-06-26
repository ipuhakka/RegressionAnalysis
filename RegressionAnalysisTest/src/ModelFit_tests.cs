using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using NUnit.Framework;
using RegressionAnalysis;
using RegressionAnalysis.Exception;

namespace RegressionAnalysisTest
{
    [TestFixture]
    class ModelFit_tests
    {
        List<double> list, list2, list3;

        [OneTimeSetUp]
        public void setUp()
        {
            list = new List<double>() { 2.2, 3.5, 3, 14, 8, 2 };
            list2 = new List<double>() { 3, 15.2, 1.1, 2, 3, 2 };
            list3 = new List<double>() { 1, 2, 3, 4, 5, 2.2 };
        }

        [OneTimeTearDown]
        public void tearDown()
        {
            list = null;
            list2 = null;
            list3 = null;
        }

        [Test]
        public void test_AdjustedR2_throws_MathError()
        {
            /* Trying to get coefficients for invalid matrix should produce ArgumentException.*/
            List<double> y = new List<double>();
            List<double> x1 = new List<double>();
            List<double> x2 = new List<double>();
            List<double> x3 = new List<double>();

            for (int i = 0; i < 100; i++)
            {
                y.Add(1);
                x1.Add(1);
                x2.Add(1);
                x3.Add(1);
            }

            List<List<double>> expl = new List<List<double>>() { x1, x2, x3};
            Assert.Throws<MathError>(() => ModelFit.AdjustedR2(y, expl));
        }

        [Test]
        public void test_FittedValues()
        {
            /*Tested on R, a model lm(list ~ list2 + list3)
             gives fitted values of 0.9818711, 3.5873076, 5.7066397, 8.1032177, 10.5016440, 3.8193199   */
            List<double> expected = new List<double>()
            { 0.9818711, 3.5873076, 5.7066397, 8.1032177, 10.5016440, 3.8193199 };
            List<List<double>> explanatory = new List<List<double>>() { list2, list3 };

            List<double> results = ModelFit.FittedValues(list, explanatory);
            results = results.Select(y => Math.Round(y, 7)).ToList();

            Assert.AreEqual(expected, results);
        }

        [Test]
        public void test_FittedValues_throws_MathError()
        {
            /*If any explanatory variable list is different size than y.Count, MathError is thrown.*/

            List<double> y = new List<double>() { 2, 3 };

            List<List<double>> lists = new List<List<double>>() { list, list2, list3 };

            Assert.Throws<MathError>(() => ModelFit.FittedValues(y, lists));
        }

        [Test]
        public void test_AdjustedR2_throws_MathError_Different_Sized_Lists()
        {
            /*Different sized lists throw MathError*/
            List<double> y = new List<double>() { 2, 3 };

            List<List<double>> lists = new List<List<double>>() { list, list2, list3 };

            Assert.Throws<MathError>(() => ModelFit.AdjustedR2(y, lists));
        }

        [Test]
        public void test_AdjustedR2_return_correct_result()
        {
            /*Calculated with R, lm (list ~ list2 + list3) produces an adjusted R2 value of 0.2081.*/
            List<List<double>> explanatory = new List<List<double>>() { list2, list3 };

            Assert.AreEqual(0.2081, Math.Round(ModelFit.AdjustedR2(list, explanatory), 4));
        }

        [Test]
        public void test_AdjustedR2_performance_10000_observations_2_variables()
        {
            Stopwatch sw = new Stopwatch();
            Random rand = new Random();
            List<double> y = new List<double>();
            List<double> x1 = new List<double>();
            List<double> x2 = new List<double>();

            for (int i = 0; i < 10000; i++)
            {
                y.Add(rand.Next(0, 10));
                x1.Add(rand.Next(0, 10));
                x2.Add(rand.Next(0, 10));
            }

            List<List<double>> expl = new List<List<double>>() { x1, x2 };

            sw.Start();
            ModelFit.AdjustedR2(y, expl);
            sw.Stop();
            Console.WriteLine("Took " + sw.ElapsedMilliseconds + " milliseconds");
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 1000);
        }

        [Test]
        public void test_AdjustedR2_performance_100000_observations_7_variables()
        {
            Stopwatch sw = new Stopwatch();
            Random rand = new Random();
            List<double> y = new List<double>();
            List<double> x1 = new List<double>();
            List<double> x2 = new List<double>();
            List<double> x3 = new List<double>();
            List<double> x4 = new List<double>();
            List<double> x5 = new List<double>();
            List<double> x6 = new List<double>();
            List<double> x7 = new List<double>();

            for (int i = 0; i < 100000; i++)
            {
                y.Add(rand.Next(0, 10));
                x1.Add(rand.Next(0, 10));
                x2.Add(rand.Next(0, 10));
                x3.Add(rand.Next(0, 10));
                x4.Add(rand.Next(0, 10));
                x5.Add(rand.Next(0, 10));
                x6.Add(rand.Next(0, 10));
                x7.Add(rand.Next(0, 10));
            }

            List<List<double>> expl = new List<List<double>>() { x1, x2, x3, x4, x5, x6, x7 };

            sw.Start();
            ModelFit.AdjustedR2(y, expl);
            sw.Stop();
            Console.WriteLine("Took " + sw.ElapsedMilliseconds + " milliseconds");
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 1500);
        }

        [Test]
        public void test_AIC()
        {
            /*Result calculated with R.*/
            List<double> y = new List<double>() { 1, 2.2, 3.1, 2.5 };
            List<double> x1 = new List<double>() { 177, 175, 183, 167 };
            List<double> x2 = new List<double>() { 3, 5, 6, 9 };
            List<List<double>> x = new List<List<double>>() { x1, x2 };

            Assert.AreEqual(-4.032542, Math.Round(ModelFit.AIC(y, x), 6));
        }

    }
}
