using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace RegressionAnalysis.test
{   
    [TestFixture]
    class Selection_tests
    {
        [Test]
        public void test_constructor_fitness_R2Adjusted()
        {
            /*Test that constructor compiles with interface implementation as parameter, and
             * fitness parameter is of type AdjustedR2.*/
            Fitness adjR2 = new AdjustedR2();
            Variable y = new Variable("", new List<double>());

            Selection sel = new Selection(y, new List<Variable>(), adjR2);
            Assert.True(sel.fitness.GetType() == typeof(AdjustedR2));
        }

        [Test]
        public void test_SelectBestFit()
        {
            /*R used to verify results: best fitted model based on adjusted coefficient of
             determination contains variables list3 and list4, with adjusted R^2 value of 0.6721534.*/
            Variable list = new Variable("list", new List<double>() { 2.2, 3.5, 3, 14, 8, 2 });

            Variable list2 = new Variable("list2", new List<double>() { 3, 15.2, 1.1, 2, 3, 2 });
            Variable list3 = new Variable("list3", new List<double>() { 1, 2, 3, 4, 5, 2.2 });
            Variable list4 = new Variable("list4", new List<double>() { 1, 1.1, 1.4, 1.3, 1.5, 1.2 });
            List<Variable> x = new List<Variable>() { list2, list3, list4 };

            Fitness adjR2 = new AdjustedR2();
            Selection sel = new Selection(list, x, adjR2);

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

            Variable list = new Variable("list", d1);
            Variable list2 = new Variable("list2", d2);
            Variable list3 = new Variable("list3", d3);
            Variable list4 = new Variable("list4", d4);
            Variable list5 = new Variable("list5", d5);
            Variable list6 = new Variable("list6", d6);
            List<Variable> x = new List<Variable>() { list2, list3, list4, list5, list6 };

            Fitness adjR2 = new AdjustedR2();
            Selection sel = new Selection(list, x, adjR2);

            Assert.Throws<MathError>(() => sel.SelectBestFit());
        }

        [Test]
        public void test_SelectBestFit_performance_100000_observations_6_variables()
        {
            List<double> d1 = new List<double>();
            List<double> d2 = new List<double>();
            List<double> d3 = new List<double>();
            List<double> d4 = new List<double>();
            List<double> d5 = new List<double>();
            List<double> d6 = new List<double>();

            Random rand = new Random();
            for (int i = 0; i < 100000; i++)
            {
                d1.Add(rand.Next(0, 100));
                d2.Add(rand.Next(0, 100));
                d3.Add(rand.Next(0, 100));
                d4.Add(rand.Next(0, 100));
                d5.Add(rand.Next(0, 100));
                d6.Add(rand.Next(0, 100));
            }

            Variable list = new Variable("list", d1);
            Variable list2 = new Variable("list2", d2);
            Variable list3 = new Variable("list3", d3);
            Variable list4 = new Variable("list4", d4);
            Variable list5 = new Variable("list5", d5);
            Variable list6 = new Variable("list6", d6);
            List<Variable> x = new List<Variable>() { list2, list3, list4, list5, list6 };

            Fitness adjR2 = new AdjustedR2();
            Selection sel = new Selection(list, x, adjR2);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            sel.SelectBestFit();
            sw.Stop();
            Console.WriteLine("took + " + sw.ElapsedMilliseconds + " milliseconds");
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 5000);
        }

        [Test]
        public void test_SelectBestFit_performance_10000_observations_6_variables()
        {
            List<double> d1 = new List<double>();
            List<double> d2 = new List<double>();
            List<double> d3 = new List<double>();
            List<double> d4 = new List<double>();
            List<double> d5 = new List<double>();
            List<double> d6 = new List<double>();

            Random rand = new Random();
            for (int i = 0; i < 10000; i++)
            {
                d1.Add(rand.Next(0, 100));
                d2.Add(rand.Next(0, 100));
                d3.Add(rand.Next(0, 100));
                d4.Add(rand.Next(0, 100));
                d5.Add(rand.Next(0, 100));
                d6.Add(rand.Next(0, 100));
            }

            Variable list = new Variable("list", d1);
            Variable list2 = new Variable("list2", d2);
            Variable list3 = new Variable("list3", d3);
            Variable list4 = new Variable("list4", d4);
            Variable list5 = new Variable("list5", d5);
            Variable list6 = new Variable("list6", d6);
            List<Variable> x = new List<Variable>() { list2, list3, list4, list5, list6 };

            Fitness adjR2 = new AdjustedR2();
            Selection sel = new Selection(list, x, adjR2);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            sel.SelectBestFit();
            sw.Stop();
            Console.WriteLine("took + " + sw.ElapsedMilliseconds + " milliseconds");
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 1000);
        }

        [Test]
        [Ignore("Takes between 15-20 seconds, run only if performance needs to be checked")]
        public void test_SelectBestFit_performance_100000_observations_8_variables()
        {
            List<double> d1 = new List<double>();
            List<double> d2 = new List<double>();
            List<double> d3 = new List<double>();
            List<double> d4 = new List<double>();
            List<double> d5 = new List<double>();
            List<double> d6 = new List<double>();
            List<double> d7 = new List<double>();
            List<double> d8 = new List<double>();

            Random rand = new Random();
            for (int i = 0; i < 100000; i++)
            {
                d1.Add(rand.Next(0, 100));
                d2.Add(rand.Next(0, 100));
                d3.Add(rand.Next(0, 100));
                d4.Add(rand.Next(0, 100));
                d5.Add(rand.Next(0, 100));
                d6.Add(rand.Next(0, 100));
                d7.Add(rand.Next(0, 100));
                d8.Add(rand.Next(0, 100));
            }

            Variable list = new Variable("list", d1);
            Variable list2 = new Variable("list2", d2);
            Variable list3 = new Variable("list3", d3);
            Variable list4 = new Variable("list4", d4);
            Variable list5 = new Variable("list5", d5);
            Variable list6 = new Variable("list6", d6);
            Variable list7 = new Variable("list7", d7);
            Variable list8 = new Variable("list8", d8);
            List<Variable> x = new List<Variable>() { list2, list3, list4, list5, list6, list7, list8 };

            Fitness adjR2 = new AdjustedR2();
            Selection sel = new Selection(list, x, adjR2);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            sel.SelectBestFit();
            sw.Stop();
            Console.WriteLine("took + " + sw.ElapsedMilliseconds + " milliseconds");
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 20000);
        }


    }
}
