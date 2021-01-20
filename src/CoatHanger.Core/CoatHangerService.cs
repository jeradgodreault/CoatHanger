using CoatHanger.Core.Configuration;
using CoatHanger.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;
using YamlDotNet.Serialization.NamingConventions;

namespace CoatHanger.Core
{
    public class CoatHangerService
    {
        private Product Product { get; set; }
        
        public IAuthorFormatter AuthorFormatter { get; set; } = new DefaultAuthorFormatter();
        public IReleaseVersionFormatter ReleaseVersionFormatter { get; set; } = new DefaultReleaseVersionFormatter();
        public CoatHangerService(IProduct product)
        {
            Product = new Product()
            {
                ProductID = product.ProductID,
                Title = product.Title,
                Summary = product.Summary
            };
        }

        public CoatHangerService(Product product)
        {
            Product = product;
        }         

        public void AddTestCase(Assembly assembly, TestContext testContext, TestProcedure testProcedure)
        {
            // TODO: Incomplete. Need to map every attribute. 

            var classType = assembly.GetType(testContext.FullyQualifiedTestClassName);
            var unitTestMethod = classType.GetMethod(testContext.TestName);
            
            //Class Only attributes 
            var functionAttribute = (FunctionAttribute)Attribute.GetCustomAttribute(classType, typeof(FunctionAttribute));

            if (functionAttribute == null)
            {
                throw new ArgumentException($"The class {classType.FullName} does not have the required {nameof(FunctionAttribute)}");
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
                    Title = testCaseAttribute.DisplayName,
                    Description = testCaseAttribute.Description,
                    TestSteps = testProcedure.Steps,
                    TestingCategory = testCaseAttribute.Category

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
                    IsCompleted = (testContext.CurrentTestOutcome == UnitTestOutcome.Passed),
                    ExecuteStartDate = testProcedure.TestExecutionStartDateTime.ToString("s"),
                    // Adding 1 second delay so that End is always ahead of Start. 
                    // Some unit test run faster than reasonable date format will allow.
                    ExecuteEndDate = DateTime.Now.AddSeconds(1).ToString("s")
                };

                var scenario = new Scenario()
                {
                    ScenarioID = testCase.TestCaseID,
                    Title = testCase.Title,
                    CurrentVersion = releaseAttribute.LastestRelease,
                    CreatedRelease = releaseAttribute.CreatedRelease,
                    TestCases = new List<TestCase>() { testCase },

                    Requirements = testCase.TestSteps
                        .Where(ts => ts.ExpectedOutcome != null)
                        .Select(ts => new Requirement()
                        {
                            RequirementID = ts.ExpectedOutcome.RequirementID,
                            Title = ts.ExpectedOutcome.ExpectedResult,                            
                        })
                        .ToList(),
                };
                
                function.Scenarios.Add(scenario);
            }
        }

        private void AddFeatureIfNotExist(FunctionAttribute functionAttribute)
        {
            if (!Product.Features.Any(f => f.FeatureID == functionAttribute.Function.Feature.FeatureID))
            {
                lock (Product)
                {
                    var feature = new Feature()
                    {
                        FeatureID = functionAttribute.Function.Feature.FeatureID,
                        Summary = functionAttribute.Function.Feature.Summary,
                        Title = functionAttribute.Function.Feature.Title,
                    };

                    Product.Features.Add(feature);
                }
            }
        }

        private Function AddFunctionIfNotExist(FunctionAttribute functionAttribute)
        {
            var feature = Product.Features
                .Where(f => f.FeatureID == functionAttribute.Function.Feature.FeatureID)
                .Single();

            if (!feature.Functions.Any(f => f.FunctionID == functionAttribute.Function.FunctionID))
            {
                lock (Product)
                {
                    var function = new Function()
                    {
                        FunctionID = functionAttribute.Function.FunctionID,
                        Summary = functionAttribute.Function.Summary,
                        Title = functionAttribute.Function.Title,
                    };

                    feature.Functions.Add(function);
                }
            }

            return feature.Functions
                .Where(f => f.FunctionID == functionAttribute.Function.FunctionID)
                .Single();
        }


        public void Finish()
        {
            // TODO: Need a way for users to pass in sorting algorthm in the constructor. 

            Product.Features.Sort((x, y) => x.FeatureID.CompareTo(y.FeatureID));

            foreach(var feature in Product.Features)
            {
                foreach(var function in feature.Functions)
                {
                    function.Scenarios.Sort((x, y) => x.ScenarioID.CompareTo(y.ScenarioID));
                }

                feature.Functions.Sort((x, y) => x.FunctionID.CompareTo(y.FunctionID));
            }

            // serialize yaml directly to a file
            using (StreamWriter file = File.CreateText(@"c:\temp\CoatHangerSpec.yaml"))
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

        public CoatHangerServiceBuilder(IProduct product)
        {
            Product = new Product()
            {
                ProductID = product.ProductID,
                Title = product.Title,
                Summary = product.Summary
            };
        }

        public CoatHangerService Build()
        {
            CoatHangerService service = new CoatHangerService(Product);

            return service;
        }
    }
}
