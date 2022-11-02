using KetKoin;
using Ketting;
using Ketting_server.Dto;
using Ketting_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ketting_server.Controllers;

public class BlockChainController : ControllerBase
{
    private BlockChainService blockChainService { get; set; }

    public BlockChainController(BlockChainService _blockChainService)
    {
        blockChainService = _blockChainService;
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
        Block block = blocksDto.ToObject();
        blockChainService.KetKoinChain.AddBlock(block);
    }
}