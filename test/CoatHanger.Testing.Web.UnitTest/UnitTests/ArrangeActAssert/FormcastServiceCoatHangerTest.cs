using CoatHanger.Core;
using CoatHanger.Core.Enums;
using CoatHanger.Testing.Web.Services.WeatherForcast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using System.Reflection;
using System.Linq.Expressions;
using CoatHanger.Core.Style;

namespace CoatHanger.Testing.Web.UnitTest
{

    /// <summary>
    /// Purpose of this test is to show everything using the Coat Hanger apporach. 
    /// Use the <see cref="FormcastServiceMsTest"/> as example of MSTest implementation. 
    /// </summary>
    [TestClass]
    [CoatHanger.Area(areaType: typeof(TemperatureCalculationFunction))]
    public class FormcastServiceCoatHangerTest
    {
        private ArrangeActAssertProcedure TestProcedure;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestInitialize()]
        public void BeforeTestExecution()
        {
            // Between each test run reset
            TestProcedure = new ArrangeActAssertProcedure();            
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
            identifier: "A.99", 
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

            // arrange            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: -1
            );

            

            // act 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // assert 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Freezing"
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "A.1.1",
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

            // arrange            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 0
            );

            // act 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // assert 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Freezing"
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier:"A.3", 
            title: "When the temperature is between 1 and 20 degree celcius",
            description: "This test case verifies the **cool** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0", "1.0.1", "1.2.0")]
        [CoatHanger.RegressionRelease("1.1.0")]
        [CoatHanger.Requirement(FunctionalRequirement.BusinessRule)]
        public void WhenTemperatureBetweenOneAndLessThanTwenty_ExpectCool()
        {
            TestProcedure.Start(MethodBase.GetCurrentMethod());

            FormcastService service = new FormcastService();

            // arrange            
            int temperature = TestProcedure.GivenInput
            ( 
                variableName: nameof(temperature),
                valueOf: 15
            );

            // act 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // assert 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Cool"
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "A.4", 
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

            // arrange            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 20
            );

            // act 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // assert 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Mild"
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "A.5", 
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

            // arrange            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 24
            );

            // act 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // assert 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Mild"
            );
 
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "A.6", 
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

            // arrange            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 25
            );

            // act 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // assert 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Hot"
                , expectedResultNotMetMessage: "The system failed to display the hot temperature"
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "A.7", 
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

            // arrange            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 27
            );

            // act 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // assert 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Hot"
            );

        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "A.8",
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

            // arrange            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 30
            );

            // act 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // assert 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Scorching"
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
        (
            identifier: "A.9", 
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

            // arrange            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 31
            );

            // act 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // assert 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Scorching"
            );
        }

        [TestMethod]
        [CoatHanger.TestCase
       (
           manualTest: true,
           identifier: "A.10",
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
            identifier: "A.01",
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
    }
}
