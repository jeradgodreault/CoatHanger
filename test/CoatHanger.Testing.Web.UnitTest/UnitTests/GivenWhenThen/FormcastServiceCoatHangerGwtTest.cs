using CoatHanger.Core;
using CoatHanger.Core.Enums;
using CoatHanger.Testing.Web.Services.WeatherForcast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Reflection;
using System.Linq.Expressions;

namespace CoatHanger.Testing.Web.UnitTest
{

    /// <summary>
    /// Purpose of this test is to show everything using the Coat Hanger apporach. 
    /// Use the <see cref="FormcastServiceMsTest"/> as example of MSTest implementation. 
    /// </summary>
    [TestClass]
    [CoatHanger.Area(areaType: typeof(TemperatureCalculationFunction))]
    public class FormcastServiceCoatHangerGwtTest
    {
        private TestProcedure TestProcedure;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestInitialize()]
        public void BeforeTestExecution()
        {
            // Between each test run reset
            TestProcedure = new TestProcedure();            
        }

        [TestCleanup]
        public void AfterTestExecution()
        {
            CoatHangerManager.CoatHangerService.AddTestCase
            (
                assembly: Assembly.GetExecutingAssembly(), 
                testContext: TestContext,
                testProcedure: TestProcedure
            );
        }

        [CoatHanger.TestCase
        (
            identifier: "A.99", 
            scenario: "When the temperature is less than zero degree celcius",
            description: "This test case verifies the **freezing** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("godreaj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.UserStory)]
        public void WhenTemperatureIsLessThanZero_ExpectFreezing()
        {
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());
            FormcastService service = new FormcastService();

            // given                        
            procedure.Given(thatFormat: "the temperature is {0} degree celcius", value: -1, out var temperature);

            // when 
            procedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // then 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Freezing"
            );
        }

        [CoatHanger.TestCase
        (
            identifier: "A.1.1",
            scenario: "When the temperature is exactly zero degree celcius",
            description: "This test case verifies the **freezing** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("godreaj")]
        [CoatHanger.Release("1.1.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureIsZero_ExpectFreezing() 
        {
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());
            FormcastService service = new FormcastService();

            // given            
            procedure.Given
            (
                thatFormat: "the temperature is {0} degree celcius",
                value: 0,
                out var temperature
            );

            // when 
            procedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // then
            procedure.Then
            (
                that: "the temperature will be freezing.",
                by : () => AreEqual("Freezing", result)
            );
        }

        [CoatHanger.TestCase
        (
            identifier:"A.3", 
            scenario: "When the temperature is between 1 and 20 degree celcius",
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
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            procedure
                .Given(that: $"the current temperature is between 1 or 20 degree celcius")
                .AndGivenTemplate
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
                .AndWhen
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
 
        [CoatHanger.TestCase
        (
            identifier: "A.4", 
            scenario: "When the temperature is exactly 20 degree celcius",
            description: "This test case verifies the **mild** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureExactlyTwenty_ExpectMild()
        {
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            procedure.Given(thatFormat: "the temperature is {0} degree celcius", value: 20, out var temperature);

            // when 
            procedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // then            
            procedure.Then
            (
                thatFormat: "the temperature will be {0}.", 
                expectedResult: "Mild",  
                by: (expectedResult) => AreEqual(expectedResult, result)
            );
        }


        [CoatHanger.TestCase
        (
            identifier: "A.5", 
            scenario: "When the temperature is less than 25 degree celcius",
            description: "This test case verifies the **mild** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureLessThanTwentyFive_ExpectMild()
        {
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            procedure.Given(thatFormat: "the temperature is {0} degree celcius", value: 24, out var temperature);

            // when 
            procedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );
            
            procedure.Then(that: "the temperature will be labeled as not cold.", ToVerify: (step) =>
            {
                step.Statement("Examine the results")
                .Confirm(that: "The value is Mild", actual: result, assertionMethod: Assert.AreEqual, expected: "Mild");

                step.Statement("Examine the results for what its not")
                .Confirm(that: "The value is not Hot", actual: result, AreNotEqual, expected: "Hot")
                .And(that: "The value is not cold", actual: result, AreNotEqual,  expected: "Hot")
                .And(that: "The temperature < 25 ", actual: true, AreEqual, expected: (temperature < 25))
                .And(that: "The temperature >= 20 ", actual: true, AreEqual, expected: (temperature >= 20))
                ;

                return step;
            });
            
 
        }


        [CoatHanger.TestCase
        (
            identifier: "A.6", 
            scenario: "When the temperature is exactly 25 degree celcius",
            description: "This test case verifies the **mild** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureExactlyThanTwentyFive_ExpectMild()
        {
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            procedure.Given(thatFormat: "the temperature is {0} degree celcius", value: 25, out var temperature);

            // when 
            procedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // then 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Hot"
                , expectedResultNotMetMessage: "The system failed to display the hot temperature"
            );
        }

        [CoatHanger.TestCase
        (
            identifier: "A.7", 
            scenario: "When the temperature is between 25 and 29 degree celcius",
            description: "This test case verifies the **hot** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureBetweenTwentyFiveAndLessThanThirty_ExpectHot()
        {
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            procedure.Given(thatFormat: "the temperature is {0} degree celcius", value: 27, out var temperature);

            // when 
            procedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // then 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Hot"
            );

        }

        [CoatHanger.TestCase
        (
            identifier: "A.8",
            scenario: "When the temperature is exactly 30 degree celcius",
            description: "This test case verifies the **scorching** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureExactlyThirty_ExpectScorching()
        {
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            procedure.Given(thatFormat: "the temperature is {0} degree celcius", value: 30, out var temperature);

            // when 
            procedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // then 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Scorching"
            );
        }

        [CoatHanger.TestCase
        (
            identifier: "A.9", 
            scenario: "When the temperature is greater than 30 degree celcius",
            description: "This test case verifies the **scorching** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureGreaterThanThirty_ExpectScorching()
        {
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // given            
            procedure.Given(thatFormat: "the temperature is {0} degree celcius", value: 31, out var temperature);

            // when 
            procedure.When
            (
                that: "getting the temperature summary",
                by: () => service.GetTemperatureSummary(temperature),
                out var result
            );

            // then 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Scorching"
            );
        }

       [CoatHanger.TestCase
       (
           manualTest: true,
           identifier: "A.10",
           scenario: "Where the performance of the temperature page is examined.",
           description: "This test case verifies bug item #45214 - page is too slow."
       )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.4.1")]
        [CoatHanger.Requirement(NonFunctionalRequirement.Usability)]
        public void WhenTemperaturePagePerformanceIsExamined()
        {
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());

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

        [CoatHanger.TestCase
        (
            manualTest: true,
            identifier: "A.01",
            scenario: "Where the performance of the \"contact us\" page is examined.",
            description: "This test case verifies cross cutting concern of about page performance."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.Requirement(NonFunctionalRequirement.Usability)]
        public void WhenAboutPagePerformanceIsExamined()
        {
            var procedure = TestProcedure.StartGivenWhenThen(MethodBase.GetCurrentMethod());

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
                expectedResult: "The system shall include a `© Coathanger YYYY` notice with the current year."
            );

            Inconclusive("Manual Test Case - not yet automated");
        }
    }
}
