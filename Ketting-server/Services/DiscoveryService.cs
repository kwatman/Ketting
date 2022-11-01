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
        List<string> responseConnections = await GetConnections(initialConnection);
        foreach (string connection in responseConnections)
        {
            connections.Add(connection);
        }

        ConnectLoop();
    }

    public async void ConnectLoop()
    {
        while (true)
        {
            foreach (string connection in connections)
            {
                List<string> responseConnections = await GetConnections(connection);
                foreach (string responseConnection in responseConnections)
                {
                    Console.WriteLine("Checking: " + responseConnection + " for new conections");
                    if (!connections.Any(item => item == responseConnection) && Environment.GetEnvironmentVariable("base_url") != responseConnection)
                    {
                        Console.WriteLine("Found new connection: " + responseConnection);
                        connections.Add(responseConnection);
                    }
                    Thread.Sleep(1000);
                }
            }
            Thread.Sleep(10000);
        }
    }
    
    public async Task<List<string>> GetConnections(string connection)
    {
        List<string> connectionsToAdd = new List<string>();
        HttpClient client = new HttpClient();
        try
        {
            ConnectionsShareDto connectionsShareDto = new ConnectionsShareDto();
            connectionsShareDto.ip = Environment.GetEnvironmentVariable("initial_conection");
            var response = await client.PostAsJsonAsync("http://" + connection + "/discovery/connect",connectionsShareDto);
            connectionsShareDto = await response.Content.ReadFromJsonAsync<ConnectionsShareDto>();
            connectionsShareDto.connections.ForEach(Console.WriteLine);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return connectionsToAdd;
    }
}