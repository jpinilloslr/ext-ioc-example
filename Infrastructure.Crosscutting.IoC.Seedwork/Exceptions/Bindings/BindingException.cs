using System;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Bindings
{
    public class BindingException : Exception
    {
        public BindingException(string message) : base(message)
        {
        }
    }
}