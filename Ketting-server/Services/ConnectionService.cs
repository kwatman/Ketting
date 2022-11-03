namespace Ketting_server.Services;

public class ConnectionService : BackgroundService
{
    private readonly ILogger<ConnectionService> logger;
    private DiscoveryService discoveryService;

    public ConnectionService(ILogger<ConnectionService> _logger,DiscoveryService _discoveryService)
    {
        logger = _logger;
        discoveryService = _discoveryService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            if (discoveryService.connections.Count < 4)
            {
                List<string> connectionsCopy = discoveryService.connections.ToList();
                connectionsCopy = connectionsCopy.Where(c => c != Environment.GetEnvironmentVariable("base_url"))
                    .ToList();
                foreach (string connection in connectionsCopy)
                {
                    string msg1 = "Checking: " + connection + " for new conections";
                    logger.LogInformation(msg1);
                    List<string> responseConnections = await discoveryService.GetConnections(connection);
                    foreach (string responseConnection in responseConnections)
                    {
                        if (discoveryService.connections.Contains(responseConnection) == false &&
                            Environment.GetEnvironmentVariable("base_url") != responseConnection)
                        {
                            string msg2 = "Found new connection: " + responseConnection;
                            logger.LogInformation(msg2);
                            discoveryService.connections.Add(responseConnection);
                        }

                        await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
                    }
                }
            }

            Console.WriteLine("I currently have the following connections stored: ");
            discoveryService.connections.ForEach(Console.WriteLine);
            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}