using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
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
        public void TotalOneSurvey()
        {
            var kate = new SurveyFilterModel(new Survey()
            {
                CuisineNotWanted = Cuisine.American,
                CuisineWanted = Cuisine.Thai,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72115",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = DateTime.Now
            });

            var kateList = new List<SurveyFilterModel> {kate};
            var result = FilterLogic.CombineSurveys(kateList,db);

            Assert.AreEqual(DateTime.Now.Date, result.LunchTime.Date);
            Assert.IsTrue(result.BaseZips.Contains("72115"));
            Assert.IsTrue(result.PossibleZips.Contains("72115"));
            Assert.IsTrue(result.PossibleZips.Contains("72198"));
            Assert.AreEqual(result.DietaryIssues, 0);
            Assert.IsTrue(result.NotWantedCuisines.Contains(Cuisine.American));
            Assert.IsTrue(result.WantedCuisines.Contains(Cuisine.Thai));

        }

        [TestMethod]
        public void TotalTwoSurveys()
        {
            var kate = new SurveyFilterModel(new Survey()
            {
                CuisineNotWanted = Cuisine.American,
                CuisineWanted = Cuisine.Thai,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72115",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = DateTime.Now
            });

            var bruce = new SurveyFilterModel(new Survey()
            {
                CuisineWanted = Cuisine.American,
                CuisineNotWanted = Cuisine.Japanese,
                DietaryIssues = 2,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72198",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = DateTime.Now
            });

            var surveyList = new List<SurveyFilterModel> { kate, bruce };
            var result = FilterLogic.CombineSurveys(surveyList, db);

            Assert.AreEqual(DateTime.Now.Date, result.LunchTime.Date);
            Assert.IsTrue(result.BaseZips.Contains("72115"));
            Assert.IsTrue(result.BaseZips.Contains("72198"));
            Assert.IsTrue(result.PossibleZips.Contains("72115"));
            Assert.IsTrue(result.PossibleZips.Contains("72198"));
            Assert.IsTrue(result.PossibleZips.Contains("72202"));
            Assert.AreEqual(result.DietaryIssues, 2);
            Assert.IsTrue(result.NotWantedCuisines.Contains(Cuisine.American));
            Assert.IsTrue(result.NotWantedCuisines.Contains(Cuisine.Japanese));
            Assert.IsTrue(result.WantedCuisines.Contains(Cuisine.Thai));
            Assert.IsTrue(result.WantedCuisines.Contains(Cuisine.American));

        }

        [TestMethod]
        public void DietaryNeedCheckPass()
        {
            var list = new List<SurveyFilterModel>();
            var bruce = new SurveyFilterModel(new Survey()
            {
                CuisineWanted = Cuisine.American,
                CuisineNotWanted = Cuisine.Japanese,
                DietaryIssues = 2,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72198",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = DateTime.Now
            });
            list.Add(bruce);

            var surveyTotal = FilterLogic.CombineSurveys(list, db);

            var seafood = new RestaurantFilterModel(new Restaurant()
            {
                Id =3,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Seafood,
                DietaryOptions = 239
            });

            var result = FilterLogic.RestaurantMeetsDietaryNeeds(surveyTotal, seafood);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DietaryNeedCheckFail()
        {
            var list = new List<SurveyFilterModel>();
            var bruce = new SurveyFilterModel(new Survey()
            {
                CuisineWanted = Cuisine.American,
                CuisineNotWanted = Cuisine.Japanese,
                DietaryIssues = 18,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72198",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = DateTime.Now
            });
            list.Add(bruce);

            var surveyTotal = FilterLogic.CombineSurveys(list, db);

            var seafood = new RestaurantFilterModel(new Restaurant()
            {
                Id = 3,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Seafood,
                DietaryOptions = 239
            });

            var result = FilterLogic.RestaurantMeetsDietaryNeeds(surveyTotal, seafood);

            Assert.IsFalse(result);
        }


        [TestMethod]
        public void FilterNone()
        {
            var kate = new SurveyFilterModel(new Survey()
            {
                CuisineNotWanted = Cuisine.Japanese,
                CuisineWanted = Cuisine.Pizza,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72115",
                ZipCodeRadius = ZipCodeRadiusOption.Thirty,
                TimeAvailable = DateTime.Now
            });

            var bruce = new SurveyFilterModel(new Survey()
            {
                CuisineWanted = Cuisine.BBQ,
                CuisineNotWanted = Cuisine.Japanese,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72198",
                ZipCodeRadius = ZipCodeRadiusOption.Thirty,
                TimeAvailable = DateTime.Now
            });

            var surveyList = new List<SurveyFilterModel> { kate, bruce };


            var seafood = new RestaurantFilterModel(new Restaurant()
            {
                Id = 3,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Seafood,
                DietaryOptions = 239
            });

            var burger = new RestaurantFilterModel(new Restaurant()
            {
                Id = 2,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.American,
                DietaryOptions = 255
            });

            var pizza = new RestaurantFilterModel(new Restaurant()
            {
                Id = 10,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Pizza,
                DietaryOptions = 251
            });

            var restaurantList = new List<RestaurantFilterModel>() {seafood, burger, pizza};

            var result = FilterLogic.Filter(restaurantList, surveyList, db);

            Assert.IsTrue(result.Count == 3);
            Assert.IsTrue(result.Contains(10));
            Assert.IsTrue(result.Contains(2));
            Assert.IsTrue(result.Contains(3));
            Assert.IsTrue(result[0] == 10);

        }

        [TestMethod]
        public void FilterOne()
        {
            var kate = new SurveyFilterModel(new Survey()
            {
                CuisineNotWanted = Cuisine.American,
                CuisineWanted = Cuisine.Pizza,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72115",
                ZipCodeRadius = ZipCodeRadiusOption.Thirty,
                TimeAvailable = DateTime.Now
            });

            var bruce = new SurveyFilterModel(new Survey()
            {
                CuisineWanted = Cuisine.BBQ,
                CuisineNotWanted = Cuisine.Japanese,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72198",
                ZipCodeRadius = ZipCodeRadiusOption.Thirty,
                TimeAvailable = DateTime.Now
            });

            var surveyList = new List<SurveyFilterModel> { kate, bruce };


            var seafood = new RestaurantFilterModel(new Restaurant()
            {
                Id = 3,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Seafood,
                DietaryOptions = 239
            });

            var burger = new RestaurantFilterModel(new Restaurant()
            {
                Id = 2,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.American,
                DietaryOptions = 255
            });

            var pizza = new RestaurantFilterModel(new Restaurant()
            {
                Id = 10,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Pizza,
                DietaryOptions = 251
            });

            var restaurantList = new List<RestaurantFilterModel>() { seafood, burger, pizza };

            var result = FilterLogic.Filter(restaurantList, surveyList, db);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.Contains(10));
            Assert.IsFalse(result.Contains(2));
            Assert.IsTrue(result.Contains(3));
            Assert.IsTrue(result[0] == 10);

        }

        [TestMethod]
        public void FilterAllByZip()
        {
            var kate = new SurveyFilterModel(new Survey()
            {
                CuisineNotWanted = Cuisine.American,
                CuisineWanted = Cuisine.Pizza,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "20002",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = DateTime.Now
            });

            var bruce = new SurveyFilterModel(new Survey()
            {
                CuisineWanted = Cuisine.BBQ,
                CuisineNotWanted = Cuisine.Japanese,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "20002",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = DateTime.Now
            });

            var surveyList = new List<SurveyFilterModel> { kate, bruce };


            var seafood = new RestaurantFilterModel(new Restaurant()
            {
                Id = 3,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Seafood,
                DietaryOptions = 239
            });

            var burger = new RestaurantFilterModel(new Restaurant()
            {
                Id = 2,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.American,
                DietaryOptions = 255
            });

            var pizza = new RestaurantFilterModel(new Restaurant()
            {
                Id = 10,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Pizza,
                DietaryOptions = 251
            });

            var defaultPick = new RestaurantFilterModel(new Restaurant()
            {
                Id = 1,
                LocationZip = "72120",
                HoursOfOperation = "Monday 2am-3am",
                PriceRange = 2,
                CuisineType = Cuisine.None,
                DietaryOptions = 0
            });

            var restaurantList = new List<RestaurantFilterModel>() { seafood, burger, pizza, defaultPick };

            var result = FilterLogic.Filter(restaurantList, surveyList, db);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Contains(1));
            Assert.IsTrue(result[0] == 1);

        }

        [TestMethod]
        public void FilterAllByTime()
        {
            var kate = new SurveyFilterModel(new Survey()
            {
                CuisineNotWanted = Cuisine.American,
                CuisineWanted = Cuisine.Pizza,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72120",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = new DateTime(2016, 5, 15, 3, 0, 0)
            });

            var bruce = new SurveyFilterModel(new Survey()
            {
                CuisineWanted = Cuisine.BBQ,
                CuisineNotWanted = Cuisine.Japanese,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72120",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = new DateTime(2016, 5, 15, 3, 0, 0)
            });

            var surveyList = new List<SurveyFilterModel> { kate, bruce };


            var seafood = new RestaurantFilterModel(new Restaurant()
            {
                Id = 3,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Seafood,
                DietaryOptions = 239
            });

            var burger = new RestaurantFilterModel(new Restaurant()
            {
                Id = 2,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.American,
                DietaryOptions = 255
            });

            var pizza = new RestaurantFilterModel(new Restaurant()
            {
                Id = 10,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Pizza,
                DietaryOptions = 251
            });

            var defaultPick = new RestaurantFilterModel(new Restaurant()
            {
                Id = 1,
                LocationZip = "72120",
                HoursOfOperation = "Monday 2am-3am",
                PriceRange = 2,
                CuisineType = Cuisine.None,
                DietaryOptions = 0
            });

            var restaurantList = new List<RestaurantFilterModel>() { seafood, burger, pizza, defaultPick };

            var result = FilterLogic.Filter(restaurantList, surveyList, db);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Contains(1));
            Assert.IsTrue(result[0] == 1);

        }

        [TestMethod]
        public void FilterAllByDietary()
        {
            var kate = new SurveyFilterModel(new Survey()
            {
                CuisineNotWanted = Cuisine.American,
                CuisineWanted = Cuisine.Pizza,
                DietaryIssues = 255,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72120",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = new DateTime(2016, 5, 15, 3, 0, 0)
            });

            var bruce = new SurveyFilterModel(new Survey()
            {
                CuisineWanted = Cuisine.BBQ,
                CuisineNotWanted = Cuisine.Japanese,
                DietaryIssues = 0,
                IsComing = true,
                IsFinished = true,
                Id = 1,
                ZipCode = "72120",
                ZipCodeRadius = ZipCodeRadiusOption.Five,
                TimeAvailable = new DateTime(2016, 5, 15, 3, 0, 0)
            });

            var surveyList = new List<SurveyFilterModel> { kate, bruce };


            var seafood = new RestaurantFilterModel(new Restaurant()
            {
                Id = 3,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Seafood,
                DietaryOptions = 239
            });

            var burger = new RestaurantFilterModel(new Restaurant()
            {
                Id = 2,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.American,
                DietaryOptions = 251
            });

            var pizza = new RestaurantFilterModel(new Restaurant()
            {
                Id = 10,
                LocationZip = "72120",
                HoursOfOperation = "Monday-Saturday 9am-9pm",
                PriceRange = 2,
                CuisineType = Cuisine.Pizza,
                DietaryOptions = 251
            });

            var defaultPick = new RestaurantFilterModel(new Restaurant()
            {
                Id = 1,
                LocationZip = "72120",
                HoursOfOperation = "Monday 2am-3am",
                PriceRange = 2,
                CuisineType = Cuisine.None,
                DietaryOptions = 0
            });

            var restaurantList = new List<RestaurantFilterModel>() { seafood, burger, pizza, defaultPick };

            var result = FilterLogic.Filter(restaurantList, surveyList, db);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Contains(1));
            Assert.IsTrue(result[0] == 1);

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
        public void OpenRestaurant()
        {
            Assert.IsTrue(FilterLogic.RestaurantOpen("Sunday-Saturday 1am-11pm", DateTime.Now));
        }

        [TestMethod]
        public void ClosedRestaurant()
        {
            Assert.IsFalse(FilterLogic.RestaurantOpen("Sunday 1am-2pm", DateTime.Now));
        }

    }
}
