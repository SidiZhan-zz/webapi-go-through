using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication
{
    public class DenyAccessToAPI : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.PathAndQuery.StartsWith("/api/"))
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() => request.CreateResponse(HttpStatusCode.Forbidden));
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}