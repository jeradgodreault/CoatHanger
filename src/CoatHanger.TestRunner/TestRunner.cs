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
                //Console.WriteLine(test.FullName);


               



                var testCases = test
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(t=> t.IsDefined(typeof(TestCaseAttribute)))
                    ;

                var testSuiteAttribute = (TestSuiteAttribute)Attribute.GetCustomAttribute(test, typeof(TestSuiteAttribute));

                //var results = GetInheritedClasses(testSuiteAttribute.SuiteType);

                    



                //foreach (var t in results)
                //{
                //    Console.WriteLine(t.FullName);
                //}


                //foreach (var testCase in testCases)
                //{
                //    var testCaseAttribute = (TestCaseAttribute)Attribute.GetCustomAttribute(testCase, typeof(TestCaseAttribute));





                //}
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

        

        private static List<Type> GetInheritedClasses(Type MyType)
        {
            var finalResult = new List<Type>();
            //if you want the abstract classes drop the !TheType.IsAbstract but it is probably to instance so its a good idea to keep it.
            var results = Assembly.GetAssembly(MyType)
                .GetTypes()
                .Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(MyType))
                .ToList();

            if (results.Count != 0)
            {
                foreach(var type in results)
                {
                    finalResult.AddRange(GetInheritedClasses(type));
                }
            }
            else
            {
                finalResult.AddRange(results);
            }
            

            return finalResult;
        }


    }
}
