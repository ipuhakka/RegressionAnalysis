using System;
using System.Collections.Generic;
using NUnit.Framework;
using MathNet.Numerics.LinearAlgebra;
using RegressionAnalysis;

namespace RegressionAnalysisTest
{
    [TestFixture]
    class Matrix_tests
    {
        Matrix<double> testMatrix;

        [OneTimeSetUp]
        public void SetUp()
        {
            var M = Matrix<double>.Build;

            double[,] x = {{ 1.0, 3 },
               { 2.0, 4.0 }};
           testMatrix = M.DenseOfArray(x);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            testMatrix = null;
        }

        [Test]
        public void testConvert()
        {
            /*Creating a list of two lists, list1 = 1.0, 2.0 and list2 = 3.0, 4.0
             and converting it into Matrix should equal to testMatrix.*/

            List<double> list1 = new List<double>() { 1.0, 2.0 };
            List<double> list2 = new List<double>() { 3.0, 4.0 };

            List<List<double>> columns = new List<List<double>>() { list1, list2};

            Assert.AreEqual(testMatrix, Matrix.Convert(columns));
        }

        [Test]
        public void testConvert_throws_ArgumentOutOfRange()
        {
            /*MathNet Conversion should catch ArgumentOutOfRange exceptions 
            when number of rows of a matrix isn't positive.*/

            List<double> list1 = new List<double>();
            List<double> list2 = new List<double>();

            List<List<double>> columns = new List<List<double>>() { list1, list2 };

            Assert.Throws<ArgumentOutOfRangeException>(() => Matrix.Convert(columns));
        }

        [Test]
        public void testConvert_throws_ArgumentException_differentSizedLists()
        {
            /*MathNet Conversion should catch ArgumentException 
            when number of rows of a matrix isn't the same.*/
            List<double> list1 = new List<double>();
            List<double> list2 = new List<double>();
            List<double> list3 = new List<double>();
            List<double> list4 = new List<double>();

            for (int i = 0; i < 10000; i++)
            {
                list1.Add(2);
                list2.Add(3);
            }

            for (int j = 0; j < 9999; j++)
            {
                list3.Add(2);
                list4.Add(3);
            }
            List<List<double>> columns = new List<List<double>>() { list1, list2, list3, list4 };

            Assert.Throws<ArgumentException>(() => Matrix.Convert(columns));
        }

        [Test]
        public void test_InvertVariableList1List()
        {
            List<double> list1 = new List<double>() { 1, 2, 4, 3 };
            List<List<double>> lists = new List<List<double>>() { list1 };

            Assert.DoesNotThrow(() => Matrix.InvertVariableList(lists));
        }

        [Test]
        public void testInvertVariableList()
        {
            List<double> list1 = new List<double>() { 1, 2, 4, 3 };
            List<double> list2 = new List<double>() { 1, 2, 3, 5 };
            List<double> list3 = new List<double>() { 1, 2, 3, 2.2 };
            List<List<double>> lists = new List<List<double>>() { list1, list2, list3 };

            double[][] jArray = new double[4][];
            jArray[0] = new double[3] { 1, 1, 1};
            jArray[1] = new double[3] { 2, 2, 2 };
            jArray[2] = new double[3] { 4, 3, 3 };
            jArray[3] = new double[3] { 3, 5, 2.2 };

            Assert.AreEqual(jArray, Matrix.InvertVariableList(lists));
        }

        [Test]
        public void testInvertVariableListThrows_ArgumentExcpetion()
        {
            /*Different size lists should throw ArgumentExceptionError.*/

            List<double> list1 = new List<double>() { 1, 2 };
            List<double> list2 = new List<double>() { 1, 2 };
            List<double> list3 = new List<double>() { 1, 2, 3 };

            List<List<double>> lists = new List<List<double>>() { list1, list2, list3 };

            Assert.Throws<ArgumentException>(() => Matrix.InvertVariableList(lists));
        }

    }
}
