using System.Web.Http.Filters;

namespace WebApplication.Filters
{
    public class AddNewHeaderFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.Add("newHeader", "hi");
        }
    }
}