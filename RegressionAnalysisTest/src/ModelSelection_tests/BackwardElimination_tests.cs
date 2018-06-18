﻿using System.Collections.Generic;
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

    }
}
