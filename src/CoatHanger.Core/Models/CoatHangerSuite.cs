using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core.Models
{
    public class CoatHangerSuite
    {
        public string Identifier { get; set; }
        public string Title { get; set; }
        public List<CoatHangerSuite> TestSuites { get; set; } = new List<CoatHangerSuite>();
        public List<CoatHangerTestCase> TestCases { get; set; } = new List<CoatHangerTestCase>();
    }

    public class CoatHangerTestCase
    {
        public string Identifier { get; set; }
        public string Title { get; set; }
        public string CreatedReleaseVersion { get; set; }
        public string ModifiedReleaseVersion { get; set; }
        public List<string> WorkItems { get; set; }
        public Author Author { get;set; }
        public List<CoatHangerTestStep> TestSteps { get; set; }
        

    }

    public class CoatHangerExecution
    {
        public Author ExecutedBy { get; set; }
        public string ExecuteStartDate { get; set; }
        public string ExecuteEndDate { get; set; }        
        public bool Outcome { get; set; }
    }

    public class CoatHangerTestStep
    {
        public int StepNumber { get; set; }
        public string Action { get; set; }
        public CoatHangerExpectedResult ExpectedOutcome { get; set; }
    }

    public class CoatHangerExpectedResult
    {
        public int StepNumber { get; set; }
        public string Title { get; set; }
    }
}

