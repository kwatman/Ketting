using Ketting_server.Dto;
using Ketting_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ketting_server.Controllers;

public class DiscoveryController : ControllerBase
{
    private readonly ILogger<DiscoveryController> logger;
    private DiscoveryService discoveryService;
    public DiscoveryController(DiscoveryService _discoveryService,ILogger<DiscoveryController> _logger)
    {
        discoveryService = _discoveryService;
        logger = _logger;
    }
    
    [Route("/discovery/connect")]
    [HttpPost]
    public async Task<ConnectionsShareDto> Connect([FromBody] ConnectionsShareDto connectionsShare)
    {
        // HttpClient client = new HttpClient();
        // var response = await client.GetStringAsync("https://" +ip +"/discovery/handshake");
        
        connectionsShare.connections = discoveryService.connections.ToList();

        if (discoveryService.connections.Count < 4 && discoveryService.connections.Contains(connectionsShare.ip) == false &&
            Environment.GetEnvironmentVariable("base_url") != connectionsShare.ip)
        {
            logger.LogInformation("adding new connection: " + connectionsShare.ip);
            discoveryService.connections.Add(connectionsShare.ip);
        }

        return connectionsShare;
    }


}