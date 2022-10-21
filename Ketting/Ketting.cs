using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace Ketting
{
    public class Ketting
    {
        public List<Block> BlockChain { get; set; }

        public void AddBlock(Block block)
        {
            if (Block.VerifyBlock(block))
            {
                BlockChain.Add(block);
            }
        }

    }
}