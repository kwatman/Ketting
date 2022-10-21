using System.Security.Cryptography;
using System.Text;
using Ketting;

namespace KetKoin;

public class KetKoinChain : KettingChain
{
    public KeyPair NodeKeys { get; set; }
    public static List<Transaction> TransactionPool { get; set; }
    public static List<Block>  BlockChain { get; set; }

    public KetKoinChain()
    {
        NodeKeys = new KeyPair();
        TransactionPool = new List<Transaction>();
    }
    
    public void SetKeys(Byte[] privateKey, Byte[] publicKey)
    {
        int keySize = KeyPair.KEYSIZE;
        NodeKeys.rsa.ImportRSAPrivateKey(privateKey,out keySize);
        NodeKeys.rsa.ImportRSAPublicKey(publicKey,out keySize);
    }
    
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

    public bool AddTransactionToPool(Transaction transaction)
    {
        if (transaction.Verify())
        {
            TransactionPool.Add(transaction);
            return true;
        }
        return false;
    }
}