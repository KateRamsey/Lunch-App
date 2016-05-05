using System;
using System.Text;
using System.Collections.Generic;
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
        public void SunStringParce()
        {
            var result = FilterLogic.BreakHoursToRanges("Sun 8a-9p");
            var expect = new HoursOfOperations
            { SunOpen = new DateTime(2016, 5, 8, 8, 0, 0), SunClose = new DateTime(2016, 5, 8, 21, 0, 0)};


            Assert.AreEqual(expect.SunOpen, result.SunOpen);
            Assert.AreEqual(expect.SunClose, result.SunClose);
        }
    }
}
