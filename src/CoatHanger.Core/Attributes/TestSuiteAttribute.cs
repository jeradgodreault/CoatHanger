using CoatHanger.Core;
using System;

namespace CoatHanger
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false)]
    public class TestSuiteAttribute : Attribute
    {
        public Type TestSuite { get; set; }

        /// <summary>
        /// Mark the test suite the methods should fall under. 
        /// For example, [TestSuite(suitClass= typeOf(WeatherForcastSuite)], 
        /// Where WeatherForcastSuite inherit (directly or indirectly) of SystemSpecification class.  
        /// </summary>
        /// <example>
        /// Represent a tree hierarchy of test suite. 
        /// <code>
        ///  -- System Suite
        ///  |-- Weather Forecast Suite
        ///  |   |-- Test Case A.1
        ///  |   |-- Test Case A.2
        ///  |   |-- Temperature Calculation Suite
        ///  |   |   |-- Test Case B.1
        ///  |   |   `-- Test Case B.2
        ///  |   `-- Weather Grid Suite
        ///  |       |-- Test Case C.1
        ///  |       `-- Test Case C.2
        ///  `-- About Suite
        ///      -- Layout Suite
        ///          |-- Test Case D.1
        ///          `-- Test Case D.2
        /// </code>
        /// </example>
        public TestSuiteAttribute(Type testSuiteType) 
        {
            // null guard
            if (testSuiteType == null) throw new ArgumentNullException($"You cannot pass null into the {nameof(TestSuiteAttribute)} attribute.");


            if (testSuiteType.IsSubclassOf(typeof(SystemSpecification)))
            {
                TestSuite = testSuiteType;
            } else
            {
                throw new ArgumentException($"The type {testSuiteType.FullName} is an invalid parameter for {nameof(TestSuiteAttribute)}. " +
                    $"It must inherit the {nameof(SystemSpecification)} class.");
            }
        }
    }
}
