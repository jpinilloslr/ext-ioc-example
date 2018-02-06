using Domain.Seedwork;

namespace ExtIocExample.Domain.Aggregates.ProductAggregate
{
    public class Product : Entity<int>
    {
        public string Name { get; set; }
    }
}