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
    }
}