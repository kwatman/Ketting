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
        // Amount of blocks person is not allowed to validate another block = floor(amount of stakers / 2)
        // Get the address with the highest stake
        public Byte[] GetHighestStake()
        {
            Dictionary<Byte[], float> totalStakePerSender = new Dictionary<Byte[], float>();
            List<Block> orderedBlockchain = KetKoinChain.BlockChain.OrderBy(b => b.Timestamp).ToList();

            // Just trust me, it works
            for(int i = 0; i < orderedBlockchain.Count; i++)
            {
                foreach (Transaction transaction in orderedBlockchain[i].Data
                .Where(t => t.GetType() == typeof(Transaction))
                .Where(t => ((Transaction) t).Type == Type.Stake)
                .ToList())
                {
                    if (!totalStakePerSender.ContainsKey(transaction.SenderKey) && Math.Floor((double) (orderedBlockchain[i].AmountOfStakers / 2)) < i)
                    {
                         totalStakePerSender.Add(transaction.SenderKey, transaction.Amount);
                    }
                }
            }

            return totalStakePerSender.MaxBy(t => t.Value).Key;
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