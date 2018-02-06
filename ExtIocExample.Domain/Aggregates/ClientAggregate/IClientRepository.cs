using Domain.Seedwork;

namespace ExtIocExample.Domain.Aggregates.ClientAggregate
{
    public interface IClientRepository : IRepository<Client, int>
    {
    }
}