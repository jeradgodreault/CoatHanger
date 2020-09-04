using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoatHanger.Core.Testing.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // arrange 

            // act

            // assert
        }



        /// <summary>
        /// Represent a tree hierarchy of test suite. 
        /// -- System Suite
        /// |-- Weather Forecast Suite
        /// |   |-- Test Case 1A
        /// |   |-- Test Case 2A
        /// |   |-- Temperature Calculation Suite
        /// |   |   |-- Test Case 1B
        /// |   |   `-- Test Case 2B
        /// |   `-- Weather Grid Suite
        /// |       |-- Test Case 1C
        /// |       `-- Test Case 2C
        /// `-- About Suite
        ///     -- Layout Suite
        ///         |-- Test Case 1D
        ///         `-- Test Case 2D
        /// </summary>
        public class SystemSuite : ISuite
        {
            public virtual string GetDisplayName() => "System";
            public virtual string GetSuitePath() => "/" + GetDisplayName();
        }

        public class WeatherForcastSuite : SystemSuite
        {
            public override string GetDisplayName() => "Weather Forcast";
            public override string GetSuitePath() => base.GetSuitePath() + "/" + GetDisplayName();
        }

        public class TemperatureCalculationSuite : WeatherForcastSuite
        {
            public override string GetDisplayName() => "Temperature Calculation";
            public override string GetSuitePath() => base.GetSuitePath() + "/" + GetDisplayName();
        }

        public class WeatherGridSuite : WeatherForcastSuite
        {
            public override string GetDisplayName() => "Weather Page Layout";
            public override string GetSuitePath() => base.GetSuitePath() + "/" + GetDisplayName();
        }


        public class AboutSuite : SystemSuite
        {
            public override string GetDisplayName() => "About";
            public override string GetSuitePath() => base.GetSuitePath() + "/" + GetDisplayName();
        }
    }
}
