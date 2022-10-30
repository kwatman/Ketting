using Ketting_server.Dto;
using Ketting_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ketting_server.Controllers;

public class DiscoveryController : ControllerBase
{

    private DiscoveryService discoveryService;
    public DiscoveryController(DiscoveryService _discoveryService)
    {
        discoveryService = _discoveryService;
    }
    
    [Route("/discovery/connect")]
    [HttpPost]
    public async Task<ConnectionsShareDto> Connect([FromBody] ConnectionsShareDto connectionsShare)
    {
        // HttpClient client = new HttpClient();
        // var response = await client.GetStringAsync("https://" +ip +"/discovery/handshake");
        
        connectionsShare.connections = discoveryService.connections.ToList();

        if (discoveryService.connections.Count < 10)
        {
            discoveryService.connections.Add(connectionsShare.ip);
        }

        return connectionsShare;
    }


}