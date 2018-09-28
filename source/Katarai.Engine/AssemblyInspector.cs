using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Engine
{
    public class AssemblyInspector
    {
        private readonly string _assemblyPath;
        private Assembly _loadedAssembly;

        public AssemblyInspector(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
            if (string.IsNullOrEmpty(assemblyPath)) throw new ArgumentNullException("assemblyPath");
            LoadAssembly();
        }

        private void LoadAssembly()
        {
            if (!File.Exists(_assemblyPath)) throw new FileNotFoundException("Assembly file not found", _assemblyPath);
            try
            {
                _loadedAssembly = Assembly.LoadFrom(_assemblyPath);
            }
            catch (Exception ex)
            {
                throw new TestpackException(_assemblyPath, "Unable to read from Testpack file: " + ex.Message);
            }
        }

        
        public IEnumerable<Type> GetTypesImplementing(Type interfaceType)
        {
            if (interfaceType == null) throw new ArgumentNullException("interfaceType");
            return _loadedAssembly.ExportedTypes.Where(interfaceType.IsAssignableFrom);
        }
    }
}