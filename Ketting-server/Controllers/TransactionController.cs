using System.ComponentModel;
using KetKoin;
using Ketting_server.Dto;
using Ketting_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ketting_server.Controllers;

public class TransactionController : ControllerBase
{
    private BlockChainService blockChainService { get; set; }
    private BroadcastService BroadcastService { get; set; }
    public DiscoveryService DiscoveryService { get; set; }
    public TransactionService TransactionService { get; set; }
    public TransactionController(BlockChainService _blockChainService,BroadcastService broadcastService,DiscoveryService discoveryService,TransactionService transactionService)
    {
        blockChainService = _blockChainService;
        BroadcastService = broadcastService;
        DiscoveryService = discoveryService;
        TransactionService = transactionService;
    }

    private List<TransactionDto> transactionsInProgress = new List<TransactionDto>();
    
    [Route("/transaction")]
    [HttpPost]
    public async Task<IActionResult> AddTransaction([FromBody] TransactionDto transactionDto)
    {
        TransactionService.TransactionBuffer.Add(transactionDto);
        /*transactionsInProgress.Add(transactionDto);
        if (transactionsInProgress.Any(t => t.Signature == transactionDto.Signature))
        {
            return Ok("Already proccesing this request");
        }
        else
        {
            transactionsInProgress.Add(transactionDto);
        }
        Console.WriteLine("Recieved transaction");
        if (KetKoinChain.TransactionPool.Any( t => t.Signature == transactionDto.Signature))
        {
            Console.WriteLine("that transaction has already been added to the pool");
            return Ok("already got that transaction");
        }
        bool success  = blockChainService.AddTransaction(transactionDto);
        if (!success)
        {
            Console.WriteLine("transaction is not vallid");
            return BadRequest("the transaction was not vallid");
        }
        else
        {
            Console.WriteLine("the transaction was valid and has been added to the transaction pool.");
            Console.WriteLine("there are currently: " + KetKoinChain.TransactionPool.Count + " transactions in the pool");
        }
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
        BroadcastService.BroadCastTransaction(transactionDto.ToObject(),DiscoveryService.connections);*/
        return Ok("transaction has been added to the buffer.");
    }
}