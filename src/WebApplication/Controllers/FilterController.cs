using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

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

    public class ValidateUserAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            IEnumerable<string> values;
            var request = actionContext.Request;
            if (!request.Headers.TryGetValues("user", out values))
            {
                SetResponse(actionContext);
                return;
            }

            if (values.FirstOrDefault() != "Sidi")
            {
                SetResponse(actionContext);
            }
        }

        static void SetResponse(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
        }
    }

    public class AddNewHeaderFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.Add("newHeader", "hi");
        }
    }
}