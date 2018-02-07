using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Seedwork.CoreAttributes;
using Infrastructure.Crosscutting.ExternalServices.TypeMapping.Configuration;

namespace Infrastructure.Crosscutting.ExternalServices.TypeMapping
{
    [Binding(IsSelfBindable = true, Scope = BindingScope.Singleton)]
    public class AutoMapperConfigurator
    {
        private static bool _loaded;
        private static readonly object Lock = new object();

        public void Configure()
        {
            lock (Lock)
            {
                LoadConfiguration();
            }
        }

        private static void LoadConfiguration()
        {
            if (!_loaded)
            {
                var configurators = GetMapConfigs();

                foreach (var configurationInstance in configurators)
                {
                    configurationInstance.Configure();
                }
                _loaded = true;
            }
        }

        private static IEnumerable<ITypeMapConfigurator> GetMapConfigs()
        {
            return Assembly.GetCallingAssembly().GetTypes()
                .Where(IsValidMapConfigType)
                .Select(CreateInstance)
                .Aggregate(new List<ITypeMapConfigurator>(), IncludeMapper);
        }

        private static List<ITypeMapConfigurator> IncludeMapper(List<ITypeMapConfigurator> list, ITypeMapConfigurator mapper)
        {
            list.Add(mapper);
            return list;
        }

        private static bool IsValidMapConfigType(Type type)
        {
            return type.Namespace != null && 
                type.Namespace.Contains("TypeMapping.Configuration") 
                && type.GetInterfaces().Contains(typeof(ITypeMapConfigurator));
        }

        private static ITypeMapConfigurator CreateInstance(Type configType)
        {
            return (ITypeMapConfigurator)Activator.CreateInstance(configType);
        }
    }
}