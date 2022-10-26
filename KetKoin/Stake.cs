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
        public Byte[] GetHighestStake()
        {
            Dictionary<Byte[], float> totalStakePerSender = new Dictionary<Byte[], float>();
            List<Block> orderedBlockchain = KetKoinChain.BlockChain.OrderBy(b => b.Timestamp).ToList();

            // Just trust me, it works
            foreach(Block block in orderedBlockchain)
            {
                foreach (Transaction transaction in block.Data
                .Where(t => t.GetType() == typeof(Transaction))
                .Where(t => ((Transaction)t).Type == Type.Stake)
                .ToList())
                {
                    if (!totalStakePerSender.ContainsKey(transaction.SenderKey))
                        totalStakePerSender.Add(transaction.SenderKey, transaction.Amount);                        
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