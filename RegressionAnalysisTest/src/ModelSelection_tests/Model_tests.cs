using System;
using System.Collections.Generic;
using NUnit.Framework;
using Newtonsoft.Json;
using RegressionAnalysis.ModelSelection;

namespace RegressionAnalysisTest
{
    [TestFixture]
    class Model_tests
    {

        [Test]
        public void test_constructor_empty_lists()
        {
            /*Object creation should work with empty list without throwing exceptions.*/
            Variable y = new Variable("", new List<double>() { });
            Variable x1 = new Variable("", new List<double>() { });
            Variable x2 = new Variable("", new List<double>() { });
            List<Variable> vars = new List<Variable>() { x1, x2 };

            Assert.DoesNotThrow(() => { Model model = new Model(y, vars); });           
        }

        [Test]
        public void test_constructor_null_lists()
        {
            /*Object creation with null list should throw ArgumentNullException.*/
            Variable y = new Variable("", new List<double>() { });
            Variable x1 = new Variable("", new List<double>() { });
            Variable x2 = new Variable("", new List<double>() { });
            List<Variable> vars = null;


            Assert.Throws<ArgumentNullException>(() => { Model model = new Model(y, vars); });
        }

        [Test]
        public void test_constructor_null_y()
        {
            /*Object creation with null y parameter should throw ArgumentNullExcpetion.*/
            Variable y = null;
            Variable x1 = new Variable("", new List<double>() { });
            Variable x2 = new Variable("", new List<double>() { });
            List<Variable> vars = new List<Variable>() { x1, x2 };

            Assert.Throws<ArgumentNullException>(() => { Model model = new Model(y, vars); });
        }

        [Test]
        public void test_JsonSerialization_Model()
        {
            /*Converting Model-object to json should produce object with only attributes marked with [JsonProperty]. */
            Variable y = new Variable("", new List<double>() { });
            Variable x1 = new Variable("", new List<double>() { });
            Variable x2 = new Variable("", new List<double>() { });
            List<Variable> vars = new List<Variable>() { x1, x2 };

            Model model = new Model(y, vars);

            string json = JsonConvert.SerializeObject(model);

            dynamic obj = JsonConvert.DeserializeObject(json);

            Assert.True(obj.yVar["name"] != null);
            Assert.True(obj.yVar["values"] == null);
            Assert.True(obj.xVars[0]["name"] != null);
            Assert.True(obj.xVars[0]["values"] == null);
            Assert.True(obj.xVars[1]["name"] != null);
            Assert.True(obj.xVars[1]["values"] == null);
        }

        [Test]
        public void test_getVariableLists()
        {
            Variable y = new Variable("y", new List<double>());
            Variable x1 = new Variable("", new List<double>() { 1, 2});
            Variable x2 = new Variable("", new List<double>() { 3, 4});
            List<Variable> vars = new List<Variable>() { x1, x2 };

            List<List<double>> expected = new List<List<double>>()
            {
                new List<double>() {1, 2}, new List<double>() {3, 4}
            };

            Model model = new Model(y, vars);

            Assert.AreEqual(expected, model.getXVariableLists());
        }
    }
}
