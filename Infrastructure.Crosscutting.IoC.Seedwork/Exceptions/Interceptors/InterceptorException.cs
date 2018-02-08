namespace Infrastructure.Crosscutting.IoC.Seedwork.Exceptions.Interceptors
{
    public class InterceptorException : IocManagerException
    {
        public InterceptorException(string message) : base(message)
        {
        }
    }
}