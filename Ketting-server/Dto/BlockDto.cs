using Ketting;

namespace Ketting_server.Dto;

public class BlockDto
{
    public string PrevHash { get; set; }
    public int Version { get; set; }
    public List<TransactionDto> Data { get; set; }
    public DateTime Timestamp { get; set; }
    public string PublicKey { get; set; }
    public string Hash { get; set; }
    public string Signature { get; set; }
}