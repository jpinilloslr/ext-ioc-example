using System.Collections.Generic;
using Application.Seedwork;
using Domain.Seedwork.CoreAttributes;
using ExtIocExample.Application.Dtos;
using ExtIocExample.Application.ExternalServices;
using ExtIocExample.Application.Services.Interceptors;
using ExtIocExample.Application.Services.Interfaces;
using ExtIocExample.Domain.Aggregates.ClientAggregate;

namespace ExtIocExample.Application.Services
{
    [Binding]
    public class ClientAppService : IClientAppService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITypeMapperService _typeMapperService;

        public ClientAppService(IClientRepository clientRepository,
            ITypeMapperService typeMapperService)
        {
            _clientRepository = clientRepository;
            _typeMapperService = typeMapperService;
        }

        public AppOperationResult<IEnumerable<ClientDto>> GetAll()
        {
            var entities = _clientRepository.GetAll();
            var dtos = _typeMapperService.MapAsEnumerable<ClientDto>(entities);
            return AppOperationResult<IEnumerable<ClientDto>>.Successful(dtos);
        }

        [CheckClientExistence(CheckBy = CheckBy.Id)]
        public AppOperationResult<ClientDto> GetById(int id)
        {
            var entity = _clientRepository.Get(id);
            var dto = _typeMapperService.Map<ClientDto>(entity);
            return AppOperationResult<ClientDto>.Successful(dto);
        }

        [ValidateClientDto]
        public AppOperationResult<ClientDto> Create(ClientDto clientDto)
        {
            var entity = _typeMapperService.Map<Client>(clientDto);
            var persistedEntity = _clientRepository.Create(entity);
            var dto = _typeMapperService.Map<ClientDto>(persistedEntity);
            _clientRepository.UnitOfWork.Commit();
            return AppOperationResult<ClientDto>.Successful(dto);
        }

        [ValidateClientDto(Order = 2)]
        [CheckClientExistence(Order = 1, CheckBy = CheckBy.Dto)]
        public AppOperationResult<ClientDto> Update(ClientDto clientDto)
        {
            var entity = _typeMapperService.Map<Client>(clientDto);
            var persistedEntity = _clientRepository.Update(entity);
            var dto = _typeMapperService.Map<ClientDto>(persistedEntity);
            _clientRepository.UnitOfWork.Commit();
            return AppOperationResult<ClientDto>.Successful(dto);
        }

        [CheckClientExistence(CheckBy = CheckBy.Id)]
        public AppOperationResult DeleteById(int id)
        {
            _clientRepository.DeleteById(id);
            _clientRepository.UnitOfWork.Commit();
            return AppOperationResult.Successful();
        }
    }
}