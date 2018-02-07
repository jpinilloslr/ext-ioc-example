using Domain.Seedwork;
using Domain.Seedwork.CoreAttributes;
using ExtIocExample.Domain.Aggregates.ClientAggregate;

namespace Infrastructure.Data
{
    [Binding(SuperType = typeof(IClientRepository))]
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
    }
}