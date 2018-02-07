using Application.Seedwork.Interceptors;
using Domain.Seedwork.CoreAttributes;
using ExtIocExample.Application.Dtos;
using ExtIocExample.Domain.Aggregates.ProductAggregate;

namespace ExtIocExample.Application.Services.Interceptors
{
    public class CheckProductExistence : BaseAppInterceptor
    {
        [Dependency]
        public IProductRepository ProductRepository { get; set; }
        public CheckBy CheckBy { get; set; }

        protected override void Run()
        {
            var id = GetId();
            if (ProductRepository.Get(id) != null)
            {
                ContinueExecution();
            }
            else
            {
                ReturnFailedOperationResult("inexistentItem", "Inexistent item.");
            }
        }

        private int GetId() => CheckBy == CheckBy.Id
            ? GetArgument<int>("id")
            : GetDto().Id;

        private ProductDto GetDto() => GetArgument<ProductDto>("productDto");
    }
}