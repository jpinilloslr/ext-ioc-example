using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExtIocExample.Application.Dtos;
using ExtIocExample.Application.Services.Interfaces;

namespace ExtIocExample.DistributedServices.WebApi.Controllers
{
    public class ClientController : ApiController
    {
        private readonly IClientAppService _clientAppService;

        public ClientController(IClientAppService clientAppService)
        {
            _clientAppService = clientAppService;
        }

        public HttpResponseMessage Get()
        {
            var result = _clientAppService.GetAll();
            return result.Success
                       ? Request.CreateResponse(HttpStatusCode.OK, result.Result)
                       : Request.CreateResponse(HttpStatusCode.BadRequest, result.Error);
        }

        public HttpResponseMessage Get(int id)
        {
            var result = _clientAppService.GetById(id);
            return result.Success
                       ? Request.CreateResponse(HttpStatusCode.OK, result.Result)
                       : Request.CreateResponse(HttpStatusCode.BadRequest, result.Error);
        }

        public HttpResponseMessage Post(ClientDto clientDto)
        {
            var result = _clientAppService.Create(clientDto);
            return result.Success
                       ? Request.CreateResponse(HttpStatusCode.Created, result.Result)
                       : Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Error);
        }

        public HttpResponseMessage Post(int id, ClientDto clientDto)
        {
            var result = _clientAppService.Update(clientDto);
            return result.Success
                       ? Request.CreateResponse(HttpStatusCode.Accepted, result.Result)
                       : Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Error);
        }

        public HttpResponseMessage Delete(int id)
        {
            var result = _clientAppService.DeleteById(id);
            return result.Success
                       ? Request.CreateResponse(HttpStatusCode.OK)
                       : Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Error);
        }
    }
}