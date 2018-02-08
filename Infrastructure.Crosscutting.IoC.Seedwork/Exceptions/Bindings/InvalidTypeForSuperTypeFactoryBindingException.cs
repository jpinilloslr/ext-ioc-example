namespace Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Bindings
{
    public class InvalidTypeForSuperTypeFactoryBindingException : BindingException
    {
        public InvalidTypeForSuperTypeFactoryBindingException(string methodName, 
            string returnTypeName, string superTypeName) :
            base(string.Format("Can not make explicit factory binding for method {0} " +
                               "because the return type {1} is not sub type of the super type {2}.",
                               methodName, returnTypeName, superTypeName))
        {
        }
    }
}