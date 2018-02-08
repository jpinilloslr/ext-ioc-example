using System.Collections.Generic;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Utils
{
    public interface IAssembliesEnumerator
    {
        IEnumerable<string> GetAssemblies();
    }
}