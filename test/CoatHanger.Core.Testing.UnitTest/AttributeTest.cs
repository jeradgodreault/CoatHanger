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
            Type classDirectlyInheirtFromSystem = typeof(ClassImplFeatureInterface);
            
            // act
            var result = new AreaAttribute(classDirectlyInheirtFromSystem);

            // assert
            Assert.IsNotNull(result.Area);
            Assert.AreEqual("System -> Feature",  result.Area.Summary);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenGiveSystemSpecType_ExpectFailure()
        {
            // arrange
            Type systemSpecification = typeof(ClassImplProductInterface);

            // act
            new AreaAttribute(systemSpecification);

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
            new AreaAttribute(nullType);

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
            new AreaAttribute(classNotInheritFromSystemSpecification);

            // assert
            Assert.Fail("The system did not throw an exceptions");
        }

        internal class ClassNotInheritFromSystemSpecification
        {
            public string GetDisplayName() => "Does not inheirt from SystemSpec";
            public string GetSuitePath() => "/" + GetDisplayName();
        }

        internal class ClassImplProductInterface : IProduct
        {
            public string ID => throw new NotImplementedException();

            public string Title => "System";

            public string Summary => "/";
        }


        internal class ClassImplFeatureInterface : FunctionArea
        {
            public override string ID => throw new NotImplementedException();
            public override string Title => "Feature";
            public override string Summary => "System -> Feature";

            public override FeatureArea Parent => throw new NotImplementedException();
        }

        internal class ClassImplFunctionInterface : FunctionArea
        {
            public override string ID => throw new NotImplementedException();

            public override string Title => "Function";

            public override string Summary => "System -> Feature -> Function";

            public override FeatureArea Parent => throw new NotImplementedException();
        }




    }
}
