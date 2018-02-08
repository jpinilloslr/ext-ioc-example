using System;
using System.Web.Http.Dependencies;
using Infrastructure.Crosscutting.IoC.Seedwork;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Bindings;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interceptors;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Utils;
using Infrastructure.Crosscutting.IoC.Seedwork.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using WebActivator;

[assembly: PreApplicationStartMethod(typeof(Bootstrapper), "Initialize")]
[assembly: ApplicationShutdownMethod(typeof(Bootstrapper), "Shutdown")]
namespace Infrastructure.Crosscutting.IoC.Seedwork
{
    public class Bootstrapper
    {
        public static IDependencyContainer DependencyContainer { get; private set; }

        public static void Initialize()
        {
            var loader = new DependencyContainerLoader();
            DependencyContainer = loader.LoadAvailable();

            if(DependencyContainer == null)
            {
                throw new Exception("Can not load dependency container assembly. Check if an assembly with name " +
                                    "*.Infrastructure.IoC.*.dll is available in the output directory. Check if " +
                                    "all dependencies of that assembly are available in the output directory.");
            }

            DependencyContainer.Initialize();
            RegisterBindings();
            RegisterInterceptors();
            DynamicModuleUtility.RegisterModule(typeof(DependencyInjectorModule));
        }

        public static void Shutdown()
        {
            DependencyContainer?.Shutdown();
        }

        public static IDependencyResolver CreateDependencyResolver()
        {
            return DependencyContainer;
        }

        private static void RegisterBindings()
        {
            var importer = new BindingsImporter(DependencyContainer);
            var bindingsProvider = AttributeBindingsProvider.Create();
            importer.Import(bindingsProvider);
        }

        private static void RegisterInterceptors()
        {
            var importer = new InterceptorsImporter(DependencyContainer);
            var bindingsProvider = AttributeInterceptorsProvider.Create();
            importer.Import(bindingsProvider);
        }
    }
}