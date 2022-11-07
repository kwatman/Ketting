using KetKoin;
using Ketting_server.Dto;
using Ketting_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ketting_server.Controllers;

public class WalletController : ControllerBase
{
    private BlockChainService blockChainService { get; set; }

    public WalletController(BlockChainService _blockChainService)
    {
        blockChainService = _blockChainService;
    }
    
    [Route("/wallet/{publicKey}/balance")]
    [HttpGet]
    public async Task<float> GetWalletBalance(string publicKey)
    {
        return blockChainService.KetKoinChain.GetBalance(Convert.FromBase64String(publicKey));
    }
    
    [Route("/wallet")]
    [HttpPost]
    public async Task<WalletDto> GetWallet([FromBody] GetWalletDto getWalletDto)
    {
        WalletDto wallet = new WalletDto();
        wallet.Address = getWalletDto.PublicKey;
        wallet.Balance = blockChainService.KetKoinChain.GetBalance(Convert.FromBase64String(getWalletDto.PublicKey));
        wallet.Transactions = new List<TransactionDto>();
        wallet.TransactionsInPool = new List<TransactionDto>();
        foreach (Transaction transaction in blockChainService.KetKoinChain.GetWalletTransactions(Convert.FromBase64String(getWalletDto.PublicKey)))
        {
            TransactionDto transactionDto = new TransactionDto();
            transactionDto.FromObject(transaction);
            wallet.Transactions.Add(transactionDto);
        }

        if (KetKoinChain.TransactionPool.Count > 0)
        {
            foreach (Transaction transaction in KetKoinChain.TransactionPool)
            {
                if(transaction.SenderKey.SequenceEqual(Convert.FromBase64String(getWalletDto.PublicKey)))
                {
                    TransactionDto transactionDto = new TransactionDto();
                    transactionDto.FromObject(transaction);
                    wallet.TransactionsInPool.Add(transactionDto);
                }
            }
        }
        return wallet;
    }
}