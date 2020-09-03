using CoatHanger.Core;
using CoatHanger.Testing.Web.UnitTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoatHanger.TestRunner
{
    public class TestRunner
    {
        public static void ExecuteTests(params Type[] types)
        {

            var testAssembly = types.Select(t => t.Assembly).ToArray();
            var tests = FindTests(testAssembly);

            foreach (var test in tests)
            {
                Console.WriteLine(test.FullName);
                var testCases = test
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(t=> t.IsDefined(typeof(TestCaseAttribute)))
                    ;

                foreach(var testCase in testCases)
                {
                    var instance = (TestCaseAttribute)Attribute.GetCustomAttribute(testCase, typeof(TestCaseAttribute));


           


                    Console.WriteLine(instance.DisplayName);
                }
            }
        }

        private static List<Type> FindTests(params Assembly[] assemblies)
        {
            var tests = assemblies
                .SelectMany(a=> a.ExportedTypes)                
                .Where(t => t.IsDefined(typeof(TestSuiteAttribute)))
                .ToList();

            return tests;
        }


    }
}
