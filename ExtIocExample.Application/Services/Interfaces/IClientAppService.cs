using System.Collections.Generic;
using Application.Seedwork;
using ExtIocExample.Application.Dtos;

namespace ExtIocExample.Application.Services.Interfaces
{
    public interface IClientAppService
    {
        AppOperationResult<IEnumerable<ClientDto>> GetAll();
        AppOperationResult<ClientDto> GetById(int id);
        AppOperationResult<ClientDto> Create(ClientDto clientDto);
        AppOperationResult<ClientDto> Update(ClientDto clientDto);
        AppOperationResult DeleteById(int id);
    }
}