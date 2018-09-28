using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Annotations;
using NUnit.Framework;

namespace Engine
{
    public class TestMethodData
    {
        public int StepNumber { get; set; }
        public string MethodName { get; set; }
        public TestMethodData(int stepNumber, string methodName)
        {
            StepNumber = stepNumber;
            MethodName = methodName;
        }
    }

    public class TestFinder
    {
        public IEnumerable<TestMethodData> TestMethods { get { return _testMethods; } }
        private List<TestMethodData> _testMethods;
        private readonly string _usersTestAssemblyPath;
        private readonly Type _testPackInterfaceType;
        private Assembly _usersTestAssembly;
        private Type _userTestPackType;

        public TestFinder(string pathToAssembly, Type testPackInterfaceType) // TODO: constrain this to be of ITestPack<something>
        {
            if (string.IsNullOrEmpty(pathToAssembly)) throw new ArgumentException("Testpack assembly not specified");
            if (testPackInterfaceType == null) throw new ArgumentException("Testpack interface not specified");
            if (!File.Exists(pathToAssembly)) throw new TestpackAssemblyNotFoundException(pathToAssembly);
            this._usersTestAssemblyPath = pathToAssembly;
            this._testPackInterfaceType = testPackInterfaceType;
            LoadTestAssembly();
            LoadTestMethods();
        }

        private void LoadTestMethods()
        {
            _testMethods = new List<TestMethodData>();
            var methods = _userTestPackType.GetMethods();
            foreach (var method in methods)
            {
                var attrib = method.GetCustomAttribute(typeof(TestStepAttribute), true) as TestStepAttribute;
                if (attrib == null) continue;
                _testMethods.Add(new TestMethodData(attrib.StepNumber, method.Name));
            }
        }

        private void LoadTestAssembly()
        {
            try
            {
                _usersTestAssembly = Assembly.LoadFrom(_usersTestAssemblyPath);
            }
            catch (Exception ex)
            {
                throw new TestpackException(_usersTestAssemblyPath, "Unable to read from Testpack file: " + ex.Message);
            }

            foreach (var t in _usersTestAssembly.ExportedTypes)
            {
                if (_testPackInterfaceType.IsAssignableFrom(t))
                {
                    _userTestPackType = t;
                    break;
                }
            }
            if (_userTestPackType == null)
                throw new TestpackException(_usersTestAssemblyPath, "Required interface implementation not found: " + _testPackInterfaceType.ToString());
        }
    }

}

namespace ScratchPad
{

    //public class TestStringCalculator : ITestPack<IStringCalculator>
    //{
    //    public Func<IStringCalculator> CreateSUT { get; set; }

    //    [SetUp]
    //    public void Setup()
    //    {
    //        CreateSUT = () => new StringCalculator();
    //    }

    //    [Test]
    //    [Ignore]
    //    public void Add_GivenEmptyString_ShouldReturnZero()
    //    {
    //        //---------------Set up test pack-------------------
    //        var calculator = CreateSUT();
    //        //---------------Assert Precondition----------------

    //        //---------------Execute Test ----------------------

    //        //---------------Test Result -----------------------
    //        Assert.Fail("Test Not Yet Implemented");
    //    }
    //}
}
