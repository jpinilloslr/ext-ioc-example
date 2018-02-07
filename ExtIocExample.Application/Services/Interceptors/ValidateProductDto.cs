using System.Collections.Generic;
using Application.Seedwork;
using Application.Seedwork.Interceptors;
using ExtIocExample.Application.Dtos;

namespace ExtIocExample.Application.Services.Interceptors
{
    public class ValidateProductDto : BaseAppValidationInterceptor
    {
        private ProductDto _productDto;

        protected override void Initialize()
        {
            _productDto = GetArgument<ProductDto>("productDto");
        }

        protected override IEnumerable<Validator> GetValidations()
        {
            return new List<Validator>
            {
                CheckName
            };
        }

        private AppOperationResult CheckName()
        {
            return string.IsNullOrEmpty(_productDto.Name)
                ? AppOperationResult.WithError("nameIsMandatory", "Name is mandatory.")
                : AppOperationResult.Successful();
        }
    }
}