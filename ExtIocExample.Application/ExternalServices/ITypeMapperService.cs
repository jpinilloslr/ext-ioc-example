using System.Collections.Generic;

namespace ExtIocExample.Application.ExternalServices
{
    public interface ITypeMapperService
    {
        TTarget Map<TTarget>(object source) where TTarget : class, new();

        IEnumerable<TTarget> MapAsEnumerable<TTarget>(IEnumerable<object> items)
            where TTarget : class, new();
    }
}