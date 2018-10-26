using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApplication.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage LogIn(PayloadContractRequest request)
        {
            if (request.UserName == "sidi" && request.Password == "correct")
            {
                var response = new HttpResponseMessage(HttpStatusCode.Created);
                response.Headers.Add("X-User-Session", "valid-session-token");
                return response;
            }
            return new HttpResponseMessage(HttpStatusCode.Forbidden);
        }

        [HttpDelete]
        public HttpResponseMessage Cancel(PayloadContractRequest request)
        {
            if (request.SessionToken != null)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public HttpResponseMessage Create(PayloadContractRequest request)
        {
            if (request.UserId >= 0)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Created);
                response.Headers.Add("Location", $"api/user/{request.UserId}");
                return response;
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpGet]
        public HttpResponseMessage GetUser(long id)
        {
            if (id >= 0)
            {
                var responseMessage = new
                {
                    userId = id.ToString(),
                    details = default(string),
                };
                var detailed = Request.RequestUri.ParseQueryString().Get("detailed");
                if (detailed == "true")
                {
                    responseMessage = new
                    {
                        userId = id.ToString(),
                        details = "Blah",
                    };
                }
                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public class PayloadContractRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string SessionToken { get; set; }
            public int UserId { get; set; }
        }
    }
}