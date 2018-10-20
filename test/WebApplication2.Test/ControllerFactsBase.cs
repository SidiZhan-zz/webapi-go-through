using System;
using System.Net.Http;
using System.Web.Http;
using WebApplication;

namespace WebApplication2.Test
{
    public class ControllerFactsBase : IDisposable
    {
        protected HttpClient httpClient;
        protected HttpServer httpServer;

        public ControllerFactsBase()
        {
            var httpConfiguration = new HttpConfiguration();
            Bootstrapper.Initialize(httpConfiguration);
            httpServer = new HttpServer(httpConfiguration);
            httpClient = new HttpClient(httpServer);
        }

        public void Dispose()
        {
            httpClient?.Dispose();
            httpServer?.Dispose();
        }
    }
}