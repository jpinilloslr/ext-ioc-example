using System.Reflection;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Interceptors
{
    public class InvalidMethodInterceptorsOrderException : InterceptorException
    {
        public InvalidMethodInterceptorsOrderException(MethodInfo methodInfo) 
            : base(string.Format("Multiple interceptors applied to method \"{0}\" " +
                          "in class \"{1}\" without specifying a correct order. If you are " +
                          "applying multiple interceptors to the same method, " +
                          "ensure that a correct order of execution is declared " +
                          "with \"Order\" property starting in 1.", methodInfo.Name, 
            (methodInfo.DeclaringType != null)?methodInfo.DeclaringType.FullName:"unknown"))
        {                        
        }
    }
}
