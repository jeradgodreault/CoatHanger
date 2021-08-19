using CoatHanger.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoatHanger.Testing.Web.UnitTest
{

    [TestClass]
    public class CoatHangerManager
    {
        public static CoatHangerService CoatHangerService { get; set; } 

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            CoatHangerService = new CoatHangerServiceBuilder(new CHWeatherProduct()).Build();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            CoatHangerService.Finish();
        }
    }

}
