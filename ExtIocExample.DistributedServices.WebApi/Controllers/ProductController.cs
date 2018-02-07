using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExtIocExample.Application.Dtos;
using ExtIocExample.Application.Services.Interfaces;

namespace ExtIocExample.DistributedServices.WebApi.Controllers
{
    public class ProductController : ApiController
    {
        private readonly IProductAppService _productAppService;

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public HttpResponseMessage Get()
        {
            var result = _productAppService.GetAll();
            return result.Success
                       ? Request.CreateResponse(HttpStatusCode.OK, result.Result)
                       : Request.CreateResponse(HttpStatusCode.BadRequest, result.Error);
        }

        public HttpResponseMessage Get(int id)
        {
            var result = _productAppService.GetById(id);
            return result.Success
                       ? Request.CreateResponse(HttpStatusCode.OK, result.Result)
                       : Request.CreateResponse(HttpStatusCode.BadRequest, result.Error);
        }

        public HttpResponseMessage Post(ProductDto productDto)
        {
            var result = _productAppService.Create(productDto);
            return result.Success
                       ? Request.CreateResponse(HttpStatusCode.Created, result.Result)
                       : Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Error);
        }

        public HttpResponseMessage Post(int id, ProductDto productDto)
        {
            var result = _productAppService.Update(productDto);
            return result.Success
                       ? Request.CreateResponse(HttpStatusCode.Accepted, result.Result)
                       : Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Error);
        }

        public HttpResponseMessage Delete(int id)
        {
            var result = _productAppService.DeleteById(id);
            return result.Success
                       ? Request.CreateResponse(HttpStatusCode.OK)
                       : Request.CreateResponse(HttpStatusCode.NotAcceptable, result.Error);
        }
    }
}