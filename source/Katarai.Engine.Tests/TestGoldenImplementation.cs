using Engine;
using Engine.Annotations;
using NUnit.Framework;

namespace Katarai.Engine.Tests
{
    [TestFixture]
    public class TestGoldenImplementation
    {
        [Test]
        public void Constructor_GivenTypeWithTestStepAttribute_ShouldSetStepNumber()
        {
            //---------------Set up test pack-------------------
            var classWithTestStepAnnotation = new MyClassWithTestStepAttribute();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var goldenImplementation = new GoldenImplementation(classWithTestStepAnnotation.GetType());
            //---------------Test Result -----------------------
            Assert.AreEqual(9, goldenImplementation.StepNumber);
        }

        [Test]
        public void Constructor_GivenTypeWithNoTestStepAttribute_ShouldNotSetStepNumber()
        {
            //---------------Set up test pack-------------------
            var classWithNoTestStepAnnotation = new MyClassWithNoAttribute();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var goldenImplementation = new GoldenImplementation(classWithNoTestStepAnnotation.GetType());
            //---------------Test Result -----------------------
            Assert.AreEqual(0, goldenImplementation.StepNumber);
        }

        [Test]
        public void Constructor_GivenTypeWithShouldFailEdgeCaseTestAttribute_ShouldBeIncludedInKataAnnotations()
        {
            //---------------Set up test pack-------------------
            var classWithAttribute = new MyClassWithShouldFailEdgeCastTestAttribute();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var goldenImplementation = new GoldenImplementation(classWithAttribute.GetType());
            //---------------Test Result -----------------------
            var kataAnnotations = goldenImplementation.KataAnnotations;
            Assert.AreEqual(1,kataAnnotations.Count);
            Assert.AreEqual(typeof(ShouldFailEdgeCaseTestAttribute), kataAnnotations[0].GetType());
        }

        [Test]
        public void Constructor_GivenTypeWithNoShouldFailEdgeCaseTestAttribute_ShouldNotBeIncludedInKataAnnotations()
        {
            //---------------Set up test pack-------------------
            var classWithNoAttribute = new MyClassWithNoAttribute();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var goldenImplementation = new GoldenImplementation(classWithNoAttribute.GetType());
            //---------------Test Result -----------------------
            Assert.AreEqual(0, goldenImplementation.KataAnnotations.Count);
        }

        [Test]
        public void Constructor_GivenTypeWithAttributes_ShouldBeIncludedInKataAnnotations()
        {
            //---------------Set up test pack-------------------
            var classWithAttribute = new MyClassWithTestStepAttribute();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var goldenImplementation = new GoldenImplementation(classWithAttribute.GetType());
            //---------------Test Result -----------------------
            var kataAnnotations = goldenImplementation.KataAnnotations;
            Assert.AreEqual(3, kataAnnotations.Count);
        }


        [TestStep(9)]
        [ShouldFailEdgeCaseTest]
        [EdgeCaseHint("Have you tested that 1000 is not filtered out?")]
        private class MyClassWithTestStepAttribute
        {
                 
        }


        private class MyClassWithNoAttribute
        {
                 
        }

        [ShouldFailEdgeCaseTest]
        private class MyClassWithShouldFailEdgeCastTestAttribute
        {
                 
        }

    }
}