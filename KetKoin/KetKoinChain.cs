using System.Security.Cryptography;
using System.Text;
using System.Transactions;
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

        Byte[] bytes = new byte[0];
        Transaction minterReward = new Transaction(1,bytes,NodeKeys.PublicKey,1,"reward",Type.Reward);
        
        block.Timestamp = DateTime.Now;
        block.PublicKey = Convert.ToBase64String(NodeKeys.rsa.ExportRSAPublicKey());
        block.Hash = Block.HashBlock(block.PrevHash, block.Data, block.Timestamp);
        block.AmountOfStakers = Stake.GetAmountOfStakers();
        block.Signature = Convert.ToBase64String(NodeKeys.Sign(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(block.Hash)))));
        return block;
    }

    public float GetBalance(Byte[] walletPublicKey)
    {
        float balance = 0;
        List<Block> orderedBlockchain = BlockChain.OrderByDescending(b => b.Timestamp).ToList();

        foreach (Block block in orderedBlockchain)
        {
            Console.WriteLine("Checking transactions from block with timestamp: " + block.Timestamp);
            List<BlockData> orderedTransactions = block.Data.OrderByDescending(t => ((Transaction) t).TimeStamp).ToList();

            foreach (Transaction transaction in orderedTransactions)
            {
                if ((transaction.RecieverKey.SequenceEqual(walletPublicKey) && transaction.Type == Type.Transaction))
                {
                    balance += transaction.Amount;
                }
                
                if ((transaction.RecieverKey.SequenceEqual(walletPublicKey) && transaction.Type == Type.Reward))
                {
                    balance += transaction.Amount;
                }
                
                if ((transaction.SenderKey.SequenceEqual(walletPublicKey) && transaction.Type == Type.Stake))
                {
                    balance -= transaction.Amount;     
                }
                
                if ((transaction.SenderKey.SequenceEqual(walletPublicKey) && transaction.Type == Type.Transaction))
                {
                    balance -= transaction.Amount;     
                }
            }
        }

        return balance;
    }

    public bool AddTransactionToPool(Transaction transaction)
    {
        if (transaction.Verify())
        {
            if (GetBalance(transaction.SenderKey) > 0)
            {
                TransactionPool.Add(transaction);
                return true;
            }
            else
            {
                Console.WriteLine("Wallet does not have that amount of ket to send.");
                return false;
            }
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
        if (!KetKoinChain.BlockChain.Any(b => b.Signature == block.Signature))
        {
            Console.WriteLine("Block not found in blockchain");
            if (Block.VerifyBlock(block) && Convert.FromBase64String(block.PublicKey).SequenceEqual(Stake.GetHighestStake()))
            {
                foreach (Transaction transaction in block.Data)
                {
                    TransactionPool.RemoveAll(t => t.Signature.SequenceEqual(transaction.Signature));
                }

                BlockChain.Add(block);
                return true;
            }
        }

        return false;
    }
}