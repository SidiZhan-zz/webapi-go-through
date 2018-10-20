using System.Threading.Tasks;
using Xunit;

namespace WebApplication2.Test
{
    public class MessageControllerFacts : ControllerFactsBase
    {
        [Fact]
        public async Task should_response_ok()
        {
            
            var response = await httpClient.GetAsync("http://wow.com/message");
            var content = response.Content;
            var message = await content.ReadAsStringAsync();
            Assert.Equal("Hello World!", message);
        }
    }
}
