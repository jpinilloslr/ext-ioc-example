using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Utils;
using Infrastructure.Crosscutting.IoC.Seedwork.Wrappers;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Utils
{
    public class AssembliesClassExplorer : IAssembliesClassExplorer
    {
        private readonly IAssembliesEnumerator _assembliesEnumerator;
        private readonly List<Type> _cache = new List<Type>();        

        public AssembliesClassExplorer(IAssembliesEnumerator assembliesEnumerator)
        {
            _assembliesEnumerator = assembliesEnumerator;
        }

        public bool CachedData => _cache.Any();

        public void ClearCache()
        {
            _cache.Clear();
        }

        public void Explore(Action<Type> processTypeFunc)
        {
            if(!CachedData)
            {
                ExploreAssemblies(processTypeFunc);
            }
            else
            {
                ExploreCache(processTypeFunc);
            }
        }

        private void ExploreAssemblies(Action<Type> processTypeFunc)
        {
            var assemblies = _assembliesEnumerator.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                InspectAssembly(assembly, processTypeFunc);
            }
        }

        private void ExploreCache(Action<Type> processTypeFunc)
        {
            foreach (var cachedType in _cache)
            {
                processTypeFunc(cachedType);
            }
        }

        private void InspectAssembly(string assembly, Action<Type> processTypeFunc)
        {
            var types = Introspector.GetAssemblyTypes(assembly).Where(type => type.IsClass);
            foreach (var type in types)
            {
                _cache.Add(type);
                processTypeFunc(type);
            }
        }
    }
}