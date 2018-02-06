using System;
using System.Reflection;

namespace Domain.Seedwork.CoreAttributes.Exceptions
{
    public class InvalidInterceptorParameterException : Exception
    {
        public InvalidInterceptorParameterException(Type parameterType, byte parameterIndex, 
            Type interceptorType, MethodInfo targetMethod) 
            :base("Can not get parameter of type '" + parameterType.Name + 
            "' with index " + parameterIndex + 
            "' in interceptor " + interceptorType.Name + 
            "' when applied to method '" + targetMethod.Name +
            "' in type '" + targetMethod.DeclaringType.Name + "'.")
        {            
        }
    }
}