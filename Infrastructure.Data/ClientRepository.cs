﻿using Domain.Seedwork;
using Domain.Seedwork.CoreAttributes;
using ExtIocExample.Domain.Aggregates.ClientAggregate;

namespace ExtIocExample.Infrastructure.Data
{
    [Binding(SuperType = typeof(IClientRepository))]
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }

        public override string FileName => "clients.json";
    }
}