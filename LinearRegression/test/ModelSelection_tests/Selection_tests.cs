using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RegressionAnalysis
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
        

    }
}
