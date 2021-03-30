using CoatHanger.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoatHanger
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestCaseAttribute : TestMethodAttribute
    {
        public string Identifier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TestingCategory Category { get; set; } = TestingCategory.Unit;
        public TestingStyle Style { get; set; } = TestingStyle.AdhocTesting;
        public bool IsManualTest { get; set; }

        public TestCaseAttribute() : base()
        {

        }

        public TestCaseAttribute(string title) : base(title)
        {
            // Using TestMethodAttribute so that the VS test explorer shows the friendlier names. 
        }

        public TestCaseAttribute(string title, string identifier) : this($"{identifier} - {title}")
        {
            Identifier = identifier;
        }

        public TestCaseAttribute(string title
            , string identifier = null
            , string description = null
            , TestingCategory testingCategory = TestingCategory.Unit
            , TestingStyle testingStyle = TestingStyle.AdhocTesting
            , bool manualTest = false
            ) 
            : this(title, identifier)
        {
            Description = description;
            Category = testingCategory;
            Style = testingStyle;
            IsManualTest = manualTest;
        }

        public override Microsoft.VisualStudio.TestTools.UnitTesting.TestResult[] Execute(ITestMethod testMethod)
        {
            return base.Execute(testMethod);
        }
    }
}
