using System;
using System.Reflection;
using Engine;
using Engine.Annotations;
using NUnit.Framework;

namespace Katarai.Engine.Tests
{
    [TestFixture]
    public class TestTestMethod
    {
        [Test]
        public void Constructor_GivenNullMethodInfo_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() =>  new TestMethod(null));
            //---------------Test Result -----------------------
            Assert.AreEqual("methodInfo", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenMethodWithAnnotations_ShouldPopulateAnnotations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MethodInfo mi = this.GetType().GetMethod("MethodWithAnnotations");
            var testMethod = new TestMethod(mi);
            //---------------Test Result -----------------------
            Assert.AreEqual(4,testMethod.KataAnnotations.Count);
        }

        [Test]
        public void Constructor_GivenMethodWithNoAnnotations_ShouldPopulateAnnotationsAsEmpty()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MethodInfo mi = this.GetType().GetMethod("MethodWithNoAnnotations");
            var testMethod = new TestMethod(mi);
            //---------------Test Result -----------------------
            Assert.AreEqual(0,testMethod.KataAnnotations.Count);
        }

        [Test]
        public void Constructor_GivenMethodWithOtherAnnotations_ShouldPopulateAnnotationsAsEmpty()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MethodInfo mi = this.GetType().GetMethod("MethodWithOtherAnnotations");
            var testMethod = new TestMethod(mi);
            //---------------Test Result -----------------------
            Assert.AreEqual(0,testMethod.KataAnnotations.Count);
        }

        [Test]
        public void Constructor_GivenMethodWithTestCaseAttribute_ShouldPopulateSetHasTestCaseAttributePropertyToTrue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MethodInfo mi = this.GetType().GetMethod("MethodWithTestCaseAnnotation");
            var testMethod = new TestMethod(mi);
            //---------------Test Result -----------------------
            Assert.IsTrue(testMethod.HasTestCaseAttribute);
        }

        [Test]
        public void Constructor_GivenMethodWithExpectedExceptionAttribute_ShouldPopulateSetHasExpectedExceptionAttributePropertyToTrue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            MethodInfo mi = GetType().GetMethod("MethodWithExpectedExceptionAnnotation");
            var testMethod = new TestMethod(mi);
            //---------------Test Result -----------------------
            Assert.IsTrue(testMethod.HasExpectedExceptionAttribute);
        }

        [Ignore("Because nunit 3.x needs this populated")]
        [TestCase]
        public void MethodWithTestCaseAnnotation()
        {
            
        }

        [Ignore("Because nunit 3.x needs this populated")]
        [ExpectedException]
        public void MethodWithExpectedExceptionAnnotation()
        {
            
        }

        [Ignore("Because nunit 3.x needs this populated")]
        [Test]
        [TestStep(9)]
        [StepShouldDo("Return all negative numbers in exception")]
        [EdgeCaseHint("Did you remember to check for more then 1 negative?")]
        [SuggestedTestName("Add_WhenHasManyNegatives_ShouldThrowErrorWithMessage")]
        public void MethodWithAnnotations()
        {
        }

        [Ignore("Because nunit 3.x needs this populated")]
        [Test]
        public void MethodWithOtherAnnotations()
        {
            MethodWithNoAnnotations();
        }

        public void MethodWithNoAnnotations()
        {
        }
    }
}