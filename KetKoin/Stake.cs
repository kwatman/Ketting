using Ketting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace KetKoin
{
    public class Stake
    {
        // Get the address with the highest stake
        public static Byte[] GetHighestStake()
        {
            Byte[] result = null;
            try {
                result = GetStakePerSender(false).MaxBy(t => t.Value.weight).Key;
            }
            catch (Exception e) { 
                result = GetStakePerSender(true).MaxBy(t => t.Value.transaction.TimeStamp).Key;
            }
            return result;
        }

        // Get the amount of stakers for the current block
        public static int GetAmountOfStakers()
        {
            return GetStakePerSender(false).Count();
        }

        public static Dictionary<Byte[], (Transaction transaction, float weight)> GetStakePerSender(bool allowAlreadyStaked)
        {
            Console.WriteLine("Checking stake from blockchain. there are "  + KetKoinChain.BlockChain.Count + " blocks to check");
            Dictionary<Byte[], (Transaction transaction, float weight)> stakers = new Dictionary<Byte[], (Transaction, float)>();
            List<Block> orderedBlockchain = KetKoinChain.BlockChain.OrderBy(b => b.Timestamp).ToList();
            List<Byte[]> publicKeys = new List<Byte[]>();
            

            for (int i = 0; i < orderedBlockchain.Count; i++)
            {
                Block block = orderedBlockchain[i];
                Console.WriteLine("Checking block with public key: ");

                foreach (Transaction transaction in block.Data
                .Where(t => t.GetType() == typeof(Transaction))
                .Where(t => ((Transaction) t).Type == Type.Stake)
                .ToList())
                {
                    Console.WriteLine("Checking transaction from: ");

                    //stakePerSender.Add(transaction.SenderKey, transaction.Amount * (i + 1));
                    if (!stakers.Any(t => t.Key.SequenceEqual(transaction.SenderKey)))
                    { // Is er al een stake van deze sender toegevoegd?
                        Console.WriteLine("No stake from current sender key in collection");
                        if (!publicKeys.Any(key => key.SequenceEqual(transaction.SenderKey)) || allowAlreadyStaked) // Is er al een block geweest met als pub key de huidige sender key
                        {
                            Console.WriteLine("Adding valid stake to list");
                            stakers.Add(transaction.SenderKey, (transaction, transaction.Amount * (i + 1)));
                        }
                    }
                }

                if (!publicKeys.Any(key => key.SequenceEqual(Convert.FromBase64String(block.PublicKey))))
                {
                    if (stakers.Any(t => t.Key.SequenceEqual(Convert.FromBase64String(block.PublicKey))))
                    {
                        Console.WriteLine("Current cooldown calculation: " + (Math.Floor((double)block.AmountOfStakers / 2) - i));
                        if ((Math.Floor((double)block.AmountOfStakers / 2) - i) < 0)
                        {
                            Console.WriteLine("Staker still on cooldown, Removing stake from list");
                            stakers = stakers.Where(s => !s.Key.SequenceEqual(Convert.FromBase64String(block.PublicKey))).ToDictionary(k => k.Key, v => v.Value);
                        }
                    }
                }

                publicKeys.Add(Convert.FromBase64String(block.PublicKey));
            }

            return stakers;
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