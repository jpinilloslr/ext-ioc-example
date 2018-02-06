using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Seedwork
{
    public interface IRepository<TEntity, in TKey> : IDisposable where TEntity : Entity<TKey>
    {
        IUnitOfWork UnitOfWork { get; }

        void Create(TEntity entity);

        void Delete(TEntity entity);

        void DeleteById(TKey id);

        void Update(TEntity entity);

        void TrackItem(TEntity entity);

        TEntity Get(TKey id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetPaged<TProperty>(int pageIndex, int pageCount, 
            Func<TEntity, TProperty> orderByExpression, bool ascending);

        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);
    }
}