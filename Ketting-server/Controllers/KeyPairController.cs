using Ketting;
using Ketting_server.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Ketting_server.Controllers;

public class KeyPairController : ControllerBase
{
    [Route("/keypair")]
    [HttpGet]
    public async Task<KeyPairDto> GetKeyPair()
    {
        KeyPair keyPair = new KeyPair();
        KeyPairDto keyPairDto = new KeyPairDto();
        keyPairDto.PrivateKey = Convert.ToBase64String(keyPair.PrivateKey);
        keyPairDto.PublicKey = Convert.ToBase64String(keyPair.PublicKey);
        return keyPairDto;
    }
}