using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain.Seedwork;
using Newtonsoft.Json;

namespace ExtIocExample.Infrastructure.Data
{
    public abstract class Repository<TEntity> : IRepository<TEntity, int> 
        where TEntity : Entity<int>
    {
        private List<TEntity> _store;
        public IUnitOfWork UnitOfWork { get; }
        public abstract string FileName { get; }


        public Repository(IUnitOfWork unitOfWork)
        {
            _store = new List<TEntity>();
            UnitOfWork = unitOfWork;
            LoadData();
        }

        public void Dispose()
        {
            SaveData();
        }
        
        public TEntity Create(TEntity entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = GetGenerateId();                
            }
            _store.Add(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            DeleteById(entity.Id);
        }

        public void DeleteById(int id)
        {
            var index = _store.FindIndex(x => x.Id == id);
            _store.RemoveAt(index);
        }

        public TEntity Update(TEntity entity)
        {
            Delete(entity);
            return Create(entity);
        }

        public TEntity Get(int id)
        {
            return _store.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _store.ToList();
        }

        private int GetGenerateId()
        {
            return _store.Count + 1;
        }

        private void SaveData()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(_store);
                writer = new StreamWriter(GetFileName());
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                writer?.Close();
            }
        }

        private void LoadData()
        {
            var fileName = GetFileName();

            if (File.Exists(fileName))
            {
                TextReader reader = null;
                try
                {
                    reader = new StreamReader(GetFileName());
                    var fileContents = reader.ReadToEnd();
                    _store = JsonConvert.DeserializeObject<List<TEntity>>(fileContents);
                }
                finally
                {
                    reader?.Close();
                }
            }
        }

        private string GetFileName()
        {
            return Path.Combine(Path.GetTempPath(), FileName);
        }
    }
}