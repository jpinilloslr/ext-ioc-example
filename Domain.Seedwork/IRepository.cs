using System;
using System.Collections.Generic;

namespace Domain.Seedwork
{
    public interface IRepository<TEntity, in TKey> : IDisposable where TEntity : Entity<TKey>
    {
        IUnitOfWork UnitOfWork { get; }

        TEntity Create(TEntity entity);

        void Delete(TEntity entity);

        void DeleteById(TKey id);

        TEntity Update(TEntity entity);

        TEntity Get(TKey id);

        IEnumerable<TEntity> GetAll();
    }
}