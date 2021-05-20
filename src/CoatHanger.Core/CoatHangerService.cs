using CoatHanger.Core.Configuration;
using CoatHanger.Core.Enums;
using CoatHanger.Core.Models;
using CoatHanger.Core.Step;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CoatHanger.Core
{
    public class CoatHangerService
    {
        private Product Product { get; set; }
        private Assembly Assembly { get; set; }

        public IAuthorFormatter AuthorFormatter { get; internal set; } = new DefaultAuthorFormatter();
        public IReleaseVersionFormatter ReleaseVersionFormatter { get; internal set; } = new DefaultReleaseVersionFormatter();
        public string EvidencePath { get; internal set; } = ".";
        public string AttachmentPath { get; internal set; } = ".";

        public CoatHangerService(ProductArea product)
        {
            Product = new Product()
            {
                ProductID = product.ID,
                Title = product.Title,
                Summary = product.Summary
            };
        }

        public CoatHangerService(Product product)
        {
            Product = product;
        }

        public void AddTestCase(Assembly assembly, TestResultOutcome testResultOutcome, TestProcedure testProcedure)
        {
            // People might mix & match with coathanger test and normal unit test.
            // so if the documentation procedure was never started... just skip it and continue.
            if (testProcedure.IsStarted)
            {
                Assembly = assembly;
                // TODO: Incomplete. Need to map every attribute. 

                var classType = testProcedure.TestMethod.DeclaringType;
                var unitTestMethod = testProcedure.TestMethod;                

                //Class Only attributes 
                var functionAttribute = (AreaAttribute)Attribute.GetCustomAttribute(classType, typeof(AreaAttribute));

                if (functionAttribute == null)
                {
                    throw new ArgumentException($"The class {classType.FullName} does not have the required {nameof(AreaAttribute)}");
                }

                AddFeatureIfNotExist(functionAttribute);
                var function = AddFunctionIfNotExist(functionAttribute);

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
                        Title = testCaseAttribute.Title,
                        Description = testCaseAttribute.Description,
                        TestSteps = testProcedure.Steps,
                        TestingCategory = testCaseAttribute.Category,
                        TestingStyle = testCaseAttribute.Style,
                        PrerequisiteSteps = testProcedure.PrerequisiteSteps
                    };

                    if (testDesignerAttribute != null)
                    {
                        testCase.Author = new Author(testDesignerAttribute.UserName);
                        // TODO: Add username => to job title mapping table.
                    }

                    if (releaseAttribute != null)
                    {
                        testCase.Releases = releaseAttribute.ReleaseVersions;
                    }

                    if (regressionReleaseAttribute != null)
                    {
                        testCase.RegressionReleases = regressionReleaseAttribute.RegressionReleaseVersions;
                    }

                    // TODO: Add a paramaters to exclude this step. 
                    testCase.TestExecution = new TestExecution()
                    {
                        IsCompleted = testResultOutcome == TestResultOutcome.Passed,
                        ExecuteStartDate = testProcedure.TestExecutionStartDateTime.ToString("s"),
                        // Adding 1 second delay so that End is always ahead of Start. 
                        // Some unit test run faster than reasonable date format will allow.
                        ExecuteEndDate = DateTime.Now.AddSeconds(1).ToString("s")
                    };

                    testCase.TestStatus = testResultOutcome == TestResultOutcome.Passed 
                            ? TestStatus.Passed 
                            : TestStatus.Failed;

                    if (testProcedure is GivenWhenThenProcedure gwt)
                    {
                        var scenario = new GherkinScenario()
                        {
                            ScenarioID = testCase.TestCaseID,
                            Givens = gwt.Givens,
                            Whens = gwt.Whens,
                            Thens = gwt.Thens,
                            // We have to transform it due to hierarchy.
                            BusinessRules = gwt.BusinessRules
                                .Select(br => new BusinessRuleDTO()
                                {
                                    BusinessRuleID = br.ID,
                                    Title = br.Title,
                                    ParentID = br?.Parent?.ID
                                })
                                .ToList()
                        };

                        function.Scenarios.Add(scenario);
                    }

                    function.TestCases.Add(testCase);
                }
            }
        }

        private void AddFeatureIfNotExist(AreaAttribute functionAttribute)
        {
            if (!Product.Features.Any(f => f.FeatureID == functionAttribute.Area.ParentArea.ID))
            {
                lock (Product)
                {
                    var feature = new Feature()
                    {
                        FeatureID = functionAttribute.Area.ParentArea.ID,
                        Summary = functionAttribute.Area.ParentArea.Summary,
                        Title = functionAttribute.Area.ParentArea.Title,
                    };

                    Product.Features.Add(feature);
                }
            }
        }

        private Function AddFunctionIfNotExist(AreaAttribute functionAttribute)
        {
            var feature = Product.Features
                .Where(f => f.FeatureID == functionAttribute.Area.ParentArea.ID)
                .Single();

            if (!feature.Functions.Any(f => f.FunctionID == functionAttribute.Area.ID))
            {
                lock (Product)
                {
                    var function = new Function()
                    {
                        FunctionID = functionAttribute.Area.ID,
                        Summary = functionAttribute.Area.Summary,
                        Title = functionAttribute.Area.Title,
                    };

                    feature.Functions.Add(function);
                }
            }

            return feature.Functions
                .Where(f => f.FunctionID == functionAttribute.Area.ID)
                .Single();
        }


        public void Finish()
        {
            // TODO: Need a way for users to pass in sorting algorthm in the constructor. 

            Product.Features.Sort((x, y) => x.FeatureID.CompareTo(y.FeatureID));

            foreach (var feature in Product.Features)
            {
                foreach (var function in feature.Functions)
                {
                    function.Scenarios.Sort((x, y) => x.ScenarioID.CompareTo(y.ScenarioID));
                }

                feature.Functions.Sort((x, y) => x.FunctionID.CompareTo(y.FunctionID));
            }

            // serialize yaml directly to a file
            using (StreamWriter file = File.CreateText(@$"{Directory.GetCurrentDirectory()}/CoatHangerSpec.yaml"))
            {
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance)
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                    .Build();

                serializer.Serialize(file, Product);
            }
        }
    }

    public class CoatHangerServiceBuilder
    {
        Product Product { get; set; }
        private string DocumentationResourcePath { get; set; } = ".";
        private IAuthorFormatter AuthorFormatter { get; set; } = new DefaultAuthorFormatter();
        private IReleaseVersionFormatter ReleaseVersionFormatter { get; set; } = new DefaultReleaseVersionFormatter();

        public CoatHangerServiceBuilder(ProductArea product)
        {
            Product = new Product()
            {
                ProductID = product.ID,
                Title = product.Title,
                Summary = product.Summary
            };
        }

        public CoatHangerServiceBuilder SetResourcePath(string path)
        {
            DocumentationResourcePath = path;
            return this;
        }

        public CoatHangerServiceBuilder WithAuthorFormatter(IAuthorFormatter authorFormatter)
        {
            AuthorFormatter = authorFormatter;
            return this;
        }

        public CoatHangerServiceBuilder WithReleaseVersionFormatter(IReleaseVersionFormatter releaseVersionFormatter)
        {
            ReleaseVersionFormatter = releaseVersionFormatter;
            return this;
        }

        public CoatHangerService Build()
        {
            CoatHangerService service = new CoatHangerService(Product);
            service.AuthorFormatter = AuthorFormatter;
            service.ReleaseVersionFormatter = ReleaseVersionFormatter;
            service.AttachmentPath = $"{DocumentationResourcePath}/Attachments";
            service.EvidencePath = $"{DocumentationResourcePath}/Evidences"; ;

            return service;
        }

    }
}
