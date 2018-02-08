using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Utils;
using Infrastructure.Crosscutting.IoC.Seedwork.Wrappers;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Utils
{
    public class AssembliesEnumerator : IAssembliesEnumerator
    {
        private readonly List<string> _assemblyFilters = new List<string>{ "ExtIocExample*.dll", "*.Seedwork.dll" };

        public IEnumerable<string> GetAssemblies()
        {
            var assembliesDirectory = Introspector.GetCallingAssemblyDirectory();
            var assemblyFileName = FileSystem.GetFileName(Introspector.GetCallingAssemblyFilename());

            if(!IsValidAssemblyFileName(assemblyFileName))
            {
                throw new Exception("Invalid assembly filename. Assemblies filenames must have the form 'Application.Layer.SubLayer.dll'.");
            }

            return GetAssembliesByFilters(assembliesDirectory);
        }

        private IEnumerable<string> GetAssembliesByFilters(string assembliesDirectory)
        {
            var result = new List<string>();
            _assemblyFilters.ForEach(searchPattern => 
                result.AddRange(FileSystem.GetDirectoryFiles(assembliesDirectory, searchPattern).ToList()));
            return result;
        } 

        private static bool IsValidAssemblyFileName(string currentAssemblyFileName)
        {
            return (currentAssemblyFileName.Count(c => c == '.') >= 2);
        }
    }
}