using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoatHanger.Core.Testing.UnitTest
{
    [TestClass]
    public class AttributeTest
    {

        [TestMethod]
        public void WhenGivenAClassDirectlyInheirtFromSystemSpec_ExpectSuccess()
        {
            // arrange 
            Type classDirectlyInheirtFromSystem = typeof(ChildSuite);
            
            // act
            var result = new FunctionAttribute(classDirectlyInheirtFromSystem);

            // assert
            Assert.AreEqual(typeof(ChildSuite),  result.Function);
        }

        [TestMethod]
        public void WhenGivenAClassDesendentOfFromSystemSpec_ExpectSuccess()
        {
            // arrange 
            Type classDirectlyInheirtFromSystem = typeof(GrandChildSuite);
            
            // act
            var result = new FunctionAttribute(classDirectlyInheirtFromSystem);

            // assert
            Assert.AreEqual(typeof(GrandChildSuite), result.Function);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenGiveSystemSpecType_ExpectFailure()
        {
            // arrange
            Type systemSpecification = typeof(SystemSpecification);

            // act
            new FunctionAttribute(systemSpecification);

            // assert
            Assert.Fail("The system did not throw an exceptions");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenGivenNullType_ExpectFailure()
        {
            // arrange
            Type nullType = null;

            // act
            new FunctionAttribute(nullType);

            // assert
            Assert.Fail("The system did not throw an exceptions");
        }



        [TestMethod]        
        [ExpectedException(typeof(ArgumentException))]
        public void WhenGivenAClassNotInheritFromSystemSpec_ExpectError()
        {
            // arrange
            Type classNotInheritFromSystemSpecification = typeof(ClassNotInheritFromSystemSpecification);

            // act
            new FunctionAttribute(classNotInheritFromSystemSpecification);

            // assert
            Assert.Fail("The system did not throw an exceptions");
        }

        internal class ClassNotInheritFromSystemSpecification
        {
            public string GetDisplayName() => "Does not inheirt from SystemSpec";
            public string GetSuitePath() => "/" + GetDisplayName();
        }

        internal class ChildSuite : SystemSpecification
        {
            public override string GetDisplayName() => "Child Suite";
            public override string GetSuitePath() => "System -> Child Suite";
        }

        internal class GrandChildSuite : ChildSuite
        {
            public override string GetDisplayName() => "Grand Child Suite";
            public override string GetSuitePath() => "System -> Child Suite -> Grand Child Suite";
        }




    }
}
