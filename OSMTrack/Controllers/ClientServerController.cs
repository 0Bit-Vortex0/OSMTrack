using Microsoft.AspNetCore.Mvc;
using OSMTrack.Server;

namespace OSMTrack.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ClientServerController : ControllerBase
    {
        private readonly TcpService _tcpService;

        public ClientServerController(TcpService tcpService)
        {
            _tcpService = tcpService;
        }

        [HttpGet]
        [Route("received")]
        public IActionResult GetReceivedMessages()
        {
            var clients = _tcpService.GetConnectedClients();
            return Ok(clients);
        }
    }
}
