using CoatHanger.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var product = GetProductModel();

            // act
            var result = reportService.GetTestResult(product);

            // assert
            Assert.IsNotNull(result);

            using (StreamWriter file = new StreamWriter(@"C:\temp\TestMethod.html", append: false))
            {
                file.WriteLine(result);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            // arrange 
            var reportService = new TraceabilityMatrixReportService();
            var product = GetProductModel();

            // act
            var result = reportService.GetTestResult(product);

            // assert

            Assert.IsNotNull(result);

            using (StreamWriter file = new StreamWriter(@"C:\temp\TestMethod2.html", append: false))
            {
                file.WriteLine(result);
            }
        }

        [TestMethod]
        public void WhenGeneratingTracabilityMatrixReport_ExamineRequirementID()
        {
            // arrange
            var service = new TraceabilityMatrixReportService();
            var product = GetProductModel();

            // act 
            var result = service.ConvertToDto(product);

            // assert
            Assert.AreEqual("C.1-1", result.Requirements[0].RequirementID);
            Assert.AreEqual("C.2-1", result.Requirements[1].RequirementID);
            Assert.AreEqual("C.2-2", result.Requirements[2].RequirementID);
        }

        [TestMethod]
        public void WhenGeneratingTracabilityMatrixReport_ExamaineCounts()
        {
            // arrange
            var service = new TraceabilityMatrixReportService();
            var product = GetProductModel();

            // act 
            var result = service.ConvertToDto(product);

            // assert
            Assert.AreEqual(2, result.TestCases.Count);
            Assert.AreEqual(3, result.Requirements.Count);
        }

        [TestMethod]
        public void WhenGeneratingTracabilityMatrixReport_ExamineRequirementsMatrixCount()
        {
            // arrange
            var service = new TraceabilityMatrixReportService();
            var product = GetProductModel();

            // act 
            var result = service.ConvertToDto(product);

            foreach(var item in result.TestCases)
            {
                Assert.AreEqual(result.Requirements.Count, item.RequirementMatrix.Count);
            }
        }

        public void WhenGeneratingTracabilityMatrixReport_ExamineTracableRequirementsCount()
        {
            // arrange
            var service = new TraceabilityMatrixReportService();
            var product = GetProductModel();

            // act 
            var result = service.ConvertToDto(product);

            Assert.AreEqual(1, result.TestCases[0].TraceableRequirementCount);
            Assert.AreEqual(2, result.TestCases[1].TraceableRequirementCount);
        }

        [TestMethod]
        public void Example4()
        {
            // arrange
            var service = new TraceabilityMatrixReportService();
            var product = GetProductModel();

            // act 
            var result = service.ConvertToDto(product);

            // assert
      
        }

        /// <summary>
        /// Represent a tree hierarchy of test suite. 
        /// -- System Product
        /// |-- Weather Forecast Feature [A]
        /// |   |-- Temperature Calculation Function [B]
        /// |   |   |-- Test Case C.1
        /// |   |   `-- Test Case C.2
        /// |   `-- Weather Grid Function [D]
        /// |       |-- Test Case E.1
        /// |       `-- Test Case E.2
        /// `-- About Suite Feature [F]
        ///     -- Layout Function [G]
        ///         |-- Test Case H.1
        ///         `-- Test Case H.2
        /// </summary>
        private Product GetProductModel()
        {
            var systemSuite = new Product()
            {
                ProductID = "CH-WEB",
                Title = "Coathanger Website",
                Summary = "The coathanger website is an example of application using this testing framework.",
                Features = new List<Feature>()
                {
                    new Feature
                    {
                        FeatureID = "A",
                        Title = "Weather Forecast Feature",
                        Functions = new List<Function>()
                        {
                            new Function
                            {
                                FunctionID = "B",
                                Title = "Temperature Calculation Function",
                                TestCases = new List<TestCase>()
                                {
                                    new TestCase()
                                    {
                                        TestCaseID = "C.1",
                                        Scenario = "When the temperature is less than zero degree celcius",
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
                                                Action = "Execute the function `GetTemperatureSummary` with the input variables `temperature` and assign the value to the `result` variable.",
                                                ExpectedOutcome = null,
                                            },
                                            new TestStep()
                                            {
                                                StepNumber = 3,
                                                Action = "Examine the system outputs",
                                                ExpectedOutcome = new ExpectedOutcome()
                                                {
                                                    StepNumber = 1,
                                                    Description = "The system shall output the value freezing."
                                                },
                                            },
                                        },

                                    },
                                    new TestCase()
                                    {
                                        TestCaseID = "C.2",
                                        Scenario = "When the temperature is exactly zero degree celcius",
                                        Author = new Author("godreaj"),
                                        WorkItems = null,
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
                                                Action = "Set the variable temperature to 0.",
                                                ExpectedOutcome = null,
                                            },
                                            new TestStep()
                                            {
                                                StepNumber = 2,
                                                Action = "Execute the function `GetTemperatureSummary` with the input variables `temperature` and assign the value to the `result` variable.",
                                                ExpectedOutcome = null,
                                            },
                                            new TestStep()
                                            {
                                                StepNumber = 3,
                                                Action = "Examine the result variable.",
                                                ExpectedOutcome = new ExpectedOutcome()
                                                {
                                                    StepNumber = 1,
                                                    Description = "The system shall output the value Freezing."
                                                },
                                            },
                                            new TestStep()
                                            {
                                                StepNumber = 4,
                                                Action = "Examine the icon.",
                                                ExpectedOutcome = new ExpectedOutcome()
                                                {
                                                    StepNumber = 2,
                                                    Description = "The system shall displayed a ice icon."
                                                },
                                            },
                                        },
                                    }
                                }

                            },

                            new Function
                            {
                                FunctionID = "C",
                                Title = "Weather Grid"
                            },
                        },


                    },
                    new Feature
                    {
                        FeatureID = "D",
                        Title = "About"
                    }
                }            
            };

            return systemSuite;
        }

        public class SystemSuite : IProduct
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
