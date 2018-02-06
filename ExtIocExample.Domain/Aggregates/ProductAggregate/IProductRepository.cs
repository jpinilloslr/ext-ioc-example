using Domain.Seedwork;

namespace ExtIocExample.Domain.Aggregates.ProductAggregate
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}