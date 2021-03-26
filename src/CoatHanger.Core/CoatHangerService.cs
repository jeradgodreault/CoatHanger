using CoatHanger.Core.Configuration;
using CoatHanger.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private Assembly Assembly { get; set; }
        
        public IAuthorFormatter AuthorFormatter { get; set; } = new DefaultAuthorFormatter();
        public IReleaseVersionFormatter ReleaseVersionFormatter { get; set; } = new DefaultReleaseVersionFormatter();
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

        public void AddTestCase(Assembly assembly, TestContext testContext, TestProcedure testProcedure)
        {
            Assembly = assembly;
            // TODO: Incomplete. Need to map every attribute. 

            var classType = assembly.GetType(testContext.FullyQualifiedTestClassName);
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
                    Title = testCaseAttribute.DisplayName,
                    Description = testCaseAttribute.Title,
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

            foreach(var feature in Product.Features)
            {
                foreach(var function in feature.Functions)
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

        public CoatHangerServiceBuilder(ProductArea product)
        {
            Product = new Product()
            {
                ProductID = product.ID,
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
