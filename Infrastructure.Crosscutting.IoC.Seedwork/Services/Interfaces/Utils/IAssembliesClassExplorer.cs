using System;

namespace Infrastructure.Crosscutting.IoC.Seedwork.Services.Interfaces.Utils
{
    public interface IAssembliesClassExplorer
    {
        bool CachedData { get; }
        void ClearCache();
        void Explore(Action<Type> processTypeFunc);
    }
}