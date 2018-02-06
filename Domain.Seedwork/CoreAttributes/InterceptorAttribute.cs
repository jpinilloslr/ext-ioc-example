using System;
using System.Linq;
using System.Reflection;
using Domain.Seedwork.CoreAttributes.Exceptions;

namespace Domain.Seedwork.CoreAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public abstract class InterceptorAttribute : Attribute
    {
        private object[] _arguments;
        private MethodInfo _methodInfo;
        private Func<object> _interceptedFunc;
        private Action<object> _returnFunc;
        private object _instance;

        public InterceptorAttribute()
        {
            Order = 1;
        }

        public int Order { get; set; }

        public void Interceptor(object[] arguments, MethodInfo method,
            Func<object> interceptedFunc, Action<object> returnFunc, object instance)
        {
            _arguments = arguments;
            _methodInfo = method;
            _interceptedFunc = interceptedFunc;
            _returnFunc = returnFunc;
            _instance = instance;            
            Run();
        }        

        protected abstract void Run();

        protected TArg GetArgument<TArg>(string argName)
        {
            var parameterInfo = _methodInfo.GetParameters().FirstOrDefault(
                info => info.Name == argName && info.ParameterType == typeof(TArg));

            if (parameterInfo == null)
            {
                throw new InvalidInterceptorArgumentNameException(
                    typeof(TArg), argName, GetType(), _methodInfo);
            }

            var parameterPosition = parameterInfo.Position;
            return (TArg)_arguments[parameterPosition];
        }

        protected TArg GetArgument<TArg>(int index)
        {
            var parameterInfo = _methodInfo.GetParameters().FirstOrDefault(
                info => info.Position == index && info.ParameterType == typeof(TArg));

            if (parameterInfo == null)
            {
                throw new InvalidInterceptorArgumentIndexException(
                    typeof(TArg), index, GetType(), _methodInfo);
            }

            return (TArg)_arguments[index];
        }

        protected object ContinueExecution()
        {
            return _interceptedFunc();
        }

        protected void ForceReturn(object returnValue = null)
        {
            _returnFunc(returnValue);
        }

        protected Type GetInterceptedMethodReturnType()
        {
            return _methodInfo.ReturnType;
        }
    }
}