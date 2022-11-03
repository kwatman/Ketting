using KetKoin;
using Ketting_server.Dto;

namespace Ketting_server.Services;

public class TransactionService
{
    public List<TransactionDto> TransactionBuffer { get; set; }

    public TransactionService()
    {
        TransactionBuffer = new List<TransactionDto>();
    }
}