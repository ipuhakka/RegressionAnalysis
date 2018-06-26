using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using RegressionAnalysis.ModelSelection;
using RegressionAnalysis.Evaluation;
using RegressionAnalysis.Exception;
using RegressionAnalysis.Converter;

namespace RegressionAnalysisTest
{   
    [TestFixture]
    class FullSelection_tests
    {
        [Test]
        public void test_constructor_fitness_R2Adjusted()
        {
            /*Test that constructor compiles with interface implementation as parameter, and
             * fitness parameter is of type AdjustedR2.*/
            Fitness adjR2 = new AdjustedR2();
            Variable y = new Variable("", new List<double>());
            Model m = new Model(y, new List<Variable>());
            FullSelection sel = new FullSelection(m, adjR2);
            Assert.True(sel.fitness.GetType() == typeof(AdjustedR2));
        }

        [Test]
        public void test_SelectBestFit()
        {
            /*R used to verify results: best fitted model based on adjusted coefficient of
             determination contains variables list3 and list4, with adjusted R^2 value of 0.6721534.*/
            Variable yList = new Variable("list", new List<double>() { 2.2, 3.5, 3, 14, 8, 2 });

            Variable list2 = new Variable("list2", new List<double>() { 3, 15.2, 1.1, 2, 3, 2 });
            Variable list3 = new Variable("list3", new List<double>() { 1, 2, 3, 4, 5, 2.2 });
            Variable list4 = new Variable("list4", new List<double>() { 1, 1.1, 1.4, 1.3, 1.5, 1.2 });
            List<Variable> xList = new List<Variable>() { list2, list3, list4 };

            Fitness adjR2 = new AdjustedR2();
            Model m = new Model(yList, xList);
            FullSelection sel = new FullSelection(m, adjR2);

            Model bestFit = sel.SelectBestFit();

            Assert.AreEqual(0.6721534, Math.Round(bestFit.fitness, 7));
            Assert.Contains(list3, bestFit.getXVars());
            Assert.Contains(list4, bestFit.getXVars());
            Assert.False(bestFit.getXVars().Contains(list2));
        }

        [Test]
        public void test_SelectBestFit_throws_MathError()
        {
            /*Matrix needs to be positive definite for using MathNets Fit.MultiDim()-function.
             This should fail and throw ArgumentException. This is noticed after the threads
             have finished because bestModels is be 0-length. Then MathError is thrown. */
            List<double> d1 = new List<double>();
            List<double> d2 = new List<double>();
            List<double> d3 = new List<double>();
            List<double> d4 = new List<double>();
            List<double> d5 = new List<double>();
            List<double> d6 = new List<double>();

            for (int i = 0; i < 1000; i++)
            {
                d1.Add(1);
                d2.Add(1);
                d3.Add(1);
                d4.Add(1);
                d5.Add(1);
                d6.Add(1);
            }

            Variable yList = new Variable("list", d1);
            Variable list2 = new Variable("list2", d2);
            Variable list3 = new Variable("list3", d3);
            Variable list4 = new Variable("list4", d4);
            Variable list5 = new Variable("list5", d5);
            Variable list6 = new Variable("list6", d6);
            List<Variable> x = new List<Variable>() { list2, list3, list4, list5, list6 };

            Fitness adjR2 = new AdjustedR2();
            Model m = new Model(yList, x);
            FullSelection sel = new FullSelection(m, adjR2);

            Assert.Throws<MathError>(() => sel.SelectBestFit());
        }

        [Test]
        public void test_SelectBestFit_performance_10000_observations_6_variables()
        {
            /*Test is done with a file containing 100 000 observations with 6 variables. */
            Directory.SetCurrentDirectory(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\"));

            Model m = CSV.ToModel(@"test_files\performance_test.csv", "ord.num");
            Fitness fitness = new AdjustedR2();
            FullSelection sel = new FullSelection(m, fitness);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            sel.SelectBestFit();
            sw.Stop();
            Console.WriteLine("took + " + sw.ElapsedMilliseconds + " milliseconds");
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 10000);
        }

    }
}
