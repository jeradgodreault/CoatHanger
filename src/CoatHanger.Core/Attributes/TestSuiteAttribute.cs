using System;

namespace CoatHanger
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false)]
    public class TestSuiteAttribute : Attribute
    {
        public Type SuiteType { get; set; }

        public TestSuiteAttribute(Type suiteClass)
        {
            SuiteType = suiteClass;

            //if (suiteType is ISuite)
            //{
            //    var instance = (ISuite)Activator.CreateInstance(suiteType);

            //} else
            //{
            //    throw new ArgumentException($"The type {suiteType.FullName} is an invalid parameter for {nameof(TestSuiteAttribute)}. " +
            //        $"It must implement the {nameof(ISuite)} interface.");
            //}
        }
    }
}
