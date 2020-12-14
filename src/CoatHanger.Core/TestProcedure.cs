using CoatHanger.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoatHanger.Core
{

    public class TestProcedure
    {
        public List<TestStep> Steps { get; private set; } = new List<TestStep>();
        public List<PrerequisiteStep> PrerequisiteSteps { get; private set; } = new List<PrerequisiteStep>();
        private int PrerequisiteStep { get; set; } = 1;
        private OrderedDictionary Inputs { get; set; } = new OrderedDictionary();
        private int StepNumber { get; set; } = 1;
        private int ExpectedResultStepNumber { get; set; } = 1;
        
        public DateTime TestExecutionStartDateTime { get; private set; }

        /// <summary>
        /// Determine if all the steps will be consider as shared step. 
        /// </summary>
        public bool IsSharedStepMode { get; set; }

        public TestProcedure()
        {
            // use the dedicate Start() method if you want to exclude the testing code setup 
            TestExecutionStartDateTime = DateTime.Now;
        }

        public T GivenInput<T>(string variableName, T valueOf)
        {
            Inputs.Add(variableName, valueOf);

            AddManualStep(action: $"Set the variable { variableName } to { valueOf }.");

            return valueOf;
        }

        public T CallFunction<T>(string functionName, Func<T> function, List<string> inputVariables, string outputVariableName)
        {
            AddManualStep(action: $"Execute the function `{functionName}` with the input variables `{string.Join(",", inputVariables)}` and assign the value to the `{outputVariableName}` variable.");

            return function.Invoke();
        }

        public void StartTesting()
        {
            TestExecutionStartDateTime = DateTime.Now;
        }

        public T CallFunction<T>(string functionName, Func<T> function, string outputVariableName)
        {
            AddManualStep(action: $"Execute the function `{functionName}` and assign the value to the `{outputVariableName}` variable.");

            return function.Invoke();
        }

        public void AddPrerequisiteStep(string description)
        {
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException("Prerequisite step description cannot be null or empty. Please provide a value.");

            PrerequisiteSteps.Add(new PrerequisiteStep()
            {
                StepNumber = PrerequisiteStep++,
                Description = description
            });
        }

        public void AddPrerequisiteStep(string description, Action setupPrerequisiteAction)
        {
            AddPrerequisiteStep(description);

            try
            {
                setupPrerequisiteAction.Invoke();
            }
            catch (Exception ex)
            {
                new Exception($"Unable to setup the Prerequisite Step - `${description}`", ex);
            }
        }

        public void AddManualStep(params string[] actions)
        {
            AddManualStep(string.Join(Environment.NewLine + Environment.NewLine, actions));
        }

        public void AddManualStep(string action)
        {
            Steps.Add(new TestStep()
            {
                StepNumber = StepNumber++,
                Action = action,
                IsSharedStep = IsSharedStepMode
            });
        }

        public void AddManualStep(string action, params string[] expectedResults)
        {
            AddManualStep(action, string.Join(Environment.NewLine + Environment.NewLine, expectedResults));
        }

        public void AddManualStep(string action, string expectedResult)
        {
            Steps.Add(new TestStep()
            {
                StepNumber = StepNumber++,
                Action = action,
                IsSharedStep = IsSharedStepMode,
                ExpectedOutcome = new ExpectedOutcome
                {
                    StepNumber = ExpectedResultStepNumber++,
                    Description = expectedResult,
                }
            });
        }

        /// <summary>
        /// Verify **that** the variable is **asserted** **to** an expected value.
        /// </summary>
        /// <param name="that">The variable **name** that we are Verify</param>
        /// <param name="value">The variable **value** that we are Verify</param>
        /// <param name="assertionMethod">The Assertion method for verfication.</param>
        /// <param name="to">Expected value of verfication</param>
        public void ThenVerify<T>(string that, T value, Action<T, T> assertionMethod, T to)
        {
            AddManualStep
            (
                action: $"Examine the {that} variable.", 
                expectedResult: $"The system shall output the value {to}"
            );

            assertionMethod.Invoke(to, value);
        }

        /// <summary>
        /// Verify **that** the variable is **asserted** **to** an expected value. 
        /// </summary>
        /// <param name="that">The variable **name** that we are Verify</param>
        /// <param name="value">The variable **value** that we are Verify</param>
        /// <param name="assertionMethod">The Assertion method for verfication.</param>
        /// <param name="to">Expected value of verfication</param>
        /// <param name="expectedResultNotMetMessage">The override failure comment for not meeting the expected result </param>
        public void ThenVerify<T>(string that, T value, Action<T, T, string> assertionMethod, T to, string expectedResultNotMetMessage) 
        {
            AddManualStep
            (
                action: $"Examine the {that} variable.",
                expectedResult: $"The system shall output the value {to}"
            );

            assertionMethod.Invoke(to, value, expectedResultNotMetMessage);
        }


        public string ToJson()
        {
            return JsonConvert.SerializeObject(Steps);
        }
    }
}
