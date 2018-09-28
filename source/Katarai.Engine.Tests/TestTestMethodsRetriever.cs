using Engine.Runners;
using NUnit.Framework;

namespace Katarai.Engine.Tests
{
    [TestFixture]
    public class TestTestMethodsRetriever
    {
        [Test]
        public void GetTestMethods_GivenMethodWithIgnoreAttribute_ShouldReturnEmpty()
        {
            //---------------Set up test pack-------------------
            var testMethodsRetriever = new TestMethodsRetriever();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var testMethods = testMethodsRetriever.GetTestMethods(typeof(SampleClassWithIgnoreAttribute));
            //---------------Test Result -----------------------
            CollectionAssert.IsEmpty(testMethods);
        }

        [Test]
        public void GetTestMethods_GivenMethodWithTestAttribute_ShouldReturnMethod()
        {
            //---------------Set up test pack-------------------
            var testMethodsRetriever = new TestMethodsRetriever();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var testMethods = testMethodsRetriever.GetTestMethods(typeof(SampleClassWithTestAttribute));
            //---------------Test Result -----------------------
            Assert.AreEqual(1, testMethods.Length);
        }

        [Test]
        public void GetTestMethods_GivenMethodWithTestCaseAttribute_ShouldReturnMethod()
        {
            //---------------Set up test pack-------------------
            var testMethodsRetriever = new TestMethodsRetriever();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var testMethods = testMethodsRetriever.GetTestMethods(typeof(SampleClassWithTestCaseAttribute));
            //---------------Test Result -----------------------
            Assert.AreEqual(1, testMethods.Length);
        }

    }

    public class SampleClassWithTestCaseAttribute
    {
        [TestCase]
        public void Method_ShouldNotBeIgnored()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual(0, 0);
        }
    }

    public class SampleClassWithIgnoreAttribute 
    {
        [Ignore("Because nunit 3.x needs this populated")]
        [Test]
        public void Method_ShouldBeIgnored()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual(0, 0);
        }

    }
    public class SampleClassWithTestAttribute 
    {

        [Test]
        public void Method_ShouldNotBeIgnored()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual(0, 0);
        }
    }
}