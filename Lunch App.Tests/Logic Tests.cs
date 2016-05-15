using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lunch_App.Logic;
using Lunch_App.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunch_App.Tests
{
    /// <summary>
    /// Summary description for Logic_Tests
    /// </summary>
    [TestClass]
    public class Logic_Tests
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public Logic_Tests()
        {
            //
            // 
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

        [TestMethod]
        public void TestZipMethodSimple()
        {
            var result = FilterLogic.FindZipCodes("72120", (ZipCodeRadiusOption) 5, db);
            var expect = new List<string>() { "72120" };

            Assert.IsTrue(result.Contains("72120"));
        }

        [TestMethod]
        public void TestZipMethodComplex()
        {
            var result = FilterLogic.FindZipCodes("72115", (ZipCodeRadiusOption)5, db);
            var expect = new List<string>()
            { "72115", "72202", "72201", "72260", "72114", "72198", "72124", "72190", "72199", "72116", "72119" };

            Assert.AreEqual(11, result.Count());
            foreach (var x in expect)
            {
                Assert.IsTrue(result.Contains(x));
            }
            
        }

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

        [TestMethod]
        public void OpenResturant()
        {
            Assert.IsTrue(FilterLogic.ResturantOpen("Sunday-Saturday 1am-11pm", DateTime.Now));
        }

        [TestMethod]
        public void ClosedResturant()
        {
            Assert.IsFalse(FilterLogic.ResturantOpen("Sunday 1am-2pm", DateTime.Now));
        }

    }
}
