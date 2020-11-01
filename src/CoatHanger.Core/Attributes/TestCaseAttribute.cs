using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoatHanger
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestCaseAttribute : TestMethodAttribute
    {
        public string Identifier { get; set; }
        public string Description { get; set; }

        public TestCaseAttribute(string scenario) : base(scenario)
        {
            // Using TestMethodAttribute so that the VS test explorer shows the friendlier names. 
        }

        public TestCaseAttribute(string scenario, string identifier) : this(scenario)
        {
            Identifier = identifier;
        }

        public TestCaseAttribute(string scenario, string identifier = null, string description = null) 
            : this(scenario, identifier)
        {
            Description = description;
        }

        public override Microsoft.VisualStudio.TestTools.UnitTesting.TestResult[] Execute(ITestMethod testMethod)
        {
            return base.Execute(testMethod);
        }
    }
}
