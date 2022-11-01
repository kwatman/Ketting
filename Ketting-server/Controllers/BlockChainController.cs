using KetKoin;
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
    
}