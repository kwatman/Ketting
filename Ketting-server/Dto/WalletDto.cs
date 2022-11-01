namespace Ketting_server.Dto;

public class WalletDto
{
    public string Address { get; set; }
    public float Balance { get; set; }
    public List<TransactionDto> Transactions { get; set; }
}