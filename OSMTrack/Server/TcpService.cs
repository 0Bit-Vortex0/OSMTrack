using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Concurrent;

namespace OSMTrack.Server
{
    public class TcpService
    {
        private readonly TcpListener _listener;
        private readonly List<TcpClient> _clients;

        public TcpService()
        {
            _listener = new TcpListener(IPAddress.Parse("144.217.170.238"), 8172);
            _clients = new List<TcpClient>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _listener.Start();
            Console.WriteLine("TCP Server Iniciando...");

            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _clients.Add(client);
                Console.WriteLine($"Cliente Conectado: {client.Client.RemoteEndPoint}");
                _ = HandleClientAsync(client, cancellationToken);
            }

            _listener.Stop();
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            using (var networkStream = client.GetStream())
            {
                var buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) != 0)
                {
                    string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Recebendo: {receivedData}");
                }
            }

            Console.WriteLine($"Cliente Desconectado: {client.Client.RemoteEndPoint}");
            _clients.Remove(client);
        }

        public List<string> GetConnectedClients()
        {
            return _clients.Select(c => c.Client.RemoteEndPoint.ToString()).ToList();
        }
    }
}
