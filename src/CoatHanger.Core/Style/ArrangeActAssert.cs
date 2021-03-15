using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace CoatHanger.Core.Style
{
    public class ArrangeActAssert
    {
        private TestProcedure TestProcedure { get; }
        private OrderedDictionary Inputs { get; set; } = new OrderedDictionary();

        public ArrangeActAssert(TestProcedure testProcedure)
        {
            TestProcedure = testProcedure;
        }

        public T GivenInput<T>(string variableName, T valueOf)
        {
            Inputs.Add(variableName, valueOf);

            TestProcedure.AddStep(action: (Inputs.Count == 0) ? $"Given the { variableName } is { valueOf }." : "");

            return valueOf;
        }

        public T CallFunction<T>(string functionName, Func<T> function, List<string> inputVariables, string outputVariableName)
        {
            TestProcedure.AddStep(action: $"Execute the function `{functionName}` with the input variables `{string.Join(",", inputVariables)}` and assign the value to the `{outputVariableName}` variable.");

            var result = function.Invoke();

            return result;
        }

        public T CallFunction<T>(string functionName, Func<T> function, string outputVariableName)
        {
            TestProcedure.AddStep(action: $"Execute the function `{functionName}` and assign the value to the `{outputVariableName}` variable.");

            return function.Invoke();
        }

    }
}
