using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApplication.Filters
{
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
}