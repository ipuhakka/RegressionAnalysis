using System;
using System.Collections.Generic;
using NUnit.Framework;
using Newtonsoft.Json;
using RegressionAnalysis.ModelSelection;

namespace RegressionAnalysisTest
{
    [TestFixture]
    class Variable_tests
    {
        [Test]
        public void test_constructor_empty_lists()
        {
            /*Object creation should work with empty arguments without throwing exceptions.*/
            Assert.DoesNotThrow(() => {
                Variable y = new Variable("", new List<double>() { });
                Variable x1 = new Variable("", new List<double>() { });
                Variable x2 = new Variable("", new List<double>() { });
            });
        }

        [Test]
        public void test_constructor_null_lists()
        {
            /*Object creation with null lists should throw ArgumentNullException.*/
            Assert.Throws<ArgumentNullException>(() => {
                Variable y = new Variable("", null);
                Variable x1 = new Variable("", null);
                Variable x2 = new Variable("", null);
            });
        }

        [Test]
        public void test_constructor_null_string()
        {
            /*Object creation with null string parameter should throw ArgumentNullExcpetion.*/
            Assert.Throws<ArgumentNullException>(() => {
                Variable y = new Variable(null, new List<double>());
                Variable x1 = new Variable(null, new List<double>());
                Variable x2 = new Variable(null, new List<double>());
            });
        }

        [Test]
        public void test_JsonSerialization()
        {
            /*Converting Model-object to json should produce object with only attributes marked with [JsonProperty]. */
            Variable x = new Variable("", new List<double>());

            string json = JsonConvert.SerializeObject(x);

            dynamic obj = JsonConvert.DeserializeObject(json);

            Assert.True(obj["name"] != null);
            Assert.True(obj["values"] == null);
        }
    }
}
