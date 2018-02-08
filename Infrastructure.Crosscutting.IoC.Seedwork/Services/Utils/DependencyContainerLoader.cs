using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Utils;
using Infrastructure.Crosscutting.IoC.Seedwork.Wrappers;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Utils
{
    public class DependencyContainerLoader : IDependencyContainerLoader
    {
        public IDependencyContainer LoadAvailable()
        {
            IDependencyContainer dependencyContainer = null;
            var filesEnum = GetIoCAvailablesAssemblies();
            var callingAssemblyFilename = Introspector.GetCallingAssemblyFilename();

            while (filesEnum.MoveNext() && dependencyContainer == null)
            {
                if (!filesEnum.Current.Contains(callingAssemblyFilename))
                {
                    dependencyContainer = GetDependencyContainerImplementationType(filesEnum.Current);                    
                }
            }

            return dependencyContainer;
        }

        private static IEnumerator<string> GetIoCAvailablesAssemblies()
        {
            var directoryName = Introspector.GetCallingAssemblyDirectory();
            var searchPattern = "*." + GetIoCAssemblyFileNamePrefix() + "*.dll";
            var files         = FileSystem.GetDirectoryFiles(directoryName, searchPattern).ToList();
            var filesEnum     = files.GetEnumerator();

            return filesEnum;
        } 

        private static string GetIoCAssemblyFileNamePrefix()
        {
            var currentAssemblyFileName = FileSystem.GetFileName(Introspector.GetCallingAssemblyFilename()) ?? "";

            if(!IsValidAssemblyFileName(currentAssemblyFileName))
            {
                throw new Exception("Invalid assembly filename. Assemblies filenames must have the form Application.Layer.SubLayer.");
            }

            var prefix = currentAssemblyFileName.Substring(0, currentAssemblyFileName.LastIndexOf(".", StringComparison.Ordinal));
            prefix = prefix.Substring(0, prefix.LastIndexOf(".", StringComparison.Ordinal));

            return prefix;
        }

        private static bool IsValidAssemblyFileName(string currentAssemblyFileName)
        {
            return (currentAssemblyFileName.Count(c => c == '.') >= 2);
        }

        private static IDependencyContainer GetDependencyContainerImplementationType(string fileName)
        {
            IDependencyContainer dependencyContainerImpl = null;
            try
            {
                var containerType = Introspector.GetAssemblyTypes(fileName).
                    FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IDependencyContainer)));

                if (null != containerType)
                    dependencyContainerImpl = (IDependencyContainer)Activator.CreateInstance(containerType);
            }
            catch
            {
                dependencyContainerImpl = null;
            }

            return dependencyContainerImpl;
        }
    }
}