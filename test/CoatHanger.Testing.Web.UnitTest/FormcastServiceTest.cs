using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoatHanger.Core;
using CoatHanger.Core.Enums;
using CoatHanger.Testing.Web.Services.WeatherForcast;
using CoatHanger.Core.Attributes;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Diagnostics;

namespace CoatHanger.Testing.Web.UnitTest
{
    [TestClass]    
    [TestSuite(suiteClass: typeof(WeatherForcastSuite))]
    public class FormcastServiceTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestCase(title: "When the temperature is less than zero degree celcius")]
        [Owner("Billy")]
        public void WhenTemperatureIsLessThanZero_ExpectFreezing()
        {
            // arrange
            FormcastService service = new FormcastService();
            var temperature = -1;

            // act
            var output = service.GetTemperatureSummary(temperature);

            // assert
            Assert.AreEqual("Freezing", output);
        }

        [TestCase(title: "When the temperature is exactly zero degree celcius")]
        public void WhenTemperatureIsZero_ExpectFreezing()
        {
            // arrange
            FormcastService service = new FormcastService();
            var temperature = 0;

            // act
            var result = service.GetTemperatureSummary(temperature);

            // assert
            Assert.AreEqual("Freezing", result);
        }

        [TestCase(title: "When the temperature is between 1 and 20 degree celcius")]
        public void WhenTemperatureBetweenOneAndLessThanTwenty_ExpectCool()
        {
            // arrange
            FormcastService service = new FormcastService();
            var temperature = 15;

            // act
            var result = service.GetTemperatureSummary(temperature);

            // assert
            Assert.AreEqual("Cool", result);
        }

        [TestCase(title: "When the temperature is exactly 20 degree celcius")]

        public void WhenTemperatureExactlyTwenty_ExpectMild()
        {
            // arrange
            FormcastService service = new FormcastService();
            var temperature = 20;

            // act
            var result = service.GetTemperatureSummary(temperature);

            // assert
            Assert.AreEqual("Mild", result);
        }


        [TestCase(title: "When the temperature is less than 25 degree celcius")]
        public void WhenTemperatureLessThanTwentyFive_ExpectMild()
        {
            // arrange
            FormcastService service = new FormcastService();
            var temperature = 24;

            // act
            var result = service.GetTemperatureSummary(temperature);

            // assert
            Assert.AreEqual("Mild", result);
        }


        [TestCase(title: "When the temperature is exactly 25 degree celcius")]
        public void WhenTemperatureExactlyThanTwentyFive_ExpectMild()
        {
            // arrange
            FormcastService service = new FormcastService();
            var temperature = 25;

            // act
            var result = service.GetTemperatureSummary(temperature);

            // assert
            Assert.AreEqual("Hot", result);
        }

        [TestCase(title: "When the temperature is between 25 and 29 degree celcius")]
        public void WhenTemperatureBetweenTwentyFiveAndLessThanThirty_ExpectHot()
        {
            // arrange
            FormcastService service = new FormcastService();
            var temperature = 27;

            // act
            var result = service.GetTemperatureSummary(temperature);

            // assert
            Assert.AreEqual("Hot", result);
        }

        [TestCase(title: "When the temperature is exactly 30 degree celcius")]
        public void WhenTemperatureExactlyThirty_ExpectScorching()
        {
            // arrange
            FormcastService service = new FormcastService();
            var temperature = 30;

            // act
            var result = service.GetTemperatureSummary(temperature);

            // assert
            Assert.AreEqual("Scorching", result);
        }

        [TestCase(title: "When the temperature is greater than 30 degree celcius")]
        public void WhenTemperatureGreaterThanThirty_ExpectScorching()
        {
            // arrange
            FormcastService service = new FormcastService();
            var temperature = 31;

            // act
            var result = service.GetTemperatureSummary(temperature);

            // assert
            Assert.AreEqual("Scorching", result);
        }


        [TestCleanup]
        public void TestCleanup()
        {

            Console.WriteLine(TestContext.TestName);
        }


    }
}
