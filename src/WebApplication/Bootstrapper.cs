using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebApplication
{
    public class Bootstrapper
    {
        public static void Initialize(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new DenyAccessToAPI());

            config.Routes.MapHttpRoute(
                "get message",
                "message",
                new {controller = "Message", action = "Get"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Get)});

            config.Routes.MapHttpRoute(
                "add a new header",
                "filter/after/{newHeader}",
                new {controller = "Filter", action = "GetNewHeader"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Get)});

            config.Routes.MapHttpRoute(
                "validate a user",
                "filter/before/secret",
                new {controller = "Filter", action = "GetSecret"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Get)});
        }
    }
}