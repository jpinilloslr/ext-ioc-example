using System;
using System.Reflection;

namespace Domain.Seedwork.CoreAttributes.Exceptions
{
    public class InvalidInterceptorArgumentNameException : Exception
    {
        public InvalidInterceptorArgumentNameException(Type argumentType, string argumentName, 
            Type interceptorType, MethodInfo targetMethod) 
            :base("Can not get argument of type '" + argumentType.Name + 
            "' with name '" + argumentName + 
            "' in interceptor '" + interceptorType.Name + 
            "' when applied to method '" + targetMethod.Name +
            "' in type '" + targetMethod.DeclaringType.Name + "'.")
        {            
        }
    }
}