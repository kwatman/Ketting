using KetKoin;
using Ketting;
using Ketting_server.Services;

namespace Ketting_server.Workers;

public class VerifyWorker : BackgroundService
{
    private BlockService BlockService { get; set; }
    private BlockChainService BlockChainService { get; set; }
    private DiscoveryService DiscoveryService { get; set; }
    private BroadcastService BroadcastService { get; set; }

    public VerifyWorker(BlockService blockService,BlockChainService blockChainService,DiscoveryService discoveryService,BroadcastService broadcastService)
    {
        BlockService = blockService;
        BlockChainService = blockChainService;
        DiscoveryService = discoveryService;
        BroadcastService = broadcastService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            if (BlockService.BlockBuffer.Count > 0)
            {
                Block block = BlockService.BlockBuffer[0];
                
                if (KetKoinChain.BlockChain.Any( b => b.Signature == block.Signature))
                {
                    Console.WriteLine("this block has already been added to the blockchain");
                }
                else
                {
                    bool success  = BlockChainService.KetKoinChain.AddBlock(block);
                    if (!success)
                    {
                        Console.WriteLine("Block is not vallid");
                    }
                    else
                    {
                        Console.WriteLine("the tblock has been added to the chain");
                        Console.WriteLine("there are currently: " + KetKoinChain.BlockChain.Count + " blocks in the chain");
                        BroadcastService.BroadCastBlockMint(block,DiscoveryService.connections);
                    }
                }

                BlockService.BlockBuffer.Remove(block);
            }
        }
    }
}