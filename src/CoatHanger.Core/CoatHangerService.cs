using CoatHanger.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace CoatHanger.Core
{
    public class CoatHangerService
    {
        private Product Product { get; set; }
        // TODO: Remove, everything should be from the product object
        private List<TestCase> TestCases { get; set; } = new List<TestCase>();

        public CoatHangerService()
        {
            Product = new Product(); // TODO: Build product object with framework.
        }

        public void AddTestCase(Assembly assembly, TestContext testContext, TestProcedure testProcedure)
        {
            // TODO: Incomplete. Need to map every attribute. 

            var classType = assembly.GetType(testContext.FullyQualifiedTestClassName);
            var unitTestMethod = classType.GetMethod(testContext.TestName);

            //Class Only attributes 
            //var function = (FunctionAttribute)Attribute.GetCustomAttribute(classType, typeof(FunctionAttribute));

            //Method Only attributes 
            var testCaseAttribute = (TestCaseAttribute)Attribute.GetCustomAttribute(unitTestMethod, typeof(TestCaseAttribute));
            var testDesignerAttribute = (TestDesignerAttribute)Attribute.GetCustomAttribute(unitTestMethod, typeof(TestDesignerAttribute));
            var requirementAttribute = (RequirementAttribute)Attribute.GetCustomAttribute(unitTestMethod, typeof(RequirementAttribute));
            var releaseAttribute = (ReleaseAttribute)Attribute.GetCustomAttribute(unitTestMethod, typeof(ReleaseAttribute));
            var regressionReleaseAttribute = (RegressionReleaseAttribute)Attribute.GetCustomAttribute(unitTestMethod, typeof(RegressionReleaseAttribute));

            if (testCaseAttribute != null)
            {
                TestCase testCase = new TestCase()
                {
                    TestCaseID = testCaseAttribute.Identifier,
                    Scenario = testCaseAttribute.DisplayName,
                    Description = testCaseAttribute.Description,
                    TestSteps = testProcedure.Steps,

                    TestExecution = new TestExecution()
                    {
                        Outcome = (testContext.CurrentTestOutcome == UnitTestOutcome.Passed),
                        ExecuteStartDate = testProcedure.TestExecutionStartDateTime.ToString("s"),
                        // Adding 1 second delay so that End is always ahead of Start. 
                        // Some unit test run faster than reasonable date format will allow.
                        ExecuteEndDate = DateTime.Now.AddSeconds(1).ToString("s") 
                    }
                };

                TestCases.Add(testCase);
            }
        }

        public void Finish()
        {
            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(@"c:\temp\CoatHangerSpec.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.Serialize(file, TestCases);
            }
        }

    }
}
