using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class MessageController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage
            {
                Content = new StringContent("Hello World!"),
                StatusCode = HttpStatusCode.Accepted,
            };
        }
    }
}