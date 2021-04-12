using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoatHanger.Core.Style
{
    public class ArrangeActAssertProcedure : TestProcedure
    {
        private OrderedDictionary Inputs { get; set; } = new OrderedDictionary();

        public ArrangeActAssertProcedure()
        {

        }

        public T GivenInput<T>(string variableName, T valueOf)
        {
            Inputs.Add(variableName, valueOf);

            AddStep(action: (Inputs.Count == 0) ? $"Given the { variableName } is { valueOf }." : "");

            return valueOf;
        }

        public T CallFunction<T>(string functionName, Func<T> function, List<string> inputVariables, string outputVariableName)
        {
            AddStep(action: $"Execute the function `{functionName}` with the input variables `{string.Join(",", inputVariables)}` and assign the value to the `{outputVariableName}` variable.");

            var result = function.Invoke();

            return result;
        }

        public T CallFunction<T>(string functionName, Func<T> function, string outputVariableName)
        {
            AddStep(action: $"Execute the function `{functionName}` and assign the value to the `{outputVariableName}` variable.");

            return function.Invoke();
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
            AddStep
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
            AddStep
            (
                action: $"Examine the {that} variable.",
                expectedResult: $"The system shall output the value {to}"
            );

            assertionMethod.Invoke(to, value, expectedResultNotMetMessage);
        }

        /// <summary>
        /// Verify **that** the variable is **asserted** **to** an expected value. 
        /// </summary>
        /// <param name="value">The variable **value** that we are Verify</param>
        /// <param name="assertionMethod">The Assertion method for verfication.</param>
        /// <param name="to">Expected value of verfication</param>
        /// <param name="expectedResultNotMetMessage">The override failure comment for not meeting the expected result </param>
        public void ThenVerify<T>(string action, string expectedResult, T value, Action<T, T, string> assertionMethod, T to, string expectedResultNotMetMessage)
        {
            AddStep
            (
                action: action,
                expectedResult: expectedResult
            );

            assertionMethod.Invoke(to, value, expectedResultNotMetMessage);
        }

    }
}
