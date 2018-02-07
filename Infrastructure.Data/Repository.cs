using System.Collections.Generic;
using System.Linq;
using Domain.Seedwork;

namespace Infrastructure.Data
{
    public abstract class Repository<TEntity> : IRepository<TEntity, int> 
        where TEntity : Entity<int>
    {
        private int _lastGeneratedId;

        public IUnitOfWork UnitOfWork { get; }
        protected List<TEntity> Store { get; set; }

        public Repository(IUnitOfWork unitOfWork)
        {
            _lastGeneratedId = 0;
            Store = new List<TEntity>();
            UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
        }
        
        public TEntity Create(TEntity entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = GetGenerateId();                
            }
            Store.Add(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            DeleteById(entity.Id);
        }

        public void DeleteById(int id)
        {
            var index = Store.FindIndex(x => x.Id == id);
            Store.RemoveAt(index);
        }

        public TEntity Update(TEntity entity)
        {
            Delete(entity);
            return Create(entity);
        }

        public TEntity Get(int id)
        {
            return Store.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Store.ToList();
        }

        private int GetGenerateId()
        {
            _lastGeneratedId++;
            return _lastGeneratedId;
        }
    }
}