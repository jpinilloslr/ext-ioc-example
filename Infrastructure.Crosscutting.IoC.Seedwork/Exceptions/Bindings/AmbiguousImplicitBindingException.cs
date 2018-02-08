namespace Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Bindings
{
    public class AmbiguousImplicitBindingException : BindingException
    {
        public AmbiguousImplicitBindingException(string typeName) :
            base(string.Format("Can not make implicit binding for type {0} " +
                               "because it implements more than one super type. " +
                               "You must specify the super types explicitly.", typeName))
        {
        }
    }
}