using System;
using System.Collections.Generic;
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
        Block block = new Block { PrevHash = "RandomString", Version = 1, Data =  new List<BlockData>(){new TestData()}, Timestamp = DateTime.Now, PublicKey = "Key" };
        string hash = Block.HashBlock(block.PrevHash, block.Data, block.Timestamp);
        block.Signature = hash;
        Assert.IsTrue(ketting.AddBlock(block));
        block.Signature = "fake";
        Assert.IsFalse(ketting.AddBlock(block));
    }
}

