using CoatHanger.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CoatHanger.Core.Testing.UnitTest
{
    [TestClass]
    public class ReportServiceTest
    {

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

            foreach (var item in result.TestCases)
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
                                Scenarios = new List<Scenario>()
                                {
                                    new Scenario()
                                    {

                                        TestCases = new List<TestCase>()
                                        {
                                                     new TestCase()
                                    {
                                        TestCaseID = "C.2",
                                        Title = "When the temperature is exactly zero degree celcius",
                                        Author = new Author("godreaj"),
                                        WorkItems = null,
                                        TestExecution = new TestExecution
                                        {
                                            IsCompleted = true,
                                            ExecutedBy = new Author("desktop-j9fucdd\\buildserver"),
                                            ExecuteStartDate = "2020-05-01 11:45",
                                            ExecuteEndDate = "2020-05-01 11:46",
                                        },
                                        TestSteps = new List<TestStep>
                                        {
                                            new TestStep()
                                            {
                                                StepNumber = 1,
                                                Actions = new List<string> { "Set the variable temperature to 0." },
                                                ExpectedOutcome = null,
                                            },
                                            new TestStep()
                                            {
                                                StepNumber = 2,
                                                Actions = new List<string> {"Execute the function `GetTemperatureSummary` with the input variables `temperature` and assign the value to the `result` variable." },
                                                ExpectedOutcome = null,
                                            },
                                            new TestStep()
                                            {
                                                StepNumber = 3,
                                                Actions = new List<string> {"Examine the result variable." },
                                                ExpectedOutcome = new ExpectedOutcome
                                                {
                                                    RequirementID = "C.2-1",
                                                    ExpectedResult = "The system shall output the value Freezing."
                                                },
                                            },
                                            new TestStep()
                                            {
                                                StepNumber = 4,
                                                Actions = new List<string> {"Examine the icon." },
                                                ExpectedOutcome = new ExpectedOutcome
                                                {
                                                    RequirementID = "C.2-2",
                                                    ExpectedResult = "The system shall displayed a ice icon."
                                                },
                                            },
                                        },
                                    }
                                        }

                                    },

                                    new Scenario()
                                    {
                                        TestCases = new List<TestCase>()
                                {
                                    new TestCase()
                                    {
                                        TestCaseID = "C.1",
                                        Title = "When the temperature is less than zero degree celcius",
                                        Author = new Author("godreaj"),
                                        WorkItems = new List<string> { "111", "222", "333" },
                                        TestExecution = new TestExecution()
                                        {
                                            IsCompleted = true,
                                            ExecutedBy = new Author("desktop-j9fucdd\\buildserver"),
                                            ExecuteStartDate = "2020-05-01 11:45",
                                            ExecuteEndDate = "2020-05-01 11:46",
                                        },
                                        TestSteps = new List<TestStep>()
                                        {
                                            new TestStep()
                                            {
                                                StepNumber = 1,
                                                Actions = new List<string> {"Set the variable temperature to -2." },
                                                ExpectedOutcome = null,
                                            },
                                            new TestStep()
                                            {
                                                StepNumber = 2,
                                                Actions = new List<string> {"Execute the function `GetTemperatureSummary` with the input variables `temperature` and assign the value to the `result` variable." },
                                                ExpectedOutcome = null,
                                            },
                                            new TestStep()
                                            {
                                                StepNumber = 3,
                                                Actions = new List<string> {"Examine the system outputs" },
                                                ExpectedOutcome = new ExpectedOutcome
                                                {
                                                    RequirementID = "C.1-1",
                                                    ExpectedResult = "The system shall output the value freezing."
                                                },
                                            },
                                        },

                                    },


                                }
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
    }
}
