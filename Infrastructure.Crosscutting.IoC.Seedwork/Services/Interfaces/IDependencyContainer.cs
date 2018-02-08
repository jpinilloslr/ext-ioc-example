using System;
using System.Web.Http.Dependencies;
using Infrastructure.Crosscutting.IoC.Seedwork.Data.Bindings;
using Infrastructure.Crosscutting.IoC.Seedwork.Data.Interceptors;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces
{
    public interface IDependencyContainer : IDependencyResolver
    {
         void Initialize();

        void Shutdown();

        void Unbind(Type type);

        void Bind(Binding binding);

        void AddInterceptor(Interceptor interceptor);
    }
}