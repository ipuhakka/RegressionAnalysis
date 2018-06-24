using System;
using System.Diagnostics;
using System.Collections.Generic;
using NUnit.Framework;
using RegressionAnalysis.ModelSelection;
using RegressionAnalysis.Evaluation;
using RegressionAnalysis.Exception;
using System.Linq;

namespace RegressionAnalysisTest.ModelSelection_tests
{
    [TestFixture]
    class BackwardElimination_tests
    {

        [Test]
        public void test_FindBestModel_correctResult()
        {
            Variable list = new Variable("list", new List<double>() { 2.2, 3.5, 3, 14, 8, 2 });

            Variable list2 = new Variable("list2", new List<double>() { 3, 15.2, 1.1, 2, 3, 2 });
            Variable list3 = new Variable("list3", new List<double>() { 1, 2, 3, 4, 5, 2.2 });
            Variable list4 = new Variable("list4", new List<double>() { 1, 1.1, 1.4, 1.3, 1.5, 1.2 });
            List<Variable> x = new List<Variable>() { list2, list3, list4 };
            Fitness fitness = new AdjustedR2();
            Model testModel = new Model(list, x);

            BackwardElimination be = new BackwardElimination();
            Model bestModel = be.FindBestModel(testModel, fitness);

            Assert.AreEqual(2, bestModel.getXVars().Count);
            Assert.That(bestModel.getXVars().Any(v => v.name == "list3"));
            Assert.That(bestModel.getXVars().Any(v => v.name == "list4"));
        }

        [Test]
        public void test_FindBestModel_throws_MathError()
        {
            /*Different sized lists should throw MathError.*/
            Variable list = new Variable("list", new List<double>() { 2.2, 3.5, 3, 14, 8, 2 });

            Variable list2 = new Variable("list2", new List<double>() { 3, 15.2, 1.1, 2, 2 });
            Variable list3 = new Variable("list3", new List<double>() { 1, 2, 3, 4, 5, 2.2 });
            Variable list4 = new Variable("list4", new List<double>() { 1, 1.1, 1.4, 1.3, 1.5, 1.2 });
            List<Variable> x = new List<Variable>() { list2, list3, list4 };
            Fitness fitness = new AdjustedR2();
            Model testModel = new Model(list, x);

            BackwardElimination be = new BackwardElimination();
            Assert.Throws<MathError>(() => be.FindBestModel(testModel, fitness));
        }

        [Test]
        public void test_FindBestModel_Throws_MathError()
        {
            /*Incorrect matrix throws MathError in ModelFit.GetCoefficients.*/
            Variable list = new Variable("list", new List<double>() { 1,1,1,1,1 });

            Variable list2 = new Variable("list2", new List<double>() { 1, 1, 1, 1, 1 });
            Variable list3 = new Variable("list3", new List<double>() { 1, 1, 1, 1, 1 });
            Variable list4 = new Variable("list4", new List<double>() { 1, 1, 1, 1, 1 });
            List<Variable> x = new List<Variable>() { list2, list3, list4 };
            Fitness fitness = new AdjustedR2();
            Model testModel = new Model(list, x);

            BackwardElimination be = new BackwardElimination();
            Assert.Throws<MathError>(() => be.FindBestModel(testModel, fitness));
        }

        [Test]
        public void test_FindBestModel_Performance()
        {
            List<double> d1 = new List<double>();
            List<double> d2 = new List<double>();
            List<double> d3 = new List<double>();
            List<double> d4 = new List<double>();
            List<double> d5 = new List<double>();
            List<double> d6 = new List<double>();
            List<double> d7 = new List<double>();
            List<double> d8 = new List<double>();
            List<double> d9 = new List<double>();
            List<double> d10 = new List<double>();
            List<double> d11 = new List<double>();
            List<double> d12 = new List<double>();
            List<double> d13 = new List<double>();
            List<double> d14 = new List<double>();

            Random rand = new Random();
            for (int i = 0; i < 10000; i++)
            {
                d1.Add(rand.Next(0, 100));
                d2.Add(rand.Next(0, 100));
                d3.Add(rand.Next(0, 100));
                d4.Add(rand.Next(0, 100));
                d5.Add(rand.Next(0, 100));
                d6.Add(rand.Next(0, 100));
                d7.Add(rand.Next(0, 100));
                d8.Add(rand.Next(0, 100));
                d9.Add(rand.Next(0, 100));
                d10.Add(rand.Next(0, 100));
                d11.Add(rand.Next(0, 100));
                d12.Add(rand.Next(0, 100));
                d13.Add(rand.Next(0, 100));
                d14.Add(rand.Next(0, 100));
            }

            Variable list = new Variable("list", d1);
            Variable list2 = new Variable("list2", d2);
            Variable list3 = new Variable("list3", d3);
            Variable list4 = new Variable("list4", d4);
            Variable list5 = new Variable("list5", d5);
            Variable list6 = new Variable("list6", d6);
            Variable list7 = new Variable("list7", d7);
            Variable list8 = new Variable("list8", d8);
            Variable list9 = new Variable("list9", d9);
            Variable list10 = new Variable("list10", d10);
            Variable list11 = new Variable("list11", d11);
            Variable list12 = new Variable("list12", d12);
            Variable list13 = new Variable("list13", d13);
            Variable list14 = new Variable("list14", d14);
            List<Variable> x = new List<Variable>() { list2, list3, list4, list5, list6, list7, list8, list9, list10, list11, list12, list13, list14};
            Model model = new Model(list, x);

            Fitness adjR2 = new AdjustedR2();
            BackwardElimination be = new BackwardElimination();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            be.FindBestModel(model, adjR2);
            sw.Stop();
            Console.WriteLine("took + " + sw.ElapsedMilliseconds + " milliseconds");
            Assert.LessOrEqual(sw.ElapsedMilliseconds, 4000);
        }

    }
}
