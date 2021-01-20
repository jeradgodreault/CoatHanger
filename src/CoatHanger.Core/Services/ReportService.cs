using CoatHanger.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CoatHanger.Core
{
    public class ReportService
    {
        public string TestCaseResultHtmlTemplate { get; set; }


        public string GetTestResult(Product product)
        {

            // TODO: Paramaterize paths so users can provide their own. 

            // Fetch the templates htmls.
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string layoutFilePath = Path.Combine(assemblyPath, @"Template\_layout.html");
            string testCaseReportFilePath = Path.Combine(assemblyPath, @"Template\TestCaseResultDetailReport.html");
            //string traceabilityMatrixFilePath = Path.Combine(assemblyPath, @"Template\TraceabilityMatrixDetailReport.html");

            // TODO Cache templates.
            string layoutHtml = File.ReadAllText(layoutFilePath);
            TestCaseResultHtmlTemplate = File.ReadAllText(testCaseReportFilePath);


            string reportHtml = "";
            //reportHtml += testCaseResultHtml;

            // There always a system layer
            reportHtml += DoSomething(product); // System suite is always the top node.





            return layoutHtml.Replace("<div id='coat-hanger-content-body' />", $@"<div id='coat-hanger-content-body'>{reportHtml}</div>");
        }

        public TraceabilityMatrixTableDTO ConvertToDto(Feature feature)
        {
            var matrix = new TraceabilityMatrixTableDTO();
            matrix.Requirements = new List<RequirementDTO>();
            matrix.TestCases = new List<TestCaseDTO>();

            foreach (var testCase in feature.Functions
                .Where(ts => ts.Scenarios.Count > 0)
                .SelectMany(ts => ts.Scenarios)
                .SelectMany(ts=> ts.TestCases)
            )
            {
                var requirements = testCase.TestSteps
                    .Where(ts => ts.ExpectedOutcome != null)
                    .Select(step => new RequirementDTO()
                    {
                        RequirementID = $"{testCase.TestCaseID}-{step.ExpectedOutcome.RequirementID}",
                        RequirementTitle = step.ExpectedOutcome.ExpectedResult
                    })
                    .ToList();

                var testCaseDto = new TestCaseDTO()
                {
                    TestCaseID = testCase.TestCaseID,
                    TestCaseDescription = testCase.Description,
                    TraceableRequirements = requirements,
                    TraceableRequirementCount = requirements.Count
                };

                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                // TODO: There needs to be a way to link other requirements as either pre-conditions or shared steps
                // From a matrix traceability point of view, almost all pages that require login step should be traced to Login Test Case (success). 
                // If we don't do this, the matrix will just look like one giant diagonal line. This is bad because you **should** have more than 1 test case
                // exercising a requirement.  
                // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                matrix.Requirements.AddRange(requirements);
                matrix.TestCases.Add(testCaseDto);
            }

            foreach (var testcase in matrix.TestCases)
            {
                testcase.RequirementMatrix = matrix.Requirements
                    .Select(r => new RequirementMatrixResultDTO()
                    {
                        Requirement = r,
                        IsTraceable = testcase.TraceableRequirements.Contains(r)
                    })
                    .ToList();
            }
          

            return matrix;
        }

        private string DoSomething(Product currentSuite)
        {
            var result = "";

            foreach (var feature in currentSuite.Features)
            {
                result += $"<h1>{feature.Title}</h1> \n\n";

                foreach (var function in feature.Functions)
                {
                    result += "<div class='indent'>\n\t";
                    result += $"<h2>{function.Title}</h2> \n\n";

                    foreach (var testCase in function.Scenarios)
                    {
                        result += Nustache.Core.Render.StringToString(TestCaseResultHtmlTemplate, testCase);
                    }
                    result += "</div>";
                }
            }

            return result;
        }
    }
}
