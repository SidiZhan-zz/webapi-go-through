using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Xunit;

namespace WebApplication.Test
{
    public class FilterControllerFacts : ControllerFactsBase
    {
        public FilterControllerFacts() : base(CustomizeContainerBuilder)
        {
            
        }

        [Fact]
        public async Task should_add_header_to_response()
        {
            var response = await httpClient.GetAsync("http://mywebsite.com/filter/after/hi");
            var newHeader = response.Headers.GetValues("newHeader").FirstOrDefault();
            Assert.Equal("hi", newHeader);
        }

        [Fact]
        public async Task should_validate_user()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://mywebsite.com/filter/before/secret");
            request.Headers.Add("user", "Sidi");
            var response = await httpClient.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var secret = await response.Content.ReadAsStringAsync();
            Assert.Equal("i am the secret", secret);
        }

        [Fact]
        public async Task should_not_get_access_to_the_path_starts_with_api()
        {
            var response = await httpClient.GetAsync("http://whatever.com/api/secret/that/you/cannot/see");
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
