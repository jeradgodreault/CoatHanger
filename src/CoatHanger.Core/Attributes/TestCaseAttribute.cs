using CoatHanger.Core.Enums;
using System;

namespace CoatHanger
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestCaseAttribute : Attribute
    {
        public string Identifier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TestingCategory Category { get; set; } = TestingCategory.Unit;
        public TestingStyle Style { get; set; } = TestingStyle.AdhocTesting;
        public bool IsManualTest { get; set; }

        public TestCaseAttribute()
        {

        }

        public TestCaseAttribute(string title, string identifier)
        {
            Identifier = identifier;
            Title = title;
        }

        public TestCaseAttribute(string title = null
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
    }
}
