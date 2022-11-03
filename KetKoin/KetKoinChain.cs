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
    
    public void SetKeys(Byte[] privateKey)
    {
        NodeKeys.PrivateKey = privateKey;
    }
    
    public Block MintBlock()
    {
        Block block = new Block();
        block.PrevHash = BlockChain.MaxBy(b => b.Timestamp).Signature;
        for (int i = 0; i < 5; i++)
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
        block.AmountOfStakers = Stake.GetAmountOfStakers();
        block.Signature = Convert.ToBase64String(NodeKeys.Sign(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(block.Hash)))));
        return block;
    }

    public static float GetBalance(Byte[] walletPublicKey)
    {
        float balance = 0;
        bool foundLatestTransaction = false;
        int ammountOfTransactions = 0;
        List<Block> orderedBlockchain = BlockChain.OrderBy(b => b.Timestamp).ToList();
        foreach (Block block in orderedBlockchain)
        {
            foreach (Transaction transaction in block.Data)
            {
                if ((transaction.RecieverKey.SequenceEqual(walletPublicKey) || transaction.SenderKey.SequenceEqual(walletPublicKey)) && transaction.Type == Type.Transaction)
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
            Console.WriteLine("Count is: " + ammountOfTransactions);
            throw new Exception("Invalid count! balance counting loop has ended but not all transactions have been found");
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

    //TODO needs to be tested
    public List<Transaction> GetWalletTransactions(Byte[] publicKey)
    {
        List<Transaction> transactions = new List<Transaction>();
        List<Block> orderedBlockchain = BlockChain.OrderBy(b => b.Timestamp).ToList();
        foreach (Block block in orderedBlockchain)
        {
            foreach (Transaction transaction in block.Data)
            {
                if (transaction.RecieverKey.SequenceEqual(publicKey) || transaction.SenderKey.SequenceEqual(publicKey))
                {
                    transactions.Add(transaction);
                }
            }
        }

        return transactions;
    }

    public bool AddBlock(Block block)
    {
        if (Block.VerifyBlock(block))
        {
            if (Convert.FromBase64String(block.PublicKey).SequenceEqual(Stake.GetHighestStake()))
            {
                return true;
            }
        }
        return false;
    }
}