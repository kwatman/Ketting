namespace Ketting_server.Services;

public class DiscoveryService
{
    public List<string> connections { get; set; }

    public async void DiscoverConnections()
    {
        if (connections.Count < 20)
        {
            foreach (string connection in connections)
            {
                
            }
        }
    }

    public async Task<List<string>> GetConnections(string connection)
    {   
        HttpClient client = new HttpClient();
        var response = await client.GetStringAsync("https://" + connection +"/discovery/handshake");
        return new List<string>();
    }
}