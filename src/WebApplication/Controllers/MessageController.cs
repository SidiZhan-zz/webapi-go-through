using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    public class MessageController : ApiController
    {
        readonly MessageService messageService;
        readonly ITimeProvider timeProvider;

        public MessageController(MessageService messageService, ITimeProvider timeProvider)
        {
            this.messageService = messageService;
            this.timeProvider = timeProvider;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var message = messageService.GenerateMessage();
            var time = timeProvider.GenerateTime();
            return new HttpResponseMessage
            {
                Content = new StringContent($"{time} {message}"),
                StatusCode = HttpStatusCode.Accepted,
            };
        }
    }
}