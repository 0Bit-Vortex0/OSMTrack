using Microsoft.AspNetCore.Mvc;
using OSMTrack.Server;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace OSMTrack.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ClientServerController : ControllerBase
    {
        [HttpGet]
        [Route("received")]
        public IActionResult GetReceivedMessages()
        {
            try
            {
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IPAddress.Parse("144.217.170.238"), 8172));
                server.Listen(10);

                if (server.Poll(1000000, SelectMode.SelectRead))
                {
                    Socket client = server.Accept();
                    byte[] buffer = new byte[1024];
                    int bytesRead = client.Receive(buffer);

                    string messageReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    server.Close();

                    return Ok(messageReceived);
                }
                else
                {
                    return Ok("No pending connections.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
