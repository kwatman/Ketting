using KetKoin;
using Ketting_server.Dto;
using Ketting_server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ketting_server.Controllers;

public class WalletController : ControllerBase
{
    private BlockChainService blockChainService { get; set; }

    public WalletController(BlockChainService _blockChainService)
    {
        blockChainService = _blockChainService;
    }
    
    [Route("/wallet/{publicKey}/balance")]
    [HttpGet]
    public async Task<float> GetWalletBalance(string publicKey)
    {
        return KetKoinChain.GetBalance(Convert.FromBase64String(publicKey));
    }
    
    [Route("/wallet/{publicKey}")]
    [HttpGet]
    public async Task<WalletDto> GetWallet(string publicKey)
    {
        WalletDto wallet = new WalletDto();
        wallet.Address = publicKey;
        wallet.Balance = KetKoinChain.GetBalance(Convert.FromBase64String(publicKey));
        
        return wallet;
    }
}