using KetKoin;
using Ketting_server.Dto;
using Ketting_server.Services;

namespace Ketting_server.Workers;

public class MintingWorker: BackgroundService
{
    private readonly ILogger<MintingWorker> logger;
    private TransactionService TransactionService { get; set; }
    public BlockChainService BlockChainService { get; set; }

    public MintingWorker(ILogger<MintingWorker> _logger,TransactionService transactionService,BlockChainService blockChainService)
    {
        logger = _logger;
        TransactionService = transactionService;
        BlockChainService = blockChainService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            if (KetKoinChain.TransactionPool.Count >= 5)
            {
                Console.WriteLine("Starting minting proccess");
                if (Stake.GetHighestStake() == BlockChainService.KetKoinChain.NodeKeys.PublicKey)
                {
                    Console.WriteLine("Im the leader! Minting new block");
                    BlockChainService.KetKoinChain.MintBlock();
                }
                else
                {
                    Console.WriteLine("Im not the leader.");
                }
            }
        }
    }
}