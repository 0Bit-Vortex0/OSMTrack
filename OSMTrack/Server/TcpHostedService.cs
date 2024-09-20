namespace OSMTrack.Server
{
    public class TcpHostedService : IHostedService
    {
        private readonly TcpService _tcpService;

        public TcpHostedService(TcpService tcpService)
        {
            _tcpService = tcpService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _tcpService.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
