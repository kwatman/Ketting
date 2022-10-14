using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Ketting
{
    public class Block
    {
        public string PrevHash { get; set; }
        public int Version { get; set; }
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string PublicKey { get; set; }
        public int Signature { get; set; }


        public int HashBlock(string prevHash, string data, DateTime timestamp)
        {
            string x = prevHash + "@@" +  data + "@@" + timestamp;
            int hash = x.GetHashCode();
            return hash;
        }

        public static bool VerifyBlock(Block block)
        {
            if (block.Data.GetHashCode() == block.Signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }


}
