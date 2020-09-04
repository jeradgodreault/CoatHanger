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
        private CoatHangerSuite GetSuite()
        {
            var systemSuite = new CoatHangerSuite()
            {
                Identifier = "System",
                Title = "System Specification",
                TestSuites = new List<CoatHangerSuite>()
                {
                    new CoatHangerSuite
                    {
                        Identifier = "A",
                        Title = "Weather Forecast",

                        TestSuites = new List<CoatHangerSuite>()
                        {
                            new CoatHangerSuite
                            {
                                Identifier = "B",
                                Title = "Temperature Calculation Suite",
                                TestCases = new List<CoatHangerTestCase>()
                                {
                                    new CoatHangerTestCase()
                                    {
                                        TestCaseID = "A.1",
                                        TestCaseDescription = "When the temperature is less than zero degree celcius",
                                        Author = new Author("godreaj"),
                                        WorkItems = new List<string> { "111", "222", "333" },
                                        TestExecution = new CoatHangerExecution()
                                        {
                                            Outcome = true,
                                            ExecutedBy = new Author("desktop-j9fucdd\\buildserver"),
                                            ExecuteStartDate = "2020-05-01 11:45",
                                            ExecuteEndDate = "2020-05-01 11:46",
                                        },
                                        TestSteps = new List<CoatHangerTestStep>()
                                        {
                                            new CoatHangerTestStep()
                                            {
                                                TestStepNumber = 1,
                                                Action = "Set the variable temperature to -2.",
                                                ExpectedOutcome = null,
                                            },
                                            new CoatHangerTestStep()
                                            {
                                                TestStepNumber = 2,
                                                Action = "Examine the system outputs",
                                                ExpectedOutcome = new CoatHangerExpectedResult()
                                                {
                                                    StepNumber = 1,
                                                    ExpectedResultDescription = "The system shall output the value freezing."
                                                },
                                            },

                                        }
                                    }
                                },

                            },

                            new CoatHangerSuite
                            {
                                Identifier = "C",
                                Title = "Weather Grid"
                            },
                        },


                    },
                    new CoatHangerSuite
                    {
                        Identifier = "D",
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
