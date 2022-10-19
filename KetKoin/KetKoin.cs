using System.Security.Cryptography;
using System.Text;
using Ketting;

namespace KetKoin;

public class KetKoin : Ketting.Ketting
{
    public int TransactionMin { get; set; }
    public KeyPair NodeKeys { get; set; }
    
    public List<Transaction> TransactionPool { get; set; }
    public new List<Block>  BlockChain { get; set; }

    public Block MintBlock()
    {
        Block block = new Block();
        block.PrevHash = BlockChain.MaxBy(b => b.Timestamp).Signature;
        for (int i = 0; i < 10; i++)
        {
            Transaction takenTransaction = TransactionPool.MinBy(t => t.TimeStamp);
            TransactionPool.Remove(takenTransaction);
            if (takenTransaction.Verify())
            {
                block.Data.Add(takenTransaction);
            }
        }
        block.Timestamp = DateTime.Now;
        block.PublicKey = Convert.ToBase64String(NodeKeys.rsa.ExportRSAPublicKey());
        block.Hash = Block.HashBlock(block.PrevHash, block.Data, block.Timestamp);
        block.Signature = Convert.ToBase64String(NodeKeys.Sign(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(block.Hash)))));
        return block;
    }
    
}