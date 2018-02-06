using System;
using System.Reflection;

namespace Domain.Seedwork.CoreAttributes.Exceptions
{
    public class InvalidInterceptorArgumentIndexException : Exception
    {
        public InvalidInterceptorArgumentIndexException(Type argumentType, int argumentIndex, 
            Type interceptorType, MethodInfo targetMethod) 
            :base("Can not get argument of type '" + argumentType.Name + 
            "' with index '" + argumentIndex + 
            "' in interceptor '" + interceptorType.Name + 
            "' when applied to method '" + targetMethod.Name +
            "' in type '" + targetMethod.DeclaringType.Name + "'.")
        {            
        }
    }
}