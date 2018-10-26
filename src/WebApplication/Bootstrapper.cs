using System;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using WebApplication.Controllers;
using WebApplication.Handlers;
using WebApplication.Services;

namespace WebApplication
{
    public class Bootstrapper
    {
        public void Initialize(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new DenyAccessToAPI());

            ConfigDependencyResolver(config);

            ConfigRoutes(config);
        }

        public void ConfigDependencyResolver(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MessageService>();
            builder.RegisterType<TimeProvider>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            CustomizeContainerBuilder?.Invoke(this, new CustomizeContainerBuilderEventArgs(builder));

            config.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
        }

        public EventHandler<CustomizeContainerBuilderEventArgs> CustomizeContainerBuilder { get; set; }

        static void ConfigRoutes(HttpConfiguration config)
        {
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

            config.Routes.MapHttpRoute(
                "log in",
                "api/session",
                new {controller = "User", action = "LogIn"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Post)});

            config.Routes.MapHttpRoute(
                "cancel",
                "api/session",
                new {controller = "User", action = "Cancel"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Delete)});

            config.Routes.MapHttpRoute(
                "create user",
                "api/user",
                new {controller = "User", action = "Create"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Post)});

            config.Routes.MapHttpRoute(
                "get user",
                "api/user/{id}",
                new {controller = "User", action = "GetUser"},
                new {httpMethod = new HttpMethodConstraint(HttpMethod.Get)});
        }
    }

    public class CustomizeContainerBuilderEventArgs : EventArgs
    {
        public ContainerBuilder Builder { get; set; }

        public CustomizeContainerBuilderEventArgs(ContainerBuilder builder)
        {
            Builder = builder;
        }
    }
}