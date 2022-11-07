using KetKoin;
using Ketting_server.Dto;
using Ketting_server.Services;
using Type = KetKoin.Type;

namespace Ketting_server.Workers;

public class TransactionWorker: BackgroundService
{
    private readonly ILogger<TransactionWorker> logger;
    private TransactionService TransactionService { get; set; }
    public BlockChainService BlockChainService { get; set; }
    public DiscoveryService DiscoveryService { get; set; }
    public BroadcastService BroadcastService { get; set; }

    public TransactionWorker(ILogger<TransactionWorker> _logger,TransactionService transactionService,BlockChainService blockChainService,DiscoveryService discoveryService,BroadcastService broadcastService)
    {
        logger = _logger;
        TransactionService = transactionService;
        BlockChainService = blockChainService;
        DiscoveryService = discoveryService;
        BroadcastService = broadcastService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            if (TransactionService.TransactionBuffer.Count > 0)
            {
                TransactionDto transaction = TransactionService.TransactionBuffer[0];

                Console.WriteLine("Recieved transaction");
                if (KetKoinChain.TransactionPool.Any( t => t.Signature == transaction.Signature || 
                                                           (t.TransactionNumber == transaction.TransactionNumber && t.SenderKey.SequenceEqual(transaction.ToObject().SenderKey) && t.Type == transaction.ToObject().Type  )))
                {
                    Console.WriteLine("that transaction has already been added to the pool");
                }
                else
                {
                    bool success  = BlockChainService.AddTransaction(transaction);
                    if (!success)
                    {
                        Console.WriteLine("transaction is not vallid");
                    }
                    else
                    {
                        Console.WriteLine("the transaction was valid and has been added to the transaction pool.");
                        Console.WriteLine("there are currently: " + KetKoinChain.TransactionPool.Count + " transactions in the pool");
                        BroadcastService.BroadCastTransaction(transaction.ToObject(),DiscoveryService.connections);
                    }
                }

                TransactionService.TransactionBuffer.Remove(transaction);
            }
        }
    }
}