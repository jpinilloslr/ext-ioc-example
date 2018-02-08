using System;
using System.Reflection;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Wrappers
{
    public static class Introspector
    {
        public static Type GetTypeByFullName(string fullName)
        {
            return Type.GetType(fullName);
        }

        public static string GetCallingAssemblyDirectory()
        {
            var assemblyFile = GetCallingAssemblyFilename();

            return FileSystem.GetDirectoryName(assemblyFile);
        }

        public static string GetCallingAssemblyFilename()
        {
            var assemblyCodeBase = Assembly.GetCallingAssembly();
            var assemblyFile     = new Uri(assemblyCodeBase.EscapedCodeBase).LocalPath;

            return assemblyFile;
        }

        public static Type[] GetAssemblyTypes(string filename)
        {
            var assembly = Assembly.LoadFrom(filename);

            return assembly.GetTypes();
        }

        public static Assembly GetAssembly(string assemblyName)
        {
            Assembly result;
            try
            {
                result = Assembly.Load(assemblyName);
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }
    }
}