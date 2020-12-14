using System.Collections.Generic;

namespace CoatHanger.Core.Models
{
    public class TestCaseTableDTO
    {
        public List<FeatureDTO> Features { get; set; }
    }

    public class FeatureDTO
    {
        public string FeatureID { get; set; }
        public string FeatureTitle { get; set; }        
    }

    public class FunctionDTO
    {
        public string FunctionID { get; set; }
        public string FunctionTitle { get; set; }
        public List<RequirementDTO> Requirements { get; set; }
    }

    public class TraceabilityMatrixTableDTO
    {
        public List<RequirementDTO> Requirements { get; set; }
        public List<TestCaseDTO> TestCases { get; set; }
    }

    public class RequirementDTO
    {
        public string RequirementID { get; set; }
        public string RequirementTitle { get; set; }
    }

    public class TestCaseDTO
    {
        public string TestCaseID { get; set; }
        public string TestCaseTitle { get; set; }
        public string TestCaseDescription { get; set; }
        public List<RequirementMatrixResultDTO> RequirementMatrix { get; set; }
        public List<RequirementDTO> TraceableRequirements { get; set; }
        public int TraceableRequirementCount { get; set; }
    }

    public class RequirementMatrixResultDTO
    {
        public RequirementDTO Requirement { get; set; } 
        public bool IsTraceable { get; set; }
    }
}
