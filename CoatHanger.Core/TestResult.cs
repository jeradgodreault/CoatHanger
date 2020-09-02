using CoatHanger.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{
    public class TestResult
    {
        public string TestCaseID { get; set; }
        public string TestCase { get; set;  }
        public List<TestStep> TestSteps { get; set; }
        public Author Author { get; set; }
    }

    public class TestStep
    {
        public int StepNumber { get; set; }
        public string Action { get; set; }
        public string ExpectedResult { get; set; }
        public bool Outcome { get; set; }
        public DateTime ExecuteStartDate { get; set; }
        public DateTime ExecuteEndDate { get; set; }
        public Author ExecutedBy { get; set; }
    }
}
