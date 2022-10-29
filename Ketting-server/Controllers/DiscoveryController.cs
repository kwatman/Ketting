using Ketting_server.Models;
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
    
    [Route("/discovery/connect/{ip}")]
    public async Task<Response> Connect(string ip)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetStringAsync("https://" +ip +"/discovery/handshake");

        if (response == "Accepted")
        {
            discoveryService.connections.Add(ip);
            return new Response();
        }
        return new Response();
    }
    
    [Route("/discovery/handshake")]
    public async Task<string> Handshake()
    {
        return ConnectionResult.Accepted.ToString();
    }
    
    
}