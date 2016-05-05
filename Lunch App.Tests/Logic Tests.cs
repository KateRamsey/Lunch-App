using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lunch_App.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunch_App.Tests
{
    /// <summary>
    /// Summary description for Logic_Tests
    /// </summary>
    [TestClass]
    public class Logic_Tests
    {
        public Logic_Tests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //public void TestZipMethod()
        //{
        //   var result = FilterLogic.FindZipCodes("72023", 2);
        //   var expect = new List<string>() {"72023"};

        //   //Assert.AreEqual(expect, result);
        //   Assert.AreSame(expect, result);
        //}

        [TestMethod]
        public void SingleDayStringParce()
        {
            var result = FilterLogic.BreakHoursToRanges("Thursday 8am-9pm").ToList();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(DayOfWeek.Thursday, result[0].DayOfWeek);
            Assert.AreEqual(new DateTime(1, 1, 1, 8, 0, 0), result[0].Open);
            Assert.AreEqual(new DateTime(1, 1, 1, 21, 0, 0), result[0].Close);
        }


        [TestMethod]
        public void SingleMultiDayStringParce()
        {
            var result = FilterLogic.BreakHoursToRanges("Thursday-Friday 8am-9pm").ToList();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(DayOfWeek.Thursday, result[0].DayOfWeek);
            Assert.AreEqual(new DateTime(1, 1, 1, 8, 0, 0), result[0].Open);
            Assert.AreEqual(new DateTime(1, 1, 1, 21, 0, 0), result[0].Close);
            Assert.AreEqual(DayOfWeek.Friday, result[1].DayOfWeek);
            Assert.AreEqual(new DateTime(1, 1, 1, 8, 0, 0), result[1].Open);
            Assert.AreEqual(new DateTime(1, 1, 1, 21, 0, 0), result[1].Close);
        }

        [TestMethod]
        public void MultiDayStringParce()
        {
            var result = FilterLogic.BreakHoursToRanges("Thursday 8am-9pm, Saturday 11am-10pm").ToList();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(DayOfWeek.Thursday, result[0].DayOfWeek);
            Assert.AreEqual(new DateTime(1, 1, 1, 8, 0, 0), result[0].Open);
            Assert.AreEqual(new DateTime(1, 1, 1, 21, 0, 0), result[0].Close);
            Assert.AreEqual(DayOfWeek.Saturday, result[1].DayOfWeek);
            Assert.AreEqual(new DateTime(1, 1, 1, 11, 0, 0), result[1].Open);
            Assert.AreEqual(new DateTime(1, 1, 1, 22, 0, 0), result[1].Close);
        }

        [TestMethod]
        public void MultiDayMultiStringParce()
        {
            var result = FilterLogic.BreakHoursToRanges("Thursday 8am-9pm, Friday-Saturday 11am-10pm").ToList();

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(DayOfWeek.Thursday, result[0].DayOfWeek);
            Assert.AreEqual(new DateTime(1, 1, 1, 8, 0, 0), result[0].Open);
            Assert.AreEqual(new DateTime(1, 1, 1, 21, 0, 0), result[0].Close);
            Assert.AreEqual(DayOfWeek.Friday, result[1].DayOfWeek);
            Assert.AreEqual(new DateTime(1, 1, 1, 11, 0, 0), result[1].Open);
            Assert.AreEqual(new DateTime(1, 1, 1, 22, 0, 0), result[1].Close);
            Assert.AreEqual(DayOfWeek.Saturday, result[2].DayOfWeek);
            Assert.AreEqual(new DateTime(1, 1, 1, 11, 0, 0), result[2].Open);
            Assert.AreEqual(new DateTime(1, 1, 1, 22, 0, 0), result[2].Close);
        }

    }
}
