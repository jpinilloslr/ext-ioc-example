using Application.Seedwork.Interceptors;
using Domain.Seedwork.CoreAttributes;
using ExtIocExample.Application.Dtos;
using ExtIocExample.Domain.Aggregates.ClientAggregate;

namespace ExtIocExample.Application.Services.Interceptors
{
    public class CheckClientExistence : BaseAppInterceptor
    {
        [Dependency]
        public IClientRepository ClientRepository { get; set; }
        public CheckBy CheckBy { get; set; }

        protected override void Run()
        {
            var id = GetId();
            if (ClientRepository.Get(id) != null)
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

        private ClientDto GetDto() => GetArgument<ClientDto>("clientDto");
    }
}