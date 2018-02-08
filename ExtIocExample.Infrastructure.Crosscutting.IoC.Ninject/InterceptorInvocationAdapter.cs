using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject.Extensions.Interception;

namespace ExtIocExample.Infrastructure.Crosscutting.IoC.Ninject
{
    public class InterceptorInvocationAdapter
    {
        public bool InterceptedFunctionDelegateHandled { get; private set; }

        public object[] GetInterceptorParametersValues(IInvocation invocation, MethodInfo methodInterceptor)
        {
            var output = new List<object>();
            var parameters = methodInterceptor.GetParameters();
            InterceptedFunctionDelegateHandled = false;
            
            foreach (var parameterInfo in parameters)
            {
                if(IsInvocationArguments(parameterInfo))
                {
                    AddInvocationArguments(invocation, output);
                }
                else if(IsReturnValueFunc(parameterInfo))
                {
                    AddReturnValueFunc(invocation, output);
                }
                else if(IsInterceptedFunctionDelegate(parameterInfo))
                {
                    AddInterceptedFunctionDelegate(invocation, output);
                }
                else if(IsTarget(parameterInfo))
                {
                    AddTarget(invocation, output);
                }
                else if(IsInvocatedMethod(parameterInfo))
                {
                    AddInvocatedMethod(invocation, output);
                }
            }

            return output.ToArray();
        }

        private static bool IsInvocationArguments(ParameterInfo parameterInfo)
        {
            return (parameterInfo.ParameterType == typeof (object[]) && 
                parameterInfo.Name != "interceptorParams");
        }

        private static bool IsReturnValueFunc(ParameterInfo parameterInfo)
        {
            return (parameterInfo.ParameterType == typeof(Action<object>));
        }

        private static bool IsTarget(ParameterInfo parameterInfo)
        {
            return (parameterInfo.ParameterType == typeof(object));
        }

        private static bool IsInvocatedMethod(ParameterInfo parameterInfo)
        {
            return (parameterInfo.ParameterType == typeof(MethodInfo));
        }

        private static bool IsInterceptedFunctionDelegate(ParameterInfo parameterInfo)
        {
            return (parameterInfo.ParameterType == typeof(Func<object>));
        }

        private static void AddInvocationArguments(IInvocation invocation, List<object> output)
        {
            output.Add(invocation.Request.Arguments);
        }

        private static void AddReturnValueFunc(IInvocation invocation, List<object> output)
        {
            Action<object> returnValueFunc = delegate(object returnValue)
            {
                invocation.ReturnValue = returnValue;
            };

            output.Add(returnValueFunc);
        }

        private void AddInterceptedFunctionDelegate(IInvocation invocation, List<object> output)
        {
            InterceptedFunctionDelegateHandled = true;

            Func<object> proceedFunc = delegate
            {
                invocation.Proceed();
                return invocation.ReturnValue;
            };

            output.Add(proceedFunc);
        }

        private static void AddTarget(IInvocation invocation, List<object> output)
        {
            output.Add(invocation.Request.Target);
        }

        private static void AddInvocatedMethod(IInvocation invocation, List<object> output)
        {
            output.Add(invocation.Request.Method);
        }
    }
}