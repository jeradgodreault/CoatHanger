using CoatHanger.Core;
using CoatHanger.Core.Enums;
using CoatHanger.Testing.Web.Services.WeatherForcast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Reflection;
using System.Linq.Expressions;
using CoatHanger.Core.Step;
using System.Diagnostics;
using System;

namespace CoatHanger.Testing.Web.UnitTest
{

    /// <summary>
    /// Purpose of this test is to show everything using the Coat Hanger apporach. 
    /// Use the <see cref="FormcastServiceMsTest"/> as example of MSTest implementation. 
    /// </summary>
    [TestClass]
    [CoatHanger.Area(areaType: typeof(TemperatureCalculation2Function))]
    public class FormcastServiceCoatHangerGwtTest
    {
        private GivenWhenThenProcedure TestProcedure;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestInitialize()]
        public void BeforeTestExecution()
        {
            // Between each test run reset
            TestProcedure = new GivenWhenThenProcedure();            
        }

        [TestCleanup]
        public void AfterTestExecution()
        {
            CoatHangerManager.CoatHangerService.AddTestCase
            (
                assembly: Assembly.GetExecutingAssembly(), 
                testResultOutcome: (TestResultOutcome)TestContext.CurrentTestOutcome,
                testProcedure: TestProcedure
            );
        }
        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "GWT.A.99", 
            title: "When the temperature is less than zero degree celcius",
            description: "This test case verifies the **freezing** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("godreaj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.UserStory)]
        public void WhenTemperatureIsLessThanZero_ExpectFreezing()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());
            FormcastService service = new FormcastService();

            // given                        
            TestProcedure.Given(thatFormat: "the temperature is {0} degree celcius", input: -1, out var temperature);

            // when 
            TestProcedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // then 
            TestProcedure.Then
            (
                that: "the temperature will be freezing.",
                by: () => AreEqual("Freezing", result)
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "GWT.A.1.1",
            title: "When the temperature is exactly zero degree celcius",
            description: "This test case verifies the **freezing** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("godreaj")]
        [CoatHanger.Release("1.1.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureIsZero_ExpectFreezing() 
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());
            FormcastService service = new FormcastService();

            // given            
            TestProcedure.Given
            (
                thatFormat: "the temperature is {0} degree celcius",
                input: 0,
                out var temperature
            );

            // when 
            TestProcedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // then
            TestProcedure.Then
            (
                that: "the temperature will be freezing.",
                by : () => AreEqual("Freezing", result)
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier:"A.3", 
            title: "When the temperature is between 1 and 20 degree celcius",
            description: "This test case verifies the temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0", "1.0.1", "1.2.0")]
        [CoatHanger.RegressionRelease("1.1.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        [DataRow("below 1 and 20", 0, "Freezing")]
        [DataRow("below 1 and 20", 1, "Cool")]
        [DataRow("below 1 and 20", 10, "Cool")]
        [DataRow("below 1 and 20", 19, "Cool")]
        [DataRow("below 1 and 20", 20, "Mild")]
        public void WhenTemperatureBetweenOneAndLessThanTwenty_ExpectCool(string range, int value, string expectedResult)
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());
            TestProcedure.RegisterParameter(nameof(range), range);
            TestProcedure.RegisterParameter(nameof(value), value);
            TestProcedure.RegisterParameter(nameof(expectedResult), expectedResult, "Expected Result");
            TestProcedure.AddTestReference("See TestRange_Approved.xlsx");

            FormcastService service = new FormcastService();
            
            // given            
            TestProcedure
                .Given(that: $"the current temperature is between 1 or 20 degree celcius")
                .AndTemplate
                (
                    template: "the temperature is {{temperature}} degree celcius", 
                    new 
                    { 
                        temperature = value, 
                        expectedResult = expectedResult 
                    } , 
                    out var weather
                )
                .When(that: "Looking at the sky for the weather forcast")
                .And
                (
                    that: "getting the temperature summary",
                    by: () => service.GetTemperatureSummary(weather.temperature),
                    out var result
                )
                .ThenTemplate
                (
                    template: "the temperature will be {expectedResult}.", 
                    expectedResult: weather, 
                    by: (weather) => AreEqual(weather.expectedResult, result)
                );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "GWT.A.4", 
            title: "When the temperature is exactly 20 degree celcius",
            description: "This test case verifies the **mild** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureExactlyTwenty_ExpectMild()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            TestProcedure.Given(thatFormat: "the temperature is {0} degree celcius", input: 20, out var temperature);


            // when 
            TestProcedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // then            
            TestProcedure.Then
            (
                thatFormat: "the temperature will be {0}.", 
                expectedResult: "Mild",  
                by: (expectedResult) => AreEqual(expectedResult, result)
            );

            TestProcedure.Finish();
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "GWT.A.5", 
            title: "When the temperature is less than 25 degree celcius",
            description: "This test case verifies the **mild** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureLessThanTwentyFive_ExpectMild()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            TestProcedure.Given(thatFormat: "the temperature is {0} degree celcius", input: 24, out var temperature);

            // when 
            TestProcedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );
            
            TestProcedure.Then(that: "the temperature will be labeled as not cold.", ToVerify: (step) =>
            {
                step.Statement("Examine the results")
                .Confirm(that: "The value is Mild", actual: result, assertionMethod: Assert.AreEqual, expected: "Mild");

                step.Statement("Examine the results for what its not")
                .Confirm(that: "The value is not Hot", actual: result, AreNotEqual, expected: "Hot")
                .And(that: "The value is not cold", actual: result, AreNotEqual,  expected: "Hot")
                .And(that: "The temperature < 25", actual: true, AreEqual, expected: (temperature < 25))
                .And(that: "The temperature >= 20", actual: true, AreEqual, expected: (temperature >= 20))
                ;

                return step;
            });
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "GWT.A.6", 
            title: "When the temperature is exactly 25 degree celcius",
            description: "This test case verifies the **mild** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureExactlyThanTwentyFive_ExpectMild()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            TestProcedure.Given(thatFormat: "the temperature is {0} degree celcius", input: 25, out var temperature);

            // when 
            TestProcedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // THEN
            TestProcedure.Then
            (
                that: "the temperature will be hot.",
                by: () => AreEqual("Hot", result)
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "GWT.A.7", 
            title: "When the temperature is between 25 and 29 degree celcius",
            description: "This test case verifies the **hot** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureBetweenTwentyFiveAndLessThanThirty_ExpectHot()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            TestProcedure.Given(thatFormat: "the temperature is {0} degree celcius", input: 27, out var temperature);

            // when 
            TestProcedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // THEN
            TestProcedure.Then
            (
                that: "the temperature will be hot.",
                by: () => AreEqual("Hot", result)
            );

        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "GWT.A.8",
            title: "When the temperature is exactly 30 degree celcius",
            description: "This test case verifies the **scorching** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureExactlyThirty_ExpectScorching()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            TestProcedure.Given(thatFormat: "the temperature is {0} degree celcius", input: 30, out var temperature);

            // when 
            TestProcedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // THEN
            TestProcedure.Then
            (
                that: "the temperature will be Scorching.",
                by: () => AreEqual("Scorching", result)
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "GWT.A.9", 
            title: "When the temperature is greater than 30 degree celcius",
            description: "This test case verifies the **scorching** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureGreaterThanThirty_ExpectScorching()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            TestProcedure.Given(thatFormat: "the temperature is {0} degree celcius", input: 31, out var temperature);

            // when 
            TestProcedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // THEN
            TestProcedure.Then
            (
                that: "the temperature will be Scorching.",
                by: () => AreEqual("Scorching", result)
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
       (
           manualTest: true,
           identifier: "GWT.A.10",
           title: "Where the performance of the temperature page is examined.",
           description: "This test case verifies bug item #45214 - page is too slow."
       )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.4.1")]
        [CoatHanger.Requirement(NonFunctionalRequirement.Usability)]
        public void WhenTemperaturePagePerformanceIsExamined()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());

            // Step 1 
            TestProcedure.AddStep("Navigate to the main page");
            
            // Step 2
            TestProcedure.AddStep
            (
                actions: new string[]
                {
                    "Look for the navigational menu on the top of the screen",
                    "Hover your mouse over the `Temperature Forcast` menu dropdown",
                    "Select the 'Today Forcast` dropdown item"
                }
            );

            // Step 3
            TestProcedure.AddStep
            (
                action: "Observe the time the next page takes to load.",
                expectedResults: new string[] 
                {
                    "The system shall navigated the user to the `Today forcast` page.",
                    "The page should load within less than 10 seconds"
                }
            );

            Inconclusive("Manual Test Case - not yet automated");
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            manualTest: true,
            identifier: "GWT.A.01",
            title: "Where the performance of the \"contact us\" page is examined.",
            description: "This test case verifies cross cutting concern of about page performance."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(NonFunctionalRequirement.Usability)]
        public void WhenAboutPagePerformanceIsExamined()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());

            // Step 1 
            TestProcedure.AddStep("Navigate to the main page");
            
            // Step 2
            TestProcedure.AddStep
            (
                actions: new string[]
                {
                    "Look for the navigational menu on the top of the screen",
                    "Hover your mouse over the `About Us` menu dropdown",
                    "Select the 'Contact Us` dropdown item"
                }
            );

            // Step 3 - shows a service layer agreement example
            TestProcedure.AddManualStep
            (
                action: "Observe the time the next page takes to load.",
                expectedResult: "The page should load within less than 5 seconds",
                requirementID: "SLA-0001" // SLA = service-level-agreement 
            );

            // Step 4 - shows a requirement example
            TestProcedure.AddManualStep
            (
                action: "Verify the contact us page loaded.",
                expectedResult: "The system shall navigated the user to the `Contact us` page.",
                requirementID: "REQ-0123" // REQ = Requirement
            );

            // Step 5 - shows user story example
            TestProcedure.AddManualStep
            (
                action: "Examine the body of the page.",
                expectedResult: "As a new customer, I want to see emails of the current management staff, so that I can contact them about question or concerns about services they provide.",
                requirementID: "US-9997" // US = User Story
            );

            // Step 6 - shows how requirementID will be auto generated if not provided.
            TestProcedure.AddStep
            (
                action: "Examine the footer page.",
                expectedResult: "The system shall include a `� Coathanger YYYY` notice with the current year."
            );

            Inconclusive("Manual Test Case - not yet automated");
        }


        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "GWT.A.11",
            title: "When the temperature is greater than 30 degree celcius",
            description: "This test case verifies the **scorching** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureGreaterThanThirty_ExpectBusinessRule()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            TestProcedure.Given(thatFormat: "the temperature is {0} degree celcius", input: 31, out var temperature);

            // when 
            TestProcedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // THEN
            TestProcedure.Then(
                businessRule: new ScorchingBusinessRule(), 
                ToVerify: (step) =>
            {
                step.Statement("Examine the results")
                .Confirm(that: "The value is Scorching", actual: result, assertionMethod: Assert.AreEqual, expected: "Scorching")
                .Confirm(businessRule: new NotEmptyBusinessRule(), actual: result != "", assertionMethod: Assert.AreEqual, expected: true)
                ;
                return step;
            });
        }

        public class ScorchingBusinessRule : BusinessRule
        {
            public override string ID { get => "A.02"; }
            public override string Title { get => "The system shall output temperature of Scorching when temperature is >= 30";  }
            public override string Summary { get => ""; }
            public override RuleType RuleType { get => RuleType.Computation; }
            public override BusinessRule Parent { get => null; }
        }

        public class NotEmptyBusinessRule : BusinessRule
        {
            public override string ID { get => "A.04"; }
            public override string Title { get => "The system shall return a value"; }
            public override string Summary { get => ""; }
            public override RuleType RuleType { get => RuleType.Decision; }
            public override BusinessRule Parent { get => new NoNullBusinessRule(); }
        }

        public class NoNullBusinessRule : BusinessRule
        {
            public override string ID { get => "A.06"; }
            public override string Title { get => "The system shall not return null"; }
            public override string Summary { get => ""; }
            public override RuleType RuleType { get => RuleType.Decision; }
            public override BusinessRule Parent { get => null; }
        }

    }
}
