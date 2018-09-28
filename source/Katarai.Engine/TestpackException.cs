using System;

namespace Engine
{
    public class TestpackException : Exception
    {
        public string TestpackPath { get; protected set; }
        public string Detail { get; protected set; }

        public TestpackException(string path, string detail)
            : base("Error with Testpack at '" + path + "': " + detail)
        {
            this.TestpackPath = path;
            this.Detail = detail;
        }
    }
}