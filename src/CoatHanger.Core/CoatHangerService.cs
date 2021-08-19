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
        
        internal IAuthorFormatter AuthorFormatter { get; set; } = new DefaultAuthorFormatter();
        internal IReleaseVersionFormatter ReleaseVersionFormatter { get; set; } = new DefaultReleaseVersionFormatter();
        internal string EvidencePath { get; set; } = ".";
        internal string AttachmentPath { get; set; } = ".";
        internal string FileNamePrefix {get; set; } = "";

        internal string TargetDirectory { get; set; }       

        // final output 
        private CoatHangerSpecDTO CoatHangerSpec { get; set; }
        private HashSet<BusinessRuleDTO> BusinessRules { get; set; }
        private CoatHangerResultDTO CoatHangerResult { get; set; }

        public CoatHangerService()
        {
            if (TargetDirectory == null)
            {
                TargetDirectory = Directory.GetCurrentDirectory();
            }
        }

        internal void Init(Product product)
        {
            try
            {
                var deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();

                Product = deserializer.Deserialize<Product>(File.ReadAllText(@$"{TargetDirectory}/{FileNamePrefix}CoatHangerProduct.yaml"));
                CoatHangerSpec = deserializer.Deserialize<CoatHangerSpecDTO>(File.ReadAllText(@$"{TargetDirectory}/{FileNamePrefix}CoatHangerSpec.yaml"));
                CoatHangerResult = deserializer.Deserialize<CoatHangerResultDTO>(File.ReadAllText(@$"{TargetDirectory}/{FileNamePrefix}CoatHangerResult.yaml"));
                var businessRulesDto = deserializer.Deserialize<CoatHangerBusinessRuleDTO>(File.ReadAllText(@$"{TargetDirectory}/{FileNamePrefix}CoatHangerBusinessRule.yaml"));

                BusinessRules = new HashSet<BusinessRuleDTO>(businessRulesDto.BusinessRules);
            }
            catch (Exception ex)
            {
                //TODO: Clean up this implementation. 
                Product = product;

                CoatHangerSpec = new CoatHangerSpecDTO()
                {
                    ProductID = product.ProductID
                };

                CoatHangerResult = new CoatHangerResultDTO()
                {
                    ProductID = product.ProductID
                };

                BusinessRules = new HashSet<BusinessRuleDTO>();
            }
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

                AddHierarchy(functionAttribute.Area);

                var function = Product.Features.SelectMany(feature => feature.Functions)
                    .Where(f=> f.FunctionID == functionAttribute.Area.ID)
                    .FirstOrDefault();

                // must be a sub-function
                if (function == null)
                {
                    function = Product.Features
                        .SelectMany(feature => feature.Functions)
                        .SelectMany(function => function.Functions)
                        .Where(f => f.FunctionID == functionAttribute.Area.ID)
                        .SingleOrDefault();
                }

                //Method Only attributes 
                var testCaseAttribute = (TestCaseAttribute)Attribute.GetCustomAttribute(unitTestMethod, typeof(TestCaseAttribute));
                var testDesignerAttribute = (TestDesignerAttribute)Attribute.GetCustomAttribute(unitTestMethod, typeof(TestDesignerAttribute));
                var requirementAttribute = (RequirementAttribute)Attribute.GetCustomAttribute(unitTestMethod, typeof(RequirementAttribute));
                var releaseAttribute = (ReleaseAttribute)Attribute.GetCustomAttribute(unitTestMethod, typeof(ReleaseAttribute));
                var regressionReleaseAttribute = (RegressionReleaseAttribute)Attribute.GetCustomAttribute(unitTestMethod, typeof(RegressionReleaseAttribute));

                if (testCaseAttribute != null)
                {
                    int currentInterationID = 1;
                    var scenario = CoatHangerSpec.Scenarios
                        .SingleOrDefault(f => f.ScenarioID == testCaseAttribute.Identifier);

                    if (scenario?.Iterations != null && scenario?.Iterations.Count > 0)
                    {
                        currentInterationID = scenario.Iterations.Max(i => i.InterationID + 1);
                    }                    

                    TestCase testCase = new TestCase()
                    {
                        TestCaseID = $"{testCaseAttribute.Identifier}",
                        ScenarioID = (scenario != null) ? scenario.ScenarioID : testCaseAttribute.Identifier,
                        FunctionID = function.FunctionID,
                        Title = testCaseAttribute.Title,
                        Description = testCaseAttribute.Description,
                        TestSteps = testProcedure.Steps,
                        TestingCategory = testCaseAttribute.Category,
                        TestingStyle = testCaseAttribute.Style,
                        PrerequisiteSteps = (testProcedure.PrerequisiteSteps.Count > 0)?  testProcedure.PrerequisiteSteps : null
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


                    var testResult = new TestResult()
                    {
                        TestCaseID = testCase.TestCaseID,
                        InterationID = currentInterationID,
                        TestExecution = new TestExecution()
                        {
                            IsSuccessful = testResultOutcome == TestResultOutcome.Passed,
                            ExecuteStartDate = testProcedure.TestExecutionStartDateTime.ToString("s"),
                            // Adding 1 second delay so that End is always ahead of Start. 
                            // Some unit test run faster than reasonable date format will allow.
                            ExecuteEndDate = DateTime.Now.AddSeconds(1).ToString("s")
                        }
                    };

                    CoatHangerResult.TestResults.Add(testResult);

                    testCase.TestStatus = CoatHangerResult.TestResults
                        .Where(tr => tr.TestCaseID == testCase.TestCaseID && tr.InterationID == currentInterationID)
                        .OrderByDescending(tr=> tr.TestExecution.ExecuteStartDate)
                        .First()
                        .TestExecution.IsSuccessful
                        ? TestStatus.Passed
                        : TestStatus.Failed;


                    if (testProcedure.References.Count > 0)
                    {
                        testCase.References = testProcedure.References.ToList();
                    }                    

                    if (testProcedure is GivenWhenThenProcedure gwt)
                    {
                        var iteration = testProcedure.Iteration;

                        if (scenario == null)
                        {
                            scenario = new GherkinScenario()
                            {
                                ScenarioID = testCase.TestCaseID,
                                FunctionID = function.FunctionID,
                                Givens = gwt.Givens,
                                Whens = gwt.Whens,
                                Thens = gwt.Thens,
                                BusinessRules = gwt.BusinessRules.Select(br => br.ID).Distinct().ToList()
                            };

                            // First interation of scenario is always one. 
                            iteration.InterationID = 1;
                            CoatHangerSpec.Scenarios.Add(scenario);

                            foreach(var br in gwt.BusinessRules)
                            {
                                AddBusinessRule(br);
                            }                            
                        }
                        else
                        {
                            iteration.InterationID = currentInterationID;
                        }

                        if (iteration.TestParameters.Count > 0 || iteration.RequirementParameters.Count > 0)
                        {
                            scenario.Iterations.Add(iteration);
                        } else
                        {
                            scenario.Iterations = null;
                        }                        
                    }

                    testCase.InterationID = currentInterationID;
                    CoatHangerSpec.TestCases.Add(testCase);
                }
            }
        }

        private void AddBusinessRule(BusinessRule br)
        {
            if (br.Parent != null)
            {
                // Recusrive navigate the hierarchy 
                AddBusinessRule(br.Parent);
            }

            BusinessRules.Add(new BusinessRuleDTO()
            {
                BusinessRuleID = br.ID,
                Title = br.Title,
                ParentID = br?.Parent?.ID
            });
        }

        private void AddHierarchy(IAreaPath areaPath)
        {            
            // Features are top-level nodes. 
            if (areaPath.Area == "Feature")
            {
                var currentFeature = Product.Features
                    .Where(f => f.FeatureID == areaPath.ID)
                    .SingleOrDefault();

                if (currentFeature == null)
                {
                    Product.Features.Add(new Feature()
                    {
                        FeatureID = areaPath.ID,
                        Summary = areaPath.Summary,
                        Title = areaPath.Title,
                    });
                }
            }
            else if (areaPath.Area == "Function")
            {
                // Does the feature exisit? If not add it. 
                if (Product.Features.Where(f => f.FeatureID == areaPath.ParentArea.ID).SingleOrDefault() == null)
                {
                    AddHierarchy(areaPath.ParentArea);
                }

                var parentFeature = Product.Features
                .Where(f => f.FeatureID == areaPath.ParentArea.ID)
                .SingleOrDefault();

                // Does the function already exist for this feature? 
                if (!Product.Features
                    .SelectMany(feature=> feature.Functions)
                    .Any(feature => feature.FunctionID == areaPath.ID)) 
                {
                    parentFeature.Functions.Add(new Function()
                    {
                        FunctionID = areaPath.ID,
                        Summary = areaPath.Summary,
                        Title = areaPath.Title,
                    });
                }
            }
            else if (areaPath.Area == "Sub-Function")
            {
                if (Product.Features.SelectMany(f=> f.Functions).Where(f => f.FunctionID == areaPath.ParentArea.ID).SingleOrDefault() == null)
                {
                    AddHierarchy(areaPath.ParentArea);
                }

                var parentFunction = Product.Features
                .SelectMany(f=> f.Functions)
                .Where(f => f.FunctionID == areaPath.ParentArea.ID)
                .SingleOrDefault();

                // Does the function already exist for this feature? 
                if (!Product.Features
                    .SelectMany(feature => feature.Functions)
                    .SelectMany(function=> function.Functions)
                    .Any(f => f.FunctionID == areaPath.ID)
                )
                {
                    parentFunction.Functions.Add(new Function()
                    {
                        FunctionID = areaPath.ID,
                        Summary = areaPath.Summary,
                        Title = areaPath.Title,
                    });
                }
            }
        }

        public void Finish()
        {
            var generatedDateTime = DateTime.Now;
            Product.GeneratedDateTime = generatedDateTime;

            // TODO: Need a way for users to pass in sorting algorthm in the constructor. 
            Product.Features.Sort((x, y) => x.FeatureID.CompareTo(y.FeatureID));

            foreach (var feature in Product.Features)
            {
                feature.Functions.Sort((x, y) => x.FunctionID.CompareTo(y.FunctionID));
            }

            CoatHangerSpec.Scenarios.Sort((x, y) => x.ScenarioID.CompareTo(y.ScenarioID));
            CoatHangerSpec.TestCases.Sort((x, y) => x.TestCaseID.CompareTo(y.TestCaseID));
            CoatHangerResult.TestResults.Sort((x, y) =>
            {
                int result = x.TestCaseID.CompareTo(y.TestCaseID);
                return result != 0 ? result : x.InterationID.CompareTo(y.InterationID);
            });

            var businessRuleList = BusinessRules.ToList();
            businessRuleList.Sort((x, y) => x.BusinessRuleID.CompareTo(y.BusinessRuleID));

            // serialize yaml directly to a file
            using (StreamWriter file = File.CreateText(@$"{TargetDirectory}/{FileNamePrefix}CoatHangerProduct.yaml"))
            {
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance)
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                    .Build();

                serializer.Serialize(file, Product);
            }

            using (StreamWriter file = File.CreateText(@$"{TargetDirectory}/{FileNamePrefix}CoatHangerSpec.yaml"))
            {
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance)
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                    .Build();

                CoatHangerSpec.GeneratedDateTime = generatedDateTime;

                serializer.Serialize(file, CoatHangerSpec);
            }

            using (StreamWriter file = File.CreateText(@$"{TargetDirectory}/{FileNamePrefix}CoatHangerResult.yaml"))
            {
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance)
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                    .Build();

                CoatHangerResult.GeneratedDateTime = generatedDateTime;

                serializer.Serialize(file, CoatHangerResult);
            }


            using (StreamWriter file = File.CreateText(@$"{TargetDirectory}/{FileNamePrefix}CoatHangerBusinessRule.yaml"))
            {
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(PascalCaseNamingConvention.Instance)
                    .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                    .Build();

                serializer.Serialize(file, new CoatHangerBusinessRuleDTO()
                {
                    ProductID = Product.ProductID,
                    GeneratedDateTime = generatedDateTime,
                    BusinessRules = businessRuleList
                });
            }

        }
    }

    public class CoatHangerServiceBuilder
    {
        Product Product { get; set; }
        private string DocumentationResourcePath { get; set; } = ".";
        private IAuthorFormatter AuthorFormatter { get; set; } = new DefaultAuthorFormatter();
        private IReleaseVersionFormatter ReleaseVersionFormatter { get; set; } = new DefaultReleaseVersionFormatter();
        private string FileNamePrefix { get; set; } = "";
        private string TargetDirectoryPath { get; set; }

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

        public CoatHangerServiceBuilder WithFileNamePrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix)) throw new ArgumentException($"The filename prefix ({nameof(prefix)} paramater) cannot be null or empty.");
            FileNamePrefix = prefix + "_";
            return this; 
        }

        /// <summary>
        /// The directory the coathanger files should be saved too.
        /// </summary>
        public CoatHangerServiceBuilder WithTargetDirectory(string targetDirectoryPath)
        {
            if (string.IsNullOrEmpty(targetDirectoryPath)) throw new ArgumentException($"The filename path ({nameof(targetDirectoryPath)} paramater) cannot be null or empty.");
            TargetDirectoryPath = targetDirectoryPath;
            return this;
        } 

        public CoatHangerService Build()
        {
            CoatHangerService service = new CoatHangerService()
            {
                AuthorFormatter = AuthorFormatter,
                ReleaseVersionFormatter = ReleaseVersionFormatter,
                AttachmentPath = $"{DocumentationResourcePath}/Attachments",
                EvidencePath = $"{DocumentationResourcePath}/Evidences",
                FileNamePrefix = FileNamePrefix,
                TargetDirectory = TargetDirectoryPath ?? Directory.GetCurrentDirectory()
            };
            service.Init(Product);

            return service;
        }

    }
}
