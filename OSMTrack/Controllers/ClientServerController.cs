using Microsoft.AspNetCore.Mvc;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace OSMTrack.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ClientServerController : ControllerBase
    {
        [HttpPost]
        [Route("ConnectServer")]
        public async Task<IActionResult> ConnectServer()
        {
            try
            {
                string host = "osmtrack.cargotrack360.com.br";
                int port = 443;

                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(host, port);
                    if(client.Connected)
                    {
                        using (SslStream sslStream = new SslStream(client.GetStream(), false))
                        {
                            await sslStream.AuthenticateAsClientAsync(host);
                            string requestMessage = "GET / HTTP/1.1\r\n" + $"Host: {host}\r\n" + "Connection: close\r\n" + "\r\n";
                            byte[] requestBytes = Encoding.ASCII.GetBytes(requestMessage);

                            await sslStream.WriteAsync(requestBytes, 0, requestBytes.Length);
                            await sslStream.FlushAsync();
                            using (StreamReader reader = new StreamReader(sslStream, Encoding.ASCII))
                            {
                                string response = await reader.ReadToEndAsync();
                                var result = new
                                {
                                    Response = response
                                };

                                return Ok(result);
                            }
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                var errorResult = new
                {
                    Error = $"Erro ao conectar ao servidor: {ex.Message}"
                };
                return StatusCode(500, errorResult);
            }
        }
    }
}
