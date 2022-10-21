using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace Ketting
{
    public class KettingChain
    {
        public List<Block> BlockChain { get; set; }

        public KettingChain()
        {
            BlockChain = new List<Block>();
        }
        
        public bool AddBlock(Block block)
        {
            if (Block.VerifyBlock(block))
            {
                BlockChain.Add(block);
                return true;
            }

            return false;
        }

    }
}