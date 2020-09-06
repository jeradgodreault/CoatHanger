using CoatHanger.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace CoatHanger.Core.Testing.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [Ignore("Work-in-progress")]
        public void TestMethod1()
        {
            // arrange 
            var reportService = new ReportService();
            var testSuite = GetSuite();


            // act
            var result = reportService.GetTestResult(testSuite);

            // assert

            Assert.IsNotNull(result);

            using (StreamWriter file = new StreamWriter(@"C:\temp\TestMethod.html", append: false))
            {
                file.WriteLine(result);
            }


        }



        /// <summary>
        /// Represent a tree hierarchy of test suite. 
        /// -- System Suite
        /// |-- Weather Forecast Suite
        /// |   |-- Test Case A.1
        /// |   |-- Test Case A.2
        /// |   |-- Temperature Calculation Suite
        /// |   |   |-- Test Case B.1
        /// |   |   `-- Test Case B.2
        /// |   `-- Weather Grid Suite
        /// |       |-- Test Case C.1
        /// |       `-- Test Case C.2
        /// `-- About Suite
        ///     -- Layout Suite
        ///         |-- Test Case D.1
        ///         `-- Test Case D.2
        /// </summary>
        private TestSuite GetSuite()
        {
            var systemSuite = new TestSuite()
            {
                TestSuiteID = "System",
                Title = "System Specification",
                TestSuites = new List<TestSuite>()
                {
                    new TestSuite
                    {
                        TestSuiteID = "A",
                        Title = "Weather Forecast",

                        TestSuites = new List<TestSuite>()
                        {
                            new TestSuite
                            {
                                TestSuiteID = "B",
                                Title = "Temperature Calculation Suite",
                                TestCases = new List<TestCase>()
                                {
                                    new TestCase()
                                    {
                                        TestCaseID = "A.1",
                                        Title = "When the temperature is less than zero degree celcius",
                                        Author = new Author("godreaj"),
                                        WorkItems = new List<string> { "111", "222", "333" },
                                        TestExecution = new TestExecution()
                                        {
                                            Outcome = true,
                                            ExecutedBy = new Author("desktop-j9fucdd\\buildserver"),
                                            ExecuteStartDate = "2020-05-01 11:45",
                                            ExecuteEndDate = "2020-05-01 11:46",
                                        },
                                        TestSteps = new List<TestStep>()
                                        {
                                            new TestStep()
                                            {
                                                StepNumber = 1,
                                                Action = "Set the variable temperature to -2.",
                                                ExpectedOutcome = null,
                                            },
                                            new TestStep()
                                            {
                                                StepNumber = 2,
                                                Action = "Examine the system outputs",
                                                ExpectedOutcome = new ExpectedOutcome()
                                                {
                                                    StepNumber = 1,
                                                    Description = "The system shall output the value freezing."
                                                },
                                            },

                                        }
                                    }
                                },

                            },

                            new TestSuite
                            {
                                TestSuiteID = "C",
                                Title = "Weather Grid"
                            },
                        },


                    },
                    new TestSuite
                    {
                        TestSuiteID = "D",
                        Title = "About"
                    }
                }
            };


            return systemSuite;
        }



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
