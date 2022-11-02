using System.Net;
using System.Net.Security;
using Ketting;
using Ketting_server.Dto;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Ketting_server.Services;

public class DiscoveryService
{
    public List<string> connections { get; set; }
    
    public DiscoveryService()
    {
        connections = new List<string>();
    }
    
    public async void DiscoverConnections(string initialConnection)
    {   
        connections.Add(initialConnection);
        Console.WriteLine("getting init connection from " + initialConnection);
        List<string> responseConnections = await GetConnections(initialConnection);
        foreach (string connection in responseConnections)
        {
            connections.Add(connection);
        }
    }
    
    
    public async Task<List<string>> GetConnections(string connection)
    {
        List<string> connectionsToAdd = new List<string>();
        HttpClient client = new HttpClient();
        try
        {
            ConnectionsShareDto connectionsShareDto = new ConnectionsShareDto();
            connectionsShareDto.ip = Environment.GetEnvironmentVariable("base_url");
            
            var response = await client.PostAsJsonAsync("http://" + connection + "/discovery/connect",connectionsShareDto);
            
            connectionsShareDto = await response.Content.ReadFromJsonAsync<ConnectionsShareDto>();
            connectionsToAdd = connectionsShareDto.connections;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            //Console.WriteLine(e.StackTrace);
        }

        return connectionsToAdd;
    }
}