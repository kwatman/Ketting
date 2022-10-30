using KetKoin;
using Ketting_server.Dto;

namespace Ketting_server.Services;

public class BlockChainService
{
    public KetKoinChain KetKoinChain { get; set; }
    public BlockChainService(IConfiguration configuration)
    {
        KetKoinChain = new KetKoinChain();
        KetKoinChain.SetKeys(Convert.FromBase64String(configuration["privateKey"]));
    }

    public void AddTransaction(TransactionDto transaction)
    {
        KetKoinChain.AddTransactionToPool(transaction.ToObject());
    }
}