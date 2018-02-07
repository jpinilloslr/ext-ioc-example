using System.Collections.Generic;
using System.Linq;
using Domain.Seedwork.CoreAttributes;
using ExtIocExample.Application.ExternalServices;

namespace Infrastructure.Crosscutting.ExternalServices.TypeMapping
{
    [Binding]
    public class AutoMapperTypeMapperService : ITypeMapperService
    {
        public static IDependencyResolver DependencyResolver { get; private set; }

        public AutoMapperTypeMapperService(AutoMapperConfigurator autoMapperConfigurator, 
            IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
            autoMapperConfigurator.Configure();
        }

        public TTarget Map<TTarget>(object source) where TTarget : class, new()
        {
            return Mapper.Map<TTarget>(source);
        }

        public IEnumerable<TTarget> MapAsEnumerable<TTarget>(IEnumerable<object> items)
            where TTarget : class, new()
        {
            return items.Select(Map<TTarget>);
        }
    }
}