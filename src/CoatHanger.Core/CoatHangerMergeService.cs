using CoatHanger.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoatHanger.Core
{
    public class CoatHangerMergeService
    {
        public CoatHangerSpecDTO CoatHangerSpec { get; private set; }
        public CoatHangerBusinessRuleDTO BusinessRules { get; private set; }
        public CoatHangerResultDTO CoatHangerResult { get; private set; }
        public Product Product { get; private set; }


        public Product Merge(Product product)
        {
            var isDirty = false;
            if (Product == null)
            {
                Product = product;
                return product;
            }
            else
            {
                foreach (var feature in product.Features)
                {
                    if (!Product.Features.Any(f => f.FeatureID == feature.FeatureID))
                    {
                        product.Features
                            .Add(feature);
                        isDirty = true;
                        continue;
                    }

                    foreach (var function in feature.Functions)
                    {
                        if (!Product.Features.SelectMany(f => f.Functions).Any(f => f.FunctionID == function.FunctionID))
                        {
                            product.Features
                                .Where(f => f.FeatureID == feature.FeatureID)
                                .Single().Functions
                                .Add(function);

                            isDirty = true;
                            continue;
                        }

                        foreach (var subFunction in function.Functions)
                        {
                            if (!Product.Features.SelectMany(f => f.Functions).SelectMany(f => f.Functions).Any(f => f.FunctionID == subFunction.FunctionID))
                            {
                                product.Features
                                    .Where(f => f.FeatureID == feature.FeatureID)
                                    .SelectMany(f => f.Functions)
                                    .SelectMany(f => f.Functions)
                                    .Where(f => f.FunctionID == subFunction.FunctionID)
                                    .Single().Functions
                                    .Add(function);

                                isDirty = true;
                                continue;
                            }
                        }
                    }
                }
            }


            if (isDirty && product.GeneratedDateTime >= Product.GeneratedDateTime)
            {
                Product.GeneratedDateTime = product.GeneratedDateTime;
            }

            return Product;
        }

        public CoatHangerSpecDTO Merge(CoatHangerSpecDTO spec)
        {
            var isDirty = false;
            if (CoatHangerSpec == null)
            {
                CoatHangerSpec = spec;
            }
            else
            {
                foreach(var scenario in spec.Scenarios)
                {
                    var existingScenario = CoatHangerSpec.Scenarios
                        .Where(s => s.ScenarioID == scenario.ScenarioID)
                        .SingleOrDefault();

                    if (existingScenario == null)
                    {
                        CoatHangerSpec.Scenarios.Add(scenario);
                        isDirty = true;
                    }
                    else if (spec.GeneratedDateTime >= CoatHangerSpec.GeneratedDateTime)
                    {
                        CoatHangerSpec.Scenarios.Remove(existingScenario);
                        CoatHangerSpec.Scenarios.Add(scenario);
                        isDirty = true;
                    }
                }

                foreach(var testCase in spec.TestCases)
                {
                    var existingTestCase = CoatHangerSpec.TestCases
                        .SingleOrDefault(tc => tc.TestCaseID == testCase.TestCaseID 
                            && tc.InterationID == testCase.InterationID
                        );

                    if (existingTestCase == null)
                    {
                        CoatHangerSpec.TestCases.Add(testCase);
                        isDirty = true;
                    }
                    else if (spec.GeneratedDateTime >= CoatHangerSpec.GeneratedDateTime)
                    {
                        CoatHangerSpec.TestCases.Remove(existingTestCase);
                        CoatHangerSpec.TestCases.Add(testCase);
                        isDirty = true;
                    }
                }
            }

            if (isDirty && spec.GeneratedDateTime >= CoatHangerSpec.GeneratedDateTime)
            {
                CoatHangerSpec.GeneratedDateTime = spec.GeneratedDateTime;
            }

            return CoatHangerSpec;
        }

        public CoatHangerResultDTO Merge(CoatHangerResultDTO result)
        {
            var isDirty = false;

            if (result == null)
            {
                CoatHangerResult = result;   
            }
            else
            {
                foreach (var testResult in result.TestResults)
                {
                    if (!CoatHangerResult.TestResults
                        .Any(f => f.TestCaseID == testResult.TestCaseID
                              && f.InterationID == testResult.InterationID
                              && f.TestExecution.ExecuteStartDate == testResult.TestExecution.ExecuteStartDate)
                        )
                    {
                        CoatHangerResult.TestResults.Add(testResult);
                    }
                }
            }

            if (isDirty && result.GeneratedDateTime >= CoatHangerResult.GeneratedDateTime)
            {
                CoatHangerResult.GeneratedDateTime = result.GeneratedDateTime;
            }

            return CoatHangerResult;
        }

        public CoatHangerBusinessRuleDTO Merge(CoatHangerBusinessRuleDTO businessRules)
        {
            if (BusinessRules == null)
            {
                BusinessRules = businessRules;
            }
            else
            {
                if (businessRules.GeneratedDateTime > BusinessRules.GeneratedDateTime)
                {
                    BusinessRules.GeneratedDateTime = businessRules.GeneratedDateTime;
                }

                foreach(var businessRule in businessRules.BusinessRules)
                {                    
                    if (!BusinessRules.BusinessRules.Any(br=> br.BusinessRuleID == businessRule.BusinessRuleID))
                    {
                        BusinessRules.BusinessRules.Add(businessRule);
                    }

                }
            }

            return BusinessRules;
        }



    }
}
