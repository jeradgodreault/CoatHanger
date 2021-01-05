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
            var result = new FunctionAttribute(classDirectlyInheirtFromSystem);

            // assert
            Assert.IsNotNull(result.Function);
            Assert.AreEqual("System -> Feature",  result.Function.Summary);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenGiveSystemSpecType_ExpectFailure()
        {
            // arrange
            Type systemSpecification = typeof(ClassImplProductInterface);

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

        internal class ClassImplProductInterface : IProduct
        {
            public string ProductID => throw new NotImplementedException();

            public string Title => "System";

            public string Summary => "/";
        }


        internal class ClassImplFeatureInterface : IFeatureFunction
        {
            public string FunctionID => throw new NotImplementedException();
            public string Title => "Feature";
            public string Summary => "System -> Feature";

            public IProductFeature Feature => throw new NotImplementedException();
        }

        internal class ClassImplFunctionInterface : IFeatureFunction
        {
            public string FunctionID => throw new NotImplementedException();

            public string Title => "Function";

            public string Summary => "System -> Feature -> Function";

            public IProductFeature Feature => throw new NotImplementedException();
        }




    }
}
