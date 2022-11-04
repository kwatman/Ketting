using Ketting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace KetKoin
{
    public class Stake
    {
        // Get the address with the highest stake
        public static Byte[] GetHighestStake()
        {
            return GetStakePerSender().MaxBy(t => t.Value).Key;
        }

        // Get the amount of stakers for the current block
        public static int GetAmountOfStakers()
        {
            return GetStakePerSender().Count();
        }

        public static Dictionary<Byte[], float> GetStakePerSender()
        {
            Dictionary<Byte[], float> stakePerSender = new Dictionary<Byte[], float>();
            List<Block> orderedBlockchain = KetKoinChain.BlockChain.OrderBy(b => b.Timestamp).ToList();
            List<Byte[]> publicKeys = new List<Byte[]>();

            for (int i = 0; i < orderedBlockchain.Count; i++)
            {
                Block block = orderedBlockchain[i];
                Console.WriteLine("Checking block with public key: " + block.PublicKey);
                publicKeys.Add(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(block.PublicKey))));

                foreach (Transaction transaction in block.Data
                .Where(t => t.GetType() == typeof(Transaction))
                .Where(t => ((Transaction) t).Type == Type.Stake)
                .ToList())
                {
                    Console.WriteLine("Checking transaction from: " + Convert.ToBase64String(transaction.SenderKey));
                    if (!stakePerSender.ContainsKey(transaction.SenderKey)
                        && (
                            (Math.Floor((double) (block.AmountOfStakers / 2)) < i
                                && Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(block.PublicKey))).SequenceEqual(transaction.SenderKey))
                            || !Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(block.PublicKey))).SequenceEqual(transaction.SenderKey)
                            )
                        && !publicKeys.Contains(transaction.SenderKey)
                        )
                    {
                        Console.WriteLine("Found valid stake from: " + Convert.ToBase64String(transaction.SenderKey));
                        stakePerSender.Add(transaction.SenderKey, transaction.Amount);
                    }
                }
            }

            return stakePerSender;
        }

        public static bool AddStake(float amount, Byte[] senderPublicKey, Byte[] senderPrivateKey, int transactionNumber)
        {
            Transaction transaction = new Transaction(amount, senderPublicKey, senderPrivateKey, null, transactionNumber, Type.Stake);
            
            if (transaction.Verify())
            {
                KetKoinChain.TransactionPool.Add(transaction);
                return true;
            }
            return false;
        }
    }
}