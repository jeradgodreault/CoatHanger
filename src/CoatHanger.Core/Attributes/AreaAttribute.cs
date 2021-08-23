using CoatHanger.Core;
using System;

namespace CoatHanger
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AreaAttribute : Attribute
    {
        public IAreaPath Area { get; set; }

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
        public AreaAttribute(Type areaType)
        {
            // null guard
            if (areaType == null) throw new ArgumentNullException($"You cannot pass null into the {nameof(AreaAttribute)} attribute.");

            if (typeof(IAreaPath).IsAssignableFrom(areaType))
            {
                Area = (IAreaPath)Activator.CreateInstance(areaType);
            }
            else
            {
                throw new ArgumentException($"The type {areaType.FullName} is an invalid parameter for {nameof(AreaAttribute)}. " +
                    $"It must implement the {nameof(IAreaPath)} interface.");
            }
        }
    }
}
