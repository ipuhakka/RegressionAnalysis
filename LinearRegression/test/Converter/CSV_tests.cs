﻿using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using RegressionAnalysis.Converter;

namespace RegressionAnalysis.test
{
    [TestFixture]
    class CSV_tests
    {
        [OneTimeSetUp]
        public void setUp()
        {
            Directory.SetCurrentDirectory(Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\"));
        }

        [Test]
        public void test_ToVariableModel_empty_cell_throws_FormatException()
        {
            /*Providing an empty cell in a csv file should produce FormatException
             as double conversion should fail.*/
            string filePath = @"test_files/test_csv_fails_empty_cell.csv";
            Assert.Throws<FormatException>(() => CSV.ToVariableModel(filePath, "Var1"));
        }

        [Test]
        public void test_ToVariableModel_throws_InvalidCSVError()
        {
            /*File where row parameter count changes should throw InvalidCSVError.*/
            string filePath = @"test_files/test_csv_fails_tooFewRows.csv";
            Assert.Throws<InvalidCSVError>(() => CSV.ToVariableModel(filePath, "Var1"));
            filePath = @"test_files/test_csv_fails_tooManyRows.csv";
            Assert.Throws<InvalidCSVError>(() => CSV.ToVariableModel(filePath, "Var1"));
        }

        [Test]
        public void test_ToVariableModel_conversion_fail()
        {
            // trying to convert string that isn't a number into double should fail, function should throw
            //FormatException.
            string filePath = @"test_files/test_csv_failsConversion.csv";
            Assert.Throws<FormatException>(() => CSV.ToVariableModel(filePath, "Var1"));
        }

        [Test]
        public void test_ToVariableModel_Argumentexception()
        {
            /* ArgumentException is thrown when variable given as response is not found*/
            string filePath = @"test_files/test_csv_runs.csv";
            Assert.Throws<ArgumentException>(() => CSV.ToVariableModel(filePath, "Var"));
        }

        [Test]
        public void test_ToVariableModel_runs()
        {
            /*converting test_csv_runs.csv with "Var1" as response variable. */
            string filePath = @"test_files/test_csv_runs.csv";
            Tuple<Variable, List<Variable>> tuple = CSV.ToVariableModel(filePath, "Var3");

            Assert.AreEqual("Var3", tuple.Item1.name);
            Assert.AreEqual(3.3, tuple.Item1.values[0]);
        }

    }
}
