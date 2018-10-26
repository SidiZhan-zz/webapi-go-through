using System.Threading.Tasks;
using Autofac;
using WebApplication.Services;
using Xunit;

namespace WebApplication.Test
{
    public class MessageControllerFacts : ControllerFactsBase
    {
        public MessageControllerFacts() : base(CustomizeContainerBuilder)
        {
            
        }

        new static void CustomizeContainerBuilder(ContainerBuilder builder)
        {
            builder.Register(_ => new FakeTimeProvider()).As<ITimeProvider>();
        }

        [Fact]
        public async Task should_response_ok()
        {
            var response = await httpClient.GetAsync("http://wow.com/message");
            var content = response.Content;
            var message = await content.ReadAsStringAsync();
            Assert.Equal("1:07 PM Hello World!", message);
        }
    }

    public class FakeTimeProvider : ITimeProvider
    {
        public string GenerateTime()
        {
            return "1:07 PM";
        }
    }
}
