using System.Collections.Generic;
using Application.Seedwork;
using Domain.Seedwork.CoreAttributes;
using ExtIocExample.Application.Dtos;
using ExtIocExample.Application.ExternalServices;
using ExtIocExample.Application.Services.Interceptors;
using ExtIocExample.Application.Services.Interfaces;
using ExtIocExample.Domain.Aggregates.ProductAggregate;

namespace ExtIocExample.Application.Services
{
    [Binding]
    public class ProductAppService : IProductAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly ITypeMapperService _typeMapperService;

        public ProductAppService(IProductRepository productRepository,
            ITypeMapperService typeMapperService)
        {
            _productRepository = productRepository;
            _typeMapperService = typeMapperService;
        }

        public AppOperationResult<IEnumerable<ProductDto>> GetAll()
        {
            var entities = _productRepository.GetAll();
            var dtos = _typeMapperService.MapAsEnumerable<ProductDto>(entities);
            return AppOperationResult<IEnumerable<ProductDto>>.Successful(dtos);
        }

        [CheckProductExistence(CheckBy = CheckBy.Id)]
        public AppOperationResult<ProductDto> GetById(int id)
        {
            var entity = _productRepository.Get(id);
            var dto = _typeMapperService.Map<ProductDto>(entity);
            return AppOperationResult<ProductDto>.Successful(dto);
        }

        [ValidateProductDto]
        public AppOperationResult<ProductDto> Create(ProductDto productDto)
        {
            var entity = _typeMapperService.Map<Product>(productDto);
            var persistedEntity = _productRepository.Create(entity);
            var dto = _typeMapperService.Map<ProductDto>(persistedEntity);
            _productRepository.UnitOfWork.Commit();
            return AppOperationResult<ProductDto>.Successful(dto);
        }

        [ValidateProductDto(Order = 2)]
        [CheckProductExistence(Order = 1, CheckBy = CheckBy.Dto)]
        public AppOperationResult<ProductDto> Update(ProductDto productDto)
        {
            var entity = _typeMapperService.Map<Product>(productDto);
            var persistedEntity = _productRepository.Update(entity);
            var dto = _typeMapperService.Map<ProductDto>(persistedEntity);
            _productRepository.UnitOfWork.Commit();
            return AppOperationResult<ProductDto>.Successful(dto);
        }

        [CheckProductExistence(CheckBy = CheckBy.Id)]
        public AppOperationResult DeleteById(int id)
        {
            _productRepository.DeleteById(id);
            _productRepository.UnitOfWork.Commit();
            return AppOperationResult.Successful();
        }
    }
}