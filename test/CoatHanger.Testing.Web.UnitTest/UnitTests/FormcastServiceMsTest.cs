using CoatHanger.Testing.Web.Services.WeatherForcast;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoatHanger.Testing.Web.UnitTest
{
    /// <summary>
    /// TRAINING PURPOSE ONLY. This class is used as a reference before being converted into a "coat hanger" test. 
    /// Hopfully this will help you 
    /// </summary>
    [TestClass]
    public class FormcastServiceMsTest
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

        [TestMethod("When the temperature is less than zero degree celcius")]
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

        [TestMethod("When the temperature is exactly zero degree celcius")]
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

        [TestMethod("When the temperature is between 1 and 20 degree celcius")]
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

        [TestMethod("When the temperature is exactly 20 degree celcius")]

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


        [TestMethod("When the temperature is less than 25 degree celcius")]
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


        [TestMethod("When the temperature is exactly 25 degree celcius")]
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

        [TestMethod("When the temperature is between 25 and 29 degree celcius")]
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

        [TestMethod("When the temperature is exactly 30 degree celcius")]
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

        [TestMethod("When the temperature is greater than 30 degree celcius")]
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
