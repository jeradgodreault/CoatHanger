using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoatHanger.Core.Testing.UnitTest
{
    [TestClass]
    public class AttributeTest
    {

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

        internal class ClassImplProductInterface : ProductArea
        {
            public override string ID => "A1";

            public override string Title => "System";

            public override string Summary => "/";
        }


        internal class ClassImplFeatureInterface : FunctionArea<ProductArea>
        {
            public override string ID => "A2";
            public override string Title => "Feature";
            public override string Summary => "System -> Feature";

            public override ProductArea Parent => throw new NotImplementedException();
        }

        internal class ClassImplFunctionInterface : FunctionArea<FeatureArea<ProductArea>>
        {
            public override string ID => throw new NotImplementedException();

            public override string Title => "Function";

            public override string Summary => "System -> Feature -> Function";

            public override FeatureArea<ProductArea> Parent => throw new NotImplementedException();
        }




    }
}
