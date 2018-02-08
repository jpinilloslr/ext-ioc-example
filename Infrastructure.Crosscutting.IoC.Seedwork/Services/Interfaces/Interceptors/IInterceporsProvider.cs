using System.Collections.Generic;
using Infrastructure.Crosscutting.IoC.Seedwork.Data.Interceptors;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Interceptors
{
    public interface IInterceptorsProvider
    {
        IEnumerable<Interceptor> GetInterceptors();
    }
}