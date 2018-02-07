using ExtIocExample.Application.Dtos;
using ExtIocExample.Domain.Aggregates.ClientAggregate;

namespace Infrastructure.Crosscutting.ExternalServices.TypeMapping.Configuration
{
    public class GadgetTypeAndGadgetTypeDtoMap : ITypeMapConfigurator
    {
        public void Configure()
        {
            var map = Mapper.CreateMap<Client, ClientDto>();
            map.ReverseMap();
        }
    }
}