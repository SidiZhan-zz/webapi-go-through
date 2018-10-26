using System;
using System.Net.Http;
using System.Web.Http;
using Autofac;

namespace WebApplication.Test
{
    public class ControllerFactsBase : IDisposable
    {
        protected HttpClient httpClient;
        protected HttpServer httpServer;

        public ControllerFactsBase(Action<ContainerBuilder> CustomizeContainerBuilder)
        {
            var httpConfiguration = new HttpConfiguration();
            var bootstrapper = new Bootstrapper();
            bootstrapper.CustomizeContainerBuilder += (sender, args) => CustomizeContainerBuilder?.Invoke(args.Builder);
            bootstrapper.Initialize(httpConfiguration);
            httpServer = new HttpServer(httpConfiguration);
            httpClient = new HttpClient(httpServer);
        }

        public static void CustomizeContainerBuilder(ContainerBuilder builder)
        {
        }

        public void Dispose()
        {
            httpClient?.Dispose();
            httpServer?.Dispose();
        }
    }
}