using Domain.Seedwork;
using Domain.Seedwork.CoreAttributes;
using ExtIocExample.Domain.Aggregates.ProductAggregate;

namespace Infrastructure.Data
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