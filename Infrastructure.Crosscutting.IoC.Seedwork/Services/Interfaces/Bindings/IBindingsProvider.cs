using System.Collections.Generic;
using Infrastructure.Crosscutting.IoC.Seedwork.Data.Bindings;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Bindings
{
    public interface IBindingsProvider
    {
        IEnumerable<Binding> GetBindings();
    }
}