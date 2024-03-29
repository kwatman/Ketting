﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ketting;


namespace Ketting
{
    public class Block
    {
        public string PrevHash { get; set; }
        public int Version { get; set; }
        public List<BlockData> Data { get; set; }
        public DateTime Timestamp { get; set; }
        public string PublicKey { get; set; }
        public string Hash { get; set; }
        public string Signature { get; set; }
        public int AmountOfStakers { get; set; }

        public Block()
        {
            Data = new List<BlockData>();
        }

        /*@
        public int HashBlock(string prevHash, string data, DateTime timestamp)
        {
            string x = prevHash + "@@" +  data + "@@" + timestamp;
            int hash = x.GetHashCode();
            return hash;
        }
        */

        public static string HashBlock(string prevHash, List<BlockData> data, DateTime timestamp)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string dataString = "";
                foreach (BlockData blockData in data)
                {
                    dataString += blockData.ToString() + "@@";
                }
                
                string text = prevHash + "@@" + dataString + "@@" + timestamp;
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text)); // check this later can cause problems cause it does not use base64 but utf8

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerifyBlock(Block block)
        {
            RSA rsaVerify = RSA.Create();
            rsaVerify.ImportRSAPublicKey(Convert.FromBase64String(block.PublicKey),out _);
            //HashBlock(block.PrevHash, block.Data, block.Timestamp)
            if (rsaVerify.VerifyData(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(Block.HashBlock(block.PrevHash,block.Data,block.Timestamp)))),
                    Convert.FromBase64String(block.Signature),
                    HashAlgorithmName.SHA256, 
                    RSASignaturePadding.Pkcs1))
            {
                foreach (BlockData blockData in block.Data)
                {
                    if (!blockData.Verify())
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }



    }


}
