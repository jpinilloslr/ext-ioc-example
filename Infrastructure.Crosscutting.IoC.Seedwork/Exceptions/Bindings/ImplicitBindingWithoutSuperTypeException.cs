using System;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Bindings
{
    public class ImplicitBindingWithoutSuperTypeException : BindingException
    {
        public ImplicitBindingWithoutSuperTypeException(string typeName) :
            base(String.Format("Can not bind type {0} because it " +
                               "is not associated to any super type.", typeName))
        {
        }
    }
}