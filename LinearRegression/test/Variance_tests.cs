using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace RegressionAnalysis
{
    [TestFixture]
    class Variance_tests
    {
        List<double> testList;
        List<double> testList2;

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
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            testList = null;
            testList2 = null;
        }

        [Test]
        public void test_StandardDeviation_calculates_correct_result()
        {
            /*Test result for testList calculated with R = 5.575766. Test rounds it up to 4 decimals*/
            Assert.AreEqual(Math.Round(5.575766, 4), Math.Round(Variance.StandardDeviation(testList), 4));
        }

        [Test]
        public void test_StandardDeviation_throws_MathError_with_null_list()
        {
            Assert.Throws<MathError>(() => Variance.StandardDeviation(null));
        }

        [Test]
        public void test_StandardDeviation_throws_MathError_with_0_items_list()
        {
            /*Zero items in list should return 0 standard deviation. List is cleared and after 
             assertion it is restored.*/
            testList.Clear();

            Assert.Throws<MathError>(() => Variance.StandardDeviation(testList));

            testList.Add(2.2);
            testList.Add(3.5);
            testList.Add(3);
            testList.Add(14);
        }

        [Test]
        public void test_Covariance_null_lists_throws_MathError() {
            /*If either of Covariance parameter lists are null, MathError should be thrown
             with error "Parameter list cannot be a null item"*/

            MathError err = Assert.Throws<MathError>(() => Variance.Covariance(testList, null));
            Assert.AreEqual("Parameter list cannot be a null item", err.error);
        }

        [Test]
        public void test_Covariance_0_Count_lists_throws_MathError()
        {
            /*If both list are of 0 length, MathError should be thrown with error "Parameter lists have 0 items"*/
            List<double> list1 = new List<double>();
            List<double> list2 = new List<double>();
            MathError err = Assert.Throws<MathError>(() => Variance.Covariance(list1, list2));
            Assert.AreEqual("Parameter lists have 0 items", err.error);

            list1 = null;
            list2 = null;
        }

        [Test]
        public void test_Covariance_different_Counts_throw_MathError()
        {
            /*MathError with error message "Variable lists not the same length" should be thrown.*/
            List<double> list1 = new List<double>();
            list1.Add(2.2);

            MathError err = Assert.Throws<MathError>(() => Variance.Covariance(testList, list1));
            Assert.AreEqual("Variable lists not the same length", err.error);
        }

        [Test]
        public void test_Covariance_returns_correct_result()
        {
            /*Covariance for testList and testList2 calculated with R: -9.925833.
             For test result is rounded to four decimals.*/

            Assert.AreEqual(Math.Round(-9.9258, 4), Math.Round(Variance.Covariance(testList, testList2), 4));
        }

    }
}
