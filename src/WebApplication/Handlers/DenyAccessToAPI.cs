using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication.Handlers
{
    public class DenyAccessToAPI : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.PathAndQuery.StartsWith("/api/secret"))
            {
                //todo: check correctness
                return Task<HttpResponseMessage>.Factory.StartNew(() => request.CreateResponse(HttpStatusCode.Forbidden));
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}