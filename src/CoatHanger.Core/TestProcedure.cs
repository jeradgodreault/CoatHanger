using CoatHanger.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CoatHanger.Core
{


    public class TestProcedure
    {
        private List<TestStep> Steps { get; set; } = new List<TestStep>();
        private OrderedDictionary Inputs { get; set; } = new OrderedDictionary();
        private int StepNumber { get; set; } = 1;
        private int ExpectedResultStepNumber { get; set; } = 1;

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

        public T CallFunction<T>(string functionName, Func<T> function, string outputVariableName)
        {
            AddManualStep(action: $"Execute the function `{functionName}` and assign the value to the `{outputVariableName}` variable.");

            return function.Invoke();
        }

        public void AddManualStep(string action)
        {
            Steps.Add(new TestStep()
            {
                StepNumber = StepNumber++,
                Action = action,
            });
        }


        public void AddManualStep(string action, string expectedResult)
        {
            Steps.Add(new TestStep()
            {
                StepNumber = StepNumber++,
                Action = action,
                ExpectedOutcome = new ExpectedOutcome()
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

        public string ToJson()
        {
            return JsonConvert.SerializeObject(Steps);
        }
    }
}
