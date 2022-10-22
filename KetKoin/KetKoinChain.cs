using System.Security.Cryptography;
using System.Text;
using Ketting;

namespace KetKoin;

public class KetKoinChain : KettingChain
{
    public KeyPair NodeKeys { get; set; }
    public static List<Transaction> TransactionPool { get; set; }
    public new static List<Block>  BlockChain { get; set; }

    public KetKoinChain()
    {
        NodeKeys = new KeyPair();
        TransactionPool = new List<Transaction>();
        BlockChain = new List<Block>();
    }
    
    public void SetKeys(Byte[] privateKey, Byte[] publicKey)
    {

        NodeKeys.PrivateKey = privateKey;
    }
    
    public Block MintBlock()
    {
        Block block = new Block();
        if (BlockChain.Count > 0)
        { 
            block.PrevHash = BlockChain.MaxBy(b => b.Timestamp).Signature;   
        }
        else
        {
            block.PrevHash = "genesis";
        }
        for (int i = 0; i < 10; i++)
        {
            if (TransactionPool.Count > 0)
            {
                Transaction takenTransaction = TransactionPool.MinBy(t => t.TimeStamp);
                TransactionPool.Remove(takenTransaction);
                if (takenTransaction.Verify())
                {
                    block.Data.Add(takenTransaction);
                }
            }
        }
        block.Timestamp = DateTime.Now;
        block.PublicKey = Convert.ToBase64String(NodeKeys.rsa.ExportRSAPublicKey());
        block.Hash = Block.HashBlock(block.PrevHash, block.Data, block.Timestamp);
        block.Signature = Convert.ToBase64String(NodeKeys.Sign(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(block.Hash)))));
        return block;
    }

    public static int GetBalance(Byte[] walletPublicKey)
    {
        int balance = 0;
        bool foundLatestTransaction = false;
        int ammountOfTransactions = 0;
        List<Block> orderedBlockchain = BlockChain.OrderBy(b => b.Timestamp).ToList();
        foreach (Block block in orderedBlockchain)
        {
            foreach (Transaction transaction in block.Data)
            {
                if (transaction.RecieverKey.SequenceEqual(walletPublicKey) || transaction.SenderKey.SequenceEqual(walletPublicKey))
                {
                    if (!foundLatestTransaction)
                    {
                        foundLatestTransaction = true;
                        ammountOfTransactions = transaction.TransactionNumber;
                    }
                    balance += transaction.Amount;
                    ammountOfTransactions--;
                }
            }
        }
        if(ammountOfTransactions != 0)
        {
            throw new Exception("Invalid count! balance counting loop has ended but not all tranasctions have been found");
        }
        return balance;
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