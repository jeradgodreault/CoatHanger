using System.Collections.Generic;

namespace CoatHanger.Core.Models
{
    public class TestSuite
    {
        public string TestSuiteID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<TestSuite> TestSuites { get; set; } = new List<TestSuite>();
        public List<TestCase> TestCases { get; set; } = new List<TestCase>();
    }

    public class TestCase
    {
        public string TestCaseID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Release> Releases { get; set; }
        public List<string> WorkItems { get; set; }
        public Author Author { get; set; }
        public List<TestStep> TestSteps { get; set; }

        public TestExecution TestExecution { get; set; }

        public bool IsAutomated { get; set; } = true;
    }

    public class Release
    {
        public string ReleaseID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class TestExecution
    {
        public Author ExecutedBy { get; set; }
        public string ExecuteStartDate { get; set; }
        public string ExecuteEndDate { get; set; }
        public bool Outcome { get; set; }
    }

    public class TestStep
    {
        public int StepNumber { get; set; }
        public string Action { get; set; }
        public ExpectedOutcome ExpectedOutcome { get; set; }
    }

    public class ExpectedOutcome
    {
        public int StepNumber { get; set; }
        public string Description { get; set; }
    }


    // mustache variables 


}

