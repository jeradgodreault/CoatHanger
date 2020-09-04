using CoatHanger.Testing.Web.UnitTest;

namespace CoatHanger.TestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            TestRunner.ExecuteTests(typeof(FormcastServiceMsTest));
        }
    }
}
