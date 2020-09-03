using CoatHanger.Core;
using CoatHanger.Testing.Web.UnitTest;
using System;
using System.Linq;
using System.Reflection;

namespace CoatHanger.TestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            TestRunner.ExecuteTests(typeof(FormcastServiceTest));
        }
    }
}
