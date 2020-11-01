using CoatHanger.Core;
using System;

namespace CoatHanger
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false)]
    public class FunctionAttribute : Attribute
    {
        public Type Function { get; set; }

        /// <summary>
        /// Mark the test suite the methods should fall under. 
        /// For example, [TestFunction(functionType= typeOf(TemperatureCalculationFunction)], 
        /// Where TemperatureCalculationFunction inherit (directly or indirectly) of SystemSpecification class.  
        /// </summary>
        /// <example>
        /// Represent a tree hierarchy of product feature/function/scenario . 
        /// <code>
        ///  -- System Suite
        ///  |-- Weather Forecast Feature
        ///  |   |-- Temperature Calculation Function
        ///  |   |   |-- Test Case: Scenario A.1
        ///  |   |   `-- Test Case: Scenario A.2
        ///  |   `-- Weather Grid Function 
        ///  |       |-- Test Case: Scenario B.1
        ///  |       `-- Test Case: Scenario B.2
        ///  `-- About Feature
        ///      -- Layout Function
        ///          |-- Test Case: Scenario C.1
        ///          `-- Test Case: Scenario C.2
        /// </code>
        /// </example>
        public FunctionAttribute(Type functionType) 
        {
            // null guard
            if (functionType == null) throw new ArgumentNullException($"You cannot pass null into the {nameof(FunctionAttribute)} attribute.");


            if (functionType.IsSubclassOf(typeof(SystemSpecification)))
            {
                Function = functionType;
            } else
            {
                throw new ArgumentException($"The type {functionType.FullName} is an invalid parameter for {nameof(FunctionAttribute)}. " +
                    $"It must inherit the {nameof(SystemSpecification)} class.");
            }
        }
    }
}
