using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication.Controllers;
using Xunit;

namespace WebApplication2.Test
{
    public class UserControllerFacts : ControllerFactsBase
    {
        [Fact]
        public async Task should_log_in_successfully_AC1()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://www.haha.com/api/session");
            var requestBody = new UserController.PayloadContractRequest {UserName = "sidi", Password = "correct"};
            request.Content = new ObjectContent<UserController.PayloadContractRequest>(requestBody, new JsonMediaTypeFormatter());
            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("valid-session-token", response.Headers.GetValues("X-User-Session").FirstOrDefault());
        }

        [Fact]
        public async Task should_log_in_forbidden_AC2()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://www.haha.com/api/session");
            var requestBody = new {userName = "sidi", password = "wrong"};
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.False(response.Headers.Contains("X-User-Session"));
        }

        [Fact]
        public async Task should_delete_return_bad_request_AC1()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "http://www.haha.com/api/session");
            var requestBody = new {sessionTokenFake = "whatever"};
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_delete_successfully_AC2()
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "http://www.haha.com/api/session");
            var requestBody = new {sessionToken = "whatever"};
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task should_create_user_return_bad_request_AC1()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://www.haha.com/api/user");
            var requestBody = new {userId = "-1"};
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_create_user_successfully_AC2()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "http://www.haha.com/api/user");
            var requestBody = new {userId = "100"};
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var location = response.Headers.GetValues("Location").FirstOrDefault();
            Assert.Equal("api/user/100", location);
        }

        [Fact]
        public async Task should_get_user_return_not_found_AC1()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.haha.com/api/user/-6?detailed=false");
            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task should_get_user_successfully_AC2()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.haha.com/api/user/90");
            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var message = await response.Content.ReadAsStringAsync();
            Assert.Contains("\"userId\":\"90\"" , message);
        }

        [Fact]
        public async Task should_get_user_successfully_AC3()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.haha.com/api/user/90?detailed=true");
            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var message = await response.Content.ReadAsStringAsync();
            Assert.Contains("\"userId\":\"90\"" , message);
            Assert.Contains("\"details\":\"Blah\"" , message);
        }
    }
}
