﻿using CoatHanger.Core.Enums;
using System;
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
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
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
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
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
        public List<GherkinScenario> Scenarios { get; set; } = new List<GherkinScenario>();
        public List<TestCase> TestCases { get; set; } = new List<TestCase>();
    }

    public class GherkinScenario
    {
        public string ScenarioID { get; set; }
        public List<string> Givens { get; set; } = new List<string>();
        public List<string> Whens { get; set; } = new List<string>();
        public List<string> Thens { get; set; } = new List<string>();
        public Dictionary<string, List<string>> Example { get; set; }
        public List<BusinessRuleDTO> BusinessRules { get; set; } = new List<BusinessRuleDTO>();
    }

    public class GherkinExample
    {
        public List<GherkinColumn> Columns { get; set; }
        public Tuple<GherkinColumn, List<GherkinRow>> Rows { get; set; }
    }

    public class GherkinColumn
    {
        public string ColumnName { get; set; }
        public string DisplayText { get; set; }
    }

    public class GherkinRow
    {
        public string ColumnName { get; set; }
        public string Value { get; set; }
    }

    public class Scenario
    {
        public string ScenarioID { get; set; }
        public string Title { get; set; }
        public string CurrentVersion { get; set; }
        public string CreatedRelease { get; set; }
    }

    public class Requirement
    {
        public string RequirementID { get; set; }
        public string Title { get; set; }
    }

    public class BusinessRuleDTO
    {
        public string BusinessRuleID { get; set; }
        public string Title { get; set; }
        public string ParentID { get; set; }
    }

    public class TestCase
    {
        public string TestCaseID { get; set; }

        public List<PrerequisiteStep> PrerequisiteSteps { get; set; }

        /// <summary>
        /// This field contains a short describes of the scenario of the test case
        /// </summary>
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string Title { get; set; }
        /// <summary>
        /// This field describes the test case objective.
        /// </summary>
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string Description { get; set; }

        public TestingCategory TestingCategory { get; set; }

        public TestingStyle TestingStyle { get; set; }

        public List<string> Releases { get; set; }

        public List<string> RegressionReleases { get; set; }

        public List<string> WorkItems { get; set; }

        public Author Author { get; set; }

        public List<TestStep> TestSteps { get; set; }

        public TestExecution TestExecution { get; set; }

        [YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public bool IsAutomated { get; set; } = true;

        public TestStatus TestStatus { get; set; }
    }

    public enum TestStatus
    {
        Failed = 0,
        Passed = 1,
        Blocked = 2,
        NotRun = 3
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
        public bool IsCompleted { get; set; }
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
        public List<string> Actions { get; set; }

        public ExpectedOutcome ExpectedOutcome { get; set; }

        [YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        public bool IsSharedStep { get; set; }

        public List<Evidence> Evidences { get; set; }

        public string Comment { get; set; }
    }

    public class Evidence
    {
        public string FileName { get; set; }
        public EvidenceType EvidenceType { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public enum EvidenceType
    {
        OTHER = 0,
        CSV = 10,
        TXT_LOG = 20,
        JPEG_IMAGE = 30,
        PNG_IMAGE = 31,
        GIF_IMAGE = 32,
    }

    public class ExpectedOutcome
    {
        /// <summary>
        /// The Requirement ID for this expected result that the automated test can be traced back to. 
        /// If requirement id is not provided manually, it will follow the convention of `REQ {TestCaseID}.{ExpectedResultStepNumber}`
        /// </summary>
        public string RequirementID { get; set; }

        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string ExpectedResult { get; set; }
    }
}

