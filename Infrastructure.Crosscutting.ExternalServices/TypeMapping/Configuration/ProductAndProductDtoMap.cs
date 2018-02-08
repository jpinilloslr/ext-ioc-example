using AutoMapper;
using ExtIocExample.Application.Dtos;
using ExtIocExample.Domain.Aggregates.ProductAggregate;

namespace ExtIocExample.Infrastructure.Crosscutting.ExternalServices.TypeMapping.Configuration
{
    public class ProductAndProductDtoMap : ITypeMapConfigurator
    {
        public void Configure()
        {
            var map = Mapper.CreateMap<Product, ProductDto>();
            map.ReverseMap();
        }
    }
}