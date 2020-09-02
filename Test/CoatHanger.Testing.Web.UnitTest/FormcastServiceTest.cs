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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]

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


        [TestMethod]

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


        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
    }
}
