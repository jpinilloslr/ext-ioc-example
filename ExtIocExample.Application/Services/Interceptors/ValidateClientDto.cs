using System.Collections.Generic;
using Application.Seedwork;
using Application.Seedwork.Interceptors;
using ExtIocExample.Application.Dtos;

namespace ExtIocExample.Application.Services.Interceptors
{
    public class ValidateClientDto : BaseAppValidationInterceptor
    {
        private ClientDto _clientDto;

        protected override void Initialize()
        {
            _clientDto = GetArgument<ClientDto>("clientDto");
        }

        protected override IEnumerable<Validator> GetValidations()
        {
            return new List<Validator>
            {
                CheckFirstName,
                CheckLastName
            };
        }

        private AppOperationResult CheckFirstName()
        {
            return string.IsNullOrEmpty(_clientDto.FirstName)
                ? AppOperationResult.WithError("firstNameIsMandatory", "First name is mandatory.")
                : AppOperationResult.Successful();
        }

        private AppOperationResult CheckLastName()
        {
            return string.IsNullOrEmpty(_clientDto.LastName)
                ? AppOperationResult.WithError("lastNameIsMandatory", "Last name is mandatory.")
                : AppOperationResult.Successful();
        }
    }
}