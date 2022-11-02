using KetKoin;
using Ketting_server.Dto;
using Ketting_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ketting_server.Controllers;

public class TransactionController : ControllerBase
{
    private BlockChainService blockChainService { get; set; }

    public TransactionController(BlockChainService _blockChainService)
    {
        blockChainService = _blockChainService;
    }
    
    [Route("/transaction")]
    [HttpPost]
    public async Task AddTransaction([FromBody] TransactionDto transactionDto)
    {
        Console.WriteLine("Recieved transaction");
        blockChainService.AddTransaction(transactionDto);
        if (KetKoinChain.TransactionPool.Count >= 5)
        {
            Console.WriteLine("Starting minting proccess");
            if (Stake.GetHighestStake() == blockChainService.KetKoinChain.NodeKeys.PublicKey)
            {
                Console.WriteLine("Im the leader! Minting new block");
                blockChainService.KetKoinChain.MintBlock();
            }
            else
            {
                Console.WriteLine("Im not the leader.");
            }
        }
    }
}