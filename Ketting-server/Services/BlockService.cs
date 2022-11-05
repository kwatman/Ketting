using Ketting;

namespace Ketting_server.Services;


public class BlockService
{
    public List<Block> BlockBuffer { get; set; }

    public BlockService()
    {
        BlockBuffer = new List<Block>();
    }
}
