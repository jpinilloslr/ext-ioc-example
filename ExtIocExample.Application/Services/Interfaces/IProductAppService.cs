using System.Collections.Generic;
using Application.Seedwork;
using ExtIocExample.Application.Dtos;

namespace ExtIocExample.Application.Services.Interfaces
{
    public interface IProductAppService
    {
        AppOperationResult<IEnumerable<ProductDto>> GetAll();
        AppOperationResult<ProductDto> GetById(int id);
        AppOperationResult<ProductDto> Create(ProductDto productDto);
        AppOperationResult<ProductDto> Update(ProductDto productDto);
        AppOperationResult DeleteById(int id);
    }
}