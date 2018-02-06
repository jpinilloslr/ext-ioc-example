using System;

namespace Domain.Seedwork
{
    /// <summary>
    ///     Representa la interfaz principal para implementar el patrón UnitOfWork.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void RollbackChanges();
    }
}
