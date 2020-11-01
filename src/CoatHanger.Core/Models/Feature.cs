using System.Collections.Generic;

namespace CoatHanger.Core.Models
{

    public class Product
    {
        public string ProductID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<Feature> Features { get; set; } = new List<Feature>();
    }

    public class Feature
    {
        public string FeatureID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<Function> Functions { get; set; } = new List<Function>();
    }

    public class Function
    {
        public string FunctionID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<TestCase> TestCases { get; set; } = new List<TestCase>();
    }

    public class TestCase
    {
        public string TestCaseID { get; set; }
        public string Scenario { get; set; }
        /// <summary>
        /// This field describes the test case objective.
        /// </summary>
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
        public bool IsSharedStep { get; set; } 
    }

    public class ExpectedOutcome
    {
        public int StepNumber { get; set; }
        public string Description { get; set; }
    }
}

