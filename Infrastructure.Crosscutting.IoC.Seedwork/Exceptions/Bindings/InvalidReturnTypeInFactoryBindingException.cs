using System;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Bindings
{
    public class InvalidReturnTypeInFactoryBindingException : BindingException
    {
        public InvalidReturnTypeInFactoryBindingException(string methodName) :
            base(String.Format("Can not make implicit factory binding for method {0} " +
                               "because the return type is not valid.", methodName))
        {
        }
    }
}