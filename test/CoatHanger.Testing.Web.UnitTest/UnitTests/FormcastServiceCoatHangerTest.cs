using CoatHanger.Core;
using CoatHanger.Core.Models;
using CoatHanger.Testing.Web.Services.WeatherForcast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;



namespace CoatHanger.Testing.Web.UnitTest
{

    /// <summary>
    /// Purpose of this test is to show everything using the Coat Hanger apporach. 
    /// Use the <see cref="FormcastServiceMsTest"/> as example of MSTest implementation. 
    /// </summary>
    [TestClass]
    [CoatHanger.Function(functionType: typeof(TemperatureCalculationFunction))]
    public class FormcastServiceCoatHangerTest
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

        [CoatHanger.TestCase
        (
            identifier: "A.1", 
            scenario: "When the temperature is less than zero degree celcius",
            description: "This test case verifies the **freezing** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("godreaj")]
        [CoatHanger.Release("1.0.0")]
        public void WhenTemperatureIsLessThanZero_ExpectFreezing()
        {
            FormcastService service = new FormcastService();

            // given            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: -1
            );

            // when 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
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
        public void WhenTemperatureIsZero_ExpectFreezing()
        {
            FormcastService service = new FormcastService();

            // given            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 0
            );

            // when 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
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
            identifier:"A.3", 
            scenario: "When the temperature is between 1 and 20 degree celcius",
            description: "This test case verifies the **cool** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        [CoatHanger.RegressionTesting("1.1.0")]
        public void WhenTemperatureBetweenOneAndLessThanTwenty_ExpectCool()
        {
            FormcastService service = new FormcastService();

            // given            
            int temperature = TestProcedure.GivenInput
            ( 
                variableName: nameof(temperature),
                valueOf: 15
            );

            // when 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // then 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Cool"
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
        public void WhenTemperatureExactlyTwenty_ExpectMild()
        {
            FormcastService service = new FormcastService();

            // given            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 20
            );

            // when 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // then 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Mild"
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
        public void WhenTemperatureLessThanTwentyFive_ExpectMild()
        {
            FormcastService service = new FormcastService();

            // given            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 24
            );

            // when 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
            );

            // then 
            TestProcedure.ThenVerify
            (
                  that: nameof(result)
                , value: result
                , AreEqual
                , to: "Mild"
            );
 
        }


        [CoatHanger.TestCase
        (
            identifier: "A.6", 
            scenario: "When the temperature is exactly 25 degree celcius",
            description: "This test case verifies the **mild** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        public void WhenTemperatureExactlyThanTwentyFive_ExpectMild()
        {
            FormcastService service = new FormcastService();

            // given            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 25
            );

            // when 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
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
            identifier: "A.7", 
            scenario: "When the temperature is between 25 and 29 degree celcius",
            description: "This test case verifies the **hot** temperature label is being calculated correctly."
        )]
        [CoatHanger.TestDesigner("smithj")]
        [CoatHanger.Release("1.0.0")]
        public void WhenTemperatureBetweenTwentyFiveAndLessThanThirty_ExpectHot()
        {
            FormcastService service = new FormcastService();

            // given            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 27
            );

            // when 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
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
        public void WhenTemperatureExactlyThirty_ExpectScorching()
        {
            FormcastService service = new FormcastService();

            // given            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 30
            );

            // when 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
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
        public void WhenTemperatureGreaterThanThirty_ExpectScorching()
        {
            FormcastService service = new FormcastService();

            // given            
            int temperature = TestProcedure.GivenInput
            (
                variableName: nameof(temperature),
                valueOf: 31
            );

            // when 
            string result = TestProcedure.CallFunction
            (
                functionName: nameof(FormcastService.GetTemperatureSummary),
                function: () => service.GetTemperatureSummary(temperature),
                inputVariables: new List<string> { nameof(temperature) },
                outputVariableName: nameof(result)
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
    }
}
