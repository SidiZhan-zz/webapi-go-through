using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;

namespace WebApplication.Test
{
    public class Class1
    {
        [Fact]
        public async Task should_response_ok()
        {
            var config = new HttpConfiguration();
            Bootstrapper.Initialize(config);
            var httpServer = new HttpServer(config);
            var httpClient = new HttpClient(httpServer);

            var response = await httpClient.GetAsync("http://haha.com/message");
            var content = response.Content;
            var message = await content.ReadAsStringAsync();
            Assert.Equal("Hello World!", message);
        }

    }
}
