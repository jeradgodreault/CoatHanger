using CoatHanger.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestCaseAttribute : TestMethodAttribute
    {
        public string Identifier { get; set; }
        
        public TestCaseAttribute(string testCaseTitle) : base(testCaseTitle) 
        {
            // Using TestMethodAttribute so that the VS test explorer shows the friendlier names. 
        }

        public TestCaseAttribute(string title, string identifier) : this(title)
        {
            Identifier = identifier;
        }

        public override Microsoft.VisualStudio.TestTools.UnitTesting.TestResult[] Execute(ITestMethod testMethod)
        {
            return base.Execute(testMethod);            
        }
    }
}
