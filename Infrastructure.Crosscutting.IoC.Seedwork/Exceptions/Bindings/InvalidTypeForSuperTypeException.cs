using System;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Bindings
{
    public class InvalidTypeForSuperTypeException : BindingException
    {
        public InvalidTypeForSuperTypeException(string typeName, string superTypeName):
            base(String.Format("Can not bind specified type. {0} is not a super type of {1}.",
                               superTypeName, typeName))
        {
        }
    }
}