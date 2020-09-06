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


        public string GetTestResult(TestSuite suite)
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
            reportHtml += DoSomething(suite, depth: 1); // System suite is always the top node.





            return layoutHtml.Replace("<div id='coat-hanger-content-body' />", $@"<div id='coat-hanger-content-body'>{reportHtml}</div>");
        }


        private string DoSomething(TestSuite currentSuite, int depth)
        {
            var result = "";

            foreach (var childSuite in currentSuite.TestSuites)
            {
                result += "<div class='indent'>\n\t";
                result += $"<h{depth}>{childSuite.Title}</h{depth}> \n\n";

                foreach (var testCase in childSuite.TestCases)
                {
                    result += Nustache.Core.Render.StringToString(TestCaseResultHtmlTemplate, testCase);
                }


                if (childSuite.TestSuites.Any())
                {
                    result += DoSomething(childSuite, depth + 1);
                }

                result += "</div>";
            }
            return result;
        }
    }


    public static class Helpers
    {
        public static IEnumerable<TestSuite> Descendants(this TestSuite testSuite)
        {
            return testSuite.TestSuites.Concat(testSuite.TestSuites.SelectMany(n => n.TestSuites));
        }
    }
}
