using CoatHanger.Core.Models;
using System.Collections.Generic;

namespace CoatHanger.Core.Configuration
{

    public interface ITestCaseSorter
    {
        List<TestCase> Sort(List<TestCase> testCases);
    }

    public class DefaultTestCaseSorter : ITestCaseSorter
    {
        public List<TestCase> Sort(List<TestCase> testCases)
        {
            testCases.Sort((x, y) => x.TestCaseID.CompareTo(y.TestCaseID));
            return testCases;
        }
    }

    public interface IFunctionSorter
    {
        List<Function> Sort(List<Function> functions);
    }

    public class DefaultFunctionSorter : IFunctionSorter
    {
        public List<Function> Sort(List<Function> functions)
        {
            functions.Sort((x, y) => x.FunctionID.CompareTo(y.FunctionID));
            return functions;
        }
    }

    public interface IFeatureSorter
    {
        List<Feature> Sort(List<Feature> features);
    }

    public class DefaultFeatureSorter : IFeatureSorter
    {
        public List<Feature> Sort(List<Feature> features)
        {
            features.Sort((x, y) => x.FeatureID.CompareTo(y.FeatureID));
            return features;
        }
    }

}
