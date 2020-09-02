using CoatHanger.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{
    public class TestCaseAttribute : System.Attribute 
    {
        public string Identifier { get; set; }
        public string Title { get; set; }
        public VerificationCategory VerificationCategory { get; set; }
        public string Author { get; set; }



        public TestCaseAttribute(string title)
        {
            Title = title;
        }

        public TestCaseAttribute(string title, string identifier) : this(title)
        {
            Identifier = identifier;
        }
    }
}
