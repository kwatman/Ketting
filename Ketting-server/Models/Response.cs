namespace Ketting_server.Models;

public class Response
{
    public ConnectionResult Result { get; set; }
    public ResponseData Data { get; set; }
    
    public Response()
    {
        
    }
    public Response(ConnectionResult result)
    {
        Result = result;
    }
}