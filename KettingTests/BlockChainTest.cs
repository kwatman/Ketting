using System;
using System.Collections.Generic;
using System.Text;
using Ketting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KettingTests;

[TestClass]
public class BlockChainTest
{
    [TestMethod]
    public void AddBlockTest()
    { 
        KettingChain ketting = new KettingChain();
        KeyPair keyPair = new KeyPair();
        Block block = new Block { PrevHash = "RandomString", Version = 1, Data =  new List<BlockData>(){new TestData()}, Timestamp = DateTime.Now, PublicKey = Convert.ToBase64String(keyPair.PublicKey) };
        string hash = Block.HashBlock(block.PrevHash, block.Data, block.Timestamp);
        block.Hash = hash;
        block.Signature = Convert.ToBase64String(keyPair.Sign(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(block.Hash)))));;
        Assert.IsTrue(ketting.AddBlock(block));
        block.Signature = "fake";
        Assert.IsFalse(ketting.AddBlock(block));
    }
}

