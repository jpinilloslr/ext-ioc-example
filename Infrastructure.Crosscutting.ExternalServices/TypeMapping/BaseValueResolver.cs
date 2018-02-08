using System.Linq;
using System.Reflection;
using System.Web.Http.Dependencies;
using AutoMapper;
using Domain.Seedwork.CoreAttributes;
using MoreLinq;

namespace ExtIocExample.Infrastructure.Crosscutting.ExternalServices.TypeMapping
{
    public abstract class BaseValueResolver : IValueResolver
    {
        public ResolutionResult Resolve(ResolutionResult source)
        {
            ResolutionResult result;
            using (var dependencyScope = AutoMapperTypeMapperService.DependencyResolver.BeginScope())
            {
                InjectResolverDependencies(dependencyScope);
                result = ResolveValue(source);
            }
            return result;
        }

        protected abstract ResolutionResult ResolveValue(ResolutionResult source);

        private void InjectResolverDependencies(IDependencyScope dependencyScope)
        {
            var type = this.GetType();
            var properties = type.GetProperties();
            properties.Where(pi => pi.CanWrite)
                .ForEach(propertyInfo => InjectIfIsDependencyProperty(propertyInfo, dependencyScope));
        }

        private void InjectIfIsDependencyProperty(PropertyInfo propertyInfo,
            IDependencyScope dependencyScope)
        {
            var dependencyAttributes = propertyInfo
                .GetCustomAttributes(typeof(DependencyAttribute));

            if (dependencyAttributes.Any())
            {
                InjectInterceptorDependencyProperty(propertyInfo, dependencyScope);
            }
        }

        private void InjectInterceptorDependencyProperty(PropertyInfo propertyInfo,
            IDependencyScope dependencyScope)
        {
            var propertyType = propertyInfo.PropertyType;
            var propertyValue = dependencyScope.GetService(propertyType);
            propertyInfo.SetValue(this, propertyValue);
        }
    }
}
