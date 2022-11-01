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
            BlockDto blockDto = new BlockDto();
            blockDto.PrevHash = block.PrevHash;
            blockDto.Version = block.Version;
            foreach (Transaction transaction in block.Data)
            {
                TransactionDto transactionDto = new TransactionDto();
                transactionDto.FromObject(transaction);
                blockDto.Data.Add(transactionDto);
            }

            blockDto.Timestamp = block.Timestamp;
            blockDto.PublicKey = block.PublicKey;
            blockDto.Hash = block.Hash;
            blockDto.Signature = block.Signature;
            blocks.Add(blockDto);

        }
        
        return blocks;
    }
    
}