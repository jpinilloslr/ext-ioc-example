﻿using Domain.Seedwork;
using Domain.Seedwork.CoreAttributes;

namespace Infrastructure.Data
{
    [Binding(SuperType = typeof(IUnitOfWork))]
    public class UnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
        }

        public void Commit()
        {
        }

        public void RollbackChanges()
        {
        }
    }
}
