using System.Collections.Generic;
using System.Reflection;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Data.Interceptors
{
    public class Interceptor
    {
        public MethodInfo TargetMethod { get; set; }
        public MethodInfo InterceptorMethod { get; set; }
        public Dictionary<string, object> InterceptorProperties { get; set; }

        public override string ToString()
        {
            var interceptorType = (InterceptorMethod.ReflectedType != null)
                ? InterceptorMethod.ReflectedType.Name
                : "UnknownInterceptorType";

            var interceptedType = (TargetMethod.ReflectedType != null)
                ? TargetMethod.ReflectedType.Name
                : "UnknownType";

            return interceptorType + " in " + interceptedType + ":" + TargetMethod.Name;
        }
    }
}