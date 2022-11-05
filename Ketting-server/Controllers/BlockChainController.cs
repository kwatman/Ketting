using KetKoin;
using Ketting;
using Ketting_server.Dto;
using Ketting_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ketting_server.Controllers;

public class BlockChainController : ControllerBase
{
    private BlockChainService blockChainService { get; set; }
    private BroadcastService broadcastService { get; set; }
    private DiscoveryService discoveryService { get; set; }

    public BlockChainController(BlockChainService _blockChainService, BroadcastService _broadcastService, DiscoveryService _discoveryService)
    {
        blockChainService = _blockChainService;
        broadcastService = _broadcastService;
        discoveryService = _discoveryService;
    }
    
    [Route("/blockchain/count")]
    [HttpGet]
    public async Task<int> GetChainCount()
    {
        return KetKoinChain.BlockChain.Count;
    }
    
    [Route("/blockchain")]
    [HttpGet]
    public async Task<List<BlockDto>> GetBlockChain()
    {
        List<BlockDto> blocks = new List<BlockDto>();
        foreach (Block block in KetKoinChain.BlockChain)
        {
            blocks.Add(new BlockDto(block));
        }
        
        return blocks;
    }
    [Route("/blockchain/block")]
    [HttpPost]
    public async void AddNewBlock([FromBody] BlockDto blocksDto)
    {
        Console.WriteLine("Received new block");
        Block block = blocksDto.ToObject();
        bool blockAdded = blockChainService.KetKoinChain.AddBlock(block);
        if (blockAdded)
        {
            broadcastService.BroadCastBlockMint(block, discoveryService.connections);
        }
    }
}