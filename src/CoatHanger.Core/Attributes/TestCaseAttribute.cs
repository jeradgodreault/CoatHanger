using CoatHanger.Core.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{
    public class TestCaseAttribute : TestMethodAttribute
    {
        public string Identifier { get; set; }
        
        public TestCaseAttribute(string title) : base(title) 
        {
            
        }

        public TestCaseAttribute(string title, string identifier) : this(title)
        {
            Identifier = identifier;
        }

        public override Microsoft.VisualStudio.TestTools.UnitTesting.TestResult[] Execute(ITestMethod testMethod)
        {
            var results = new Microsoft.VisualStudio.TestTools.UnitTesting.TestResult[] { testMethod.Invoke(null) };
            return results;
        }
    }
}
