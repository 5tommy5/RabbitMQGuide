using Microsoft.AspNetCore.Mvc;
using Producer.Api.Interfaces;
using Producer.Api.Models;

namespace Producer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IRabbitService _service;
        public MessageController(IRabbitService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult PushMessage(Message message)
        {
            _service.SendMessage(message);
            return Ok();
        }
    }
}
