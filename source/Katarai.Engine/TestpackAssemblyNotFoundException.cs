using System;

namespace Engine
{
    public class TestpackAssemblyNotFoundException : Exception
    {
        public string TestpackPath { get; protected set; }

        public TestpackAssemblyNotFoundException(string atPath)
            : base("Testpack assembly not found")
        {
            this.TestpackPath = atPath;
        }
    }
}