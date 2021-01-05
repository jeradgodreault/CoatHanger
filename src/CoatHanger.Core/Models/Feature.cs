using CoatHanger.Core.Enums;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace CoatHanger.Core.Models
{
    /// <summary>
    /// A software solution that is created to solve a specific industry need or business problem.
    /// </summary>
    public class Product
    {
        public string ProductID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<Feature> Features { get; set; } = new List<Feature>();
    }

    /// <summary>
    /// Features are the "tools" you use within a system to complete a set of tasks or actions.
    /// </summary>
    public class Feature
    {
        public string FeatureID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<Function> Functions { get; set; } = new List<Function>();
    }

    /// <summary>
    /// Functionality is how those features actually work to provide you with a desired outcome.
    /// </summary>
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

        /// <summary>
        /// This field contains a short describes of the scenario of the test case
        /// </summary>
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string Scenario { get; set; }
        /// <summary>
        /// This field describes the test case objective.
        /// </summary>
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string Description { get; set; }

        public TestingCategory TestingCategory { get; set; }

        public List<string> Releases { get; set; }

        public List<string> RegressionReleases { get; set; }

        public List<string> WorkItems { get; set; }
        
        public Author Author { get; set; }

        public List<TestStep> TestSteps { get; set; }

        public TestExecution TestExecution { get; set; }
        
        [YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public bool IsAutomated { get; set; } = true; 
    }

    public class Release
    {

        public string ReleaseID { get; set; }

        public string Title { get; set; }
        
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string Description { get; set; }
    }

    public class TestExecution
    {
        public Author ExecutedBy { get; set; }
        public string ExecuteStartDate { get; set; }
        public string ExecuteEndDate { get; set; }
        public bool Outcome { get; set; }
    }

    public class PrerequisiteStep
    {
        public int StepNumber { get; set; }

        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string Description { get; set; }
    }

    public class TestStep
    {
        public int StepNumber { get; set; }

        [YamlMember(ScalarStyle = ScalarStyle.Literal)]        
        public string Action { get; set; }
        
        public ExpectedOutcome ExpectedOutcome { get; set; }

        [YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public bool IsSharedStep { get; set; } 
    }

    public class ExpectedOutcome
    {
        /// <summary>
        /// The Requirement ID for this expected result that the automated test can be traced back to. 
        /// If requirement id is not provided manually, it will follow the convention of `REQ {TestCaseID}.{ExpectedResultStepNumber}`
        /// </summary>
        public string RequirementID { get; set; }
        
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string Description { get; set; }
    }
}

