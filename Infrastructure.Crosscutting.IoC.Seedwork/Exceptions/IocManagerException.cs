using System;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Exceptions
{
    public class IocManagerException : Exception
    {
        public IocManagerException(string message) : base(message)
        {
        }
    }
}