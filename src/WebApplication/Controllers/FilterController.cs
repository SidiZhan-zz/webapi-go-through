using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication.Filters;

namespace WebApplication.Controllers
{
    public class FilterController : ApiController
    {
        [HttpGet]
        [AddNewHeaderFilter]
        public HttpResponseMessage GetNewHeader(string newHeader)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [ValidateUser]
        public HttpResponseMessage GetSecret()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("i am the secret")
            };
        }
    }
}