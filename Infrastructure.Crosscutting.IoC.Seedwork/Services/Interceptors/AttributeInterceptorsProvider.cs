using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Seedwork.CoreAttributes;
using Infrastructure.Crosscutting.IoC.Seedwork.Data.Interceptors;
using Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Interceptors;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Interceptors;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Utils;
using Infrastructure.Crosscutting.IoC.Seedwork.Services.Utils;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Interceptors
{
    public class AttributeInterceptorsProvider : IInterceptorsProvider
    {
        private readonly IAssembliesClassExplorer _assembliesClassExplorer;
        private const int MaxClassHierarchyDepth = 15;

        public static IInterceptorsProvider Create()
        {
            return new AttributeInterceptorsProvider(
                new AssembliesClassExplorer(new AssembliesEnumerator()));
        }

        public AttributeInterceptorsProvider(IAssembliesClassExplorer assembliesClassExplorer)
        {
            _assembliesClassExplorer = assembliesClassExplorer;
        }

        public IEnumerable<Interceptor> GetInterceptors()
        {
            var result = new List<Interceptor>();
            _assembliesClassExplorer.Explore(type =>
            {
                CheckForInterceptorsInClass(type, result);
                CheckForInterceptorsInMethods(type, result);
            });

            return result;
        }

        private void CheckForInterceptorsInClass(Type type, List<Interceptor> result)
        {
            var attributes = GetClassInterceptorsInOrder(type);
            foreach (InterceptorAttribute attribute in attributes)
            {
                AddInterceptorToClass(type, attribute, result);
            }
        }

        private static object[] GetClassInterceptorsInOrder(Type type)
        {
            var attributes = new List<object>();
            GetClassInterceptorsOverClassHierarchy(type, attributes, MaxClassHierarchyDepth);
            return attributes.ToArray();
        }

        private static void GetClassInterceptorsOverClassHierarchy(Type type, List<object> interceptors, int depth)
        {
            var baseType = type.BaseType;
            if (baseType != null && depth > 0)
            {
                GetClassInterceptorsOverClassHierarchy(baseType, interceptors, depth - 1);
            }
            AddInterceptorsFromType(type, interceptors);
        }

        private static void AddInterceptorsFromType(Type type, List<object> interceptors)
        {
            var attributes = type.GetCustomAttributes(typeof(InterceptorAttribute), false);
            attributes = attributes.OrderBy(o => ((InterceptorAttribute)o).Order).ToArray();
            ValidateAttributesOrderInClass(attributes, type);
            interceptors.AddRange(attributes);
        }

        private static void ValidateAttributesOrderInClass(object[] attribues, Type type)
        {
            ValidateAttributesOrder(attribues,
                () => { throw new InvalidClassInterceptorsOrderException(type); });
        }

        private void AddInterceptorToClass(Type type, InterceptorAttribute attribute, List<Interceptor> result)
        {
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                AddInterceptor(attribute, method, result);
            }
        }

        private void CheckForInterceptorsInMethods(Type type, List<Interceptor> result)
        {
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                CheckForInterceptorsInMethod(method, result);
            }
        }

        private void CheckForInterceptorsInMethod(MethodInfo method, List<Interceptor> result)
        {
            var attributes = GetMethodInterceptorsInOrder(method);
            foreach (InterceptorAttribute attribute in attributes)
            {
                AddInterceptor(attribute, method, result);
            }
        }

        private static object[] GetMethodInterceptorsInOrder(MethodInfo method)
        {
            var attributes = new List<object>();
            GetMethodInterceptorsOverClassHierarchy(method, attributes, MaxClassHierarchyDepth);
            return attributes.ToArray();
        }

        private static void GetMethodInterceptorsOverClassHierarchy(MethodInfo method, List<object> interceptors, int depth)
        {
            var baseMethod = GetBaseMethod(method);
            if (baseMethod != null && depth > 0)
            {
                GetMethodInterceptorsOverClassHierarchy(baseMethod, interceptors, depth - 1);
            }
            AddInterceptorsFromMethod(method, interceptors);
        }

        private static MethodInfo GetBaseMethod(MethodInfo method)
        {
            var declaringType = method.DeclaringType;
            var baseType = (declaringType != null) ? declaringType.BaseType : null;
            MethodInfo baseMethod = null;

            if (baseType != null)
            {
                var parametersTypes = method.GetParameters().Select(pi => pi.ParameterType).ToArray();
                baseMethod = baseType.GetMethod(method.Name, parametersTypes);
            }
            return baseMethod;
        }

        private static void AddInterceptorsFromMethod(MethodInfo method, List<object> interceptors)
        {
            var attributes = method.GetCustomAttributes(typeof(InterceptorAttribute), false);
            attributes = attributes.OrderBy(o => ((InterceptorAttribute)o).Order).ToArray();
            ValidateAttributesOrderInMethod(attributes, method);
            interceptors.AddRange(attributes);
        }

        private static void ValidateAttributesOrderInMethod(object[] attribues, MethodInfo method)
        {
            ValidateAttributesOrder(attribues,
                () => { throw new InvalidMethodInterceptorsOrderException(method); });
        }

        private static void ValidateAttributesOrder(object[] attribues, Action OnInvalidOrder)
        {
            var currentOrder = 1;
            foreach (InterceptorAttribute interceptorAttribute in attribues)
            {
                if (interceptorAttribute.Order != currentOrder)
                {
                    OnInvalidOrder();
                }
                currentOrder++;
            }
        }

        private void AddInterceptor(InterceptorAttribute attribute, MethodInfo method, List<Interceptor> result)
        {
            var interceptor = new Interceptor
            {
                InterceptorMethod = GetInterceptorMethod(attribute),
                TargetMethod = method,
                InterceptorProperties = GetInterceptorProperties(attribute)
            };
            result.Add(interceptor);
        }

        private Dictionary<string, object> GetInterceptorProperties(InterceptorAttribute attribute)
        {
            var type = attribute.GetType();
            var properties = type.GetProperties().Where(HasReadWriteAccess);
            var result = new Dictionary<string, object>();

            foreach (var propertyInfo in properties)
            {
                AddPropertyIfNotNull(attribute, propertyInfo, result);
            }
            return result;
        }

        private static void AddPropertyIfNotNull(InterceptorAttribute attribute,
            PropertyInfo propertyInfo, Dictionary<string, object> result)
        {
            var name = propertyInfo.Name;
            var value = propertyInfo.GetValue(attribute);

            if (value != null)
            {
                result.Add(name, value);
            }
        }

        private bool HasReadWriteAccess(PropertyInfo propertyInfo)
        {
            return propertyInfo.CanRead && propertyInfo.CanWrite;
        }

        private MethodInfo GetInterceptorMethod(InterceptorAttribute attribute)
        {
            var type = attribute.GetType();
            return type.GetMethod("Interceptor");
        }
    }
}