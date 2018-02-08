using Domain.Seedwork;
using Domain.Seedwork.CoreAttributes;
using ExtIocExample.Domain.Aggregates.ProductAggregate;

namespace ExtIocExample.Infrastructure.Data
{
    [Binding(SuperType = typeof(IProductRepository))]
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }
    }
}