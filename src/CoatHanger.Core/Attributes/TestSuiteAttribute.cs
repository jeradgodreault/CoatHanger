using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{
    public class TestSuiteAttribute : Attribute
    {
        public string Title { get; private set; }
        public string Identifier { get; private set; }
        public string ParentSuiteIdentifier { get; private set; }

        public TestSuiteAttribute(string title)
        {
            Title = title;
        }

        public TestSuiteAttribute(string title, string identifier) : this(title)
        {
            Identifier = identifier;
        }

        public TestSuiteAttribute(string title, string identifier, string parentSuiteIdentifer) : this(title, identifier)
        {
            ParentSuiteIdentifier = parentSuiteIdentifer;
        }
    }
}
