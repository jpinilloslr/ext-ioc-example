using System;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Interceptors
{
    public class InvalidClassInterceptorsOrderException : InterceptorException
    {
        public InvalidClassInterceptorsOrderException(Type type) 
            : base(string.Format("Multiple interceptors applied " +
                          "in class \"{0}\" without specifying a correct order. If you are " +
                          "applying multiple interceptors to the same class, " +
                          "ensure that a correct order of execution is declared " +
                          "with \"Order\" property starting in 1.", type.FullName))
        {                        
        }
    }
}
