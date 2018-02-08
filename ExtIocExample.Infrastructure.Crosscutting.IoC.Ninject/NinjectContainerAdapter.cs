using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dependencies;
using Domain.Seedwork.CoreAttributes;
using Infrastructure.Crosscutting.IoC.Seedwork.Data.Bindings;
using Infrastructure.Crosscutting.IoC.Seedwork.Data.Interceptors;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces;
using Ninject;
using Ninject.Extensions.Interception.Infrastructure.Language;
using Ninject.Parameters;
using Ninject.Syntax;
using Ninject.Web.Common;

namespace ExtIocExample.Infrastructure.Crosscutting.IoC.Ninject
{
    public class NinjectContainerAdapter : IDependencyContainer
    {
        private readonly Bootstrapper _bootstrapper = new Bootstrapper();        
        private IKernel _kernel;

        public void Initialize()
        {
            _bootstrapper.Initialize(CreateKernel);
        }

        public void Shutdown()
        {
            _bootstrapper.ShutDown();
        }

        public void Bind(Binding binding)
        {
            if(binding is TypeBinding)
            {
                Bind((TypeBinding)binding);
            }
            else if(binding is FactoryBinding)
            {
                Bind((FactoryBinding)binding);
            }
            else if(binding is MethodBinding)
            {
                Bind((MethodBinding)binding);
            }
        }

        public void AddInterceptor(Interceptor interceptor)
        {
            var type = interceptor.InterceptorMethod.ReflectedType;
            
            if (null == type)
            {
                throw new Exception(
                    $"Can not access declaring type for method {interceptor.InterceptorMethod.Name} in interceptor.");
            }
            
            _kernel.AddMethodInterceptor(interceptor.TargetMethod, invocation =>
            {
                using(var scope = BeginScope())
                {
                    var interceptorInstance = Activator.CreateInstance(type);
                    var interceptorInvocationAdapter = new InterceptorInvocationAdapter();
                    InjectInterceptorClassProperties(interceptorInstance, interceptor, scope);

                    var arguments = interceptorInvocationAdapter.
                        GetInterceptorParametersValues(invocation, interceptor.InterceptorMethod);
                    try
                    {
                        interceptor.InterceptorMethod.Invoke(interceptorInstance, arguments);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }

                    if (!interceptorInvocationAdapter.InterceptedFunctionDelegateHandled)
                    {
                        invocation.Proceed();
                    }    
                }                
            });
        }        

        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            return GetServices(serviceType).FirstOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {           
            var request = _kernel.CreateRequest(serviceType, null, new IParameter[0], true, true);

            return _kernel.Resolve(request);
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(_kernel.BeginBlock());
        }

        private IKernel CreateKernel()
        {
            _kernel = new StandardKernel();

            return _kernel;
        }

        public void Unbind(Type type)
        {
            _kernel.Unbind(type);
        }

        private void Bind(TypeBinding binding)
        {
            var ninjectBinding = _kernel.Bind(binding.FromType).To(binding.ToType);
            SetScope(ninjectBinding, binding.BindingScope);
        }

        private void Bind(MethodBinding binding)
        {
            var ninjectBinding = _kernel.Bind(binding.FromType).ToMethod(context => binding.ToMethod());
            SetScope(ninjectBinding, binding.BindingScope);
        }

        private void Bind(FactoryBinding binding)
        {
            var type = binding.FactoryMethod.DeclaringType;

            if(null == type)
            {
                throw new Exception(
                    $"Can not access declaring type for method {binding.FactoryMethod.Name} in factory binding");
            }

            var instance = Activator.CreateInstance(type);

            var ninjectBinding = _kernel.Bind(binding.FromType).ToMethod(context => binding.FactoryMethod.Invoke(instance, new object[] {}));
            SetScope(ninjectBinding, binding.BindingScope);
        }

        private static void SetScope(IBindingInSyntax<object> ninjectBinding, BindingScope? bindingScope)
        {
            if(null == bindingScope)
            {
                return;
            }

            switch (bindingScope)
            {
                case BindingScope.Request:
                    ninjectBinding.InRequestScope();
                    break;
                case BindingScope.Singleton:
                    ninjectBinding.InSingletonScope();
                    break;
                case BindingScope.Transient:
                    ninjectBinding.InTransientScope();
                    break;
                case BindingScope.Thread:
                    ninjectBinding.InThreadScope();
                    break;
            }
        }

        private void InjectInterceptorClassProperties(object instance, Interceptor interceptor, IDependencyScope dependencyScope)
        {
            var type = instance.GetType();
            var properties = type.GetProperties();

            foreach (var propertyInfo in properties.Where(pi => pi.CanWrite))
            {
                GetProperty(instance, interceptor, dependencyScope, propertyInfo);
            }
        }

        private static void GetProperty(object instance, Interceptor interceptor, 
            IDependencyScope dependencyScope, PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;

            if (interceptor.InterceptorProperties.ContainsKey(name))
            {
                InjectInterceptorUserDefinedProperty(instance, interceptor, propertyInfo, name);
            }
            else
            {
                InjectIfIsDependencyProperty(instance, dependencyScope, propertyInfo);
            }
        }

        private static void InjectIfIsDependencyProperty(object instance,
            IDependencyScope dependencyScope, PropertyInfo propertyInfo)
        {
            var dependencyAttributes = propertyInfo
                .GetCustomAttributes(typeof (DependencyAttribute));

            if (dependencyAttributes.Any())
            {
                InjectInterceptorDependencyProperty(instance, dependencyScope, propertyInfo);    
            }            
        }

        private static void InjectInterceptorDependencyProperty(object instance, 
            IDependencyScope dependencyScope, PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;
            var propertyValue = dependencyScope.GetService(propertyType);
            propertyInfo.SetValue(instance, propertyValue);
        }

        private static void InjectInterceptorUserDefinedProperty(object instance, Interceptor interceptor,
            PropertyInfo propertyInfo, string name)
        {
            var propertyValue = interceptor.InterceptorProperties[name];
            propertyInfo.SetValue(instance, propertyValue);
        }
    }
}