using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Interceptors;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Interceptors
{
    public class InterceptorsImporter : IInterceptorsImporter
    {
        private readonly IDependencyContainer _dependencyContainer;

        public InterceptorsImporter(IDependencyContainer dependencyContainer)
        {
            _dependencyContainer = dependencyContainer;
        }

        public void Import(IInterceptorsProvider interceptorsProvider)
        {
            var interceptors = interceptorsProvider.GetInterceptors();

            foreach (var interceptor in interceptors)
            {
                _dependencyContainer.AddInterceptor(interceptor);
            }
        }
    }
}