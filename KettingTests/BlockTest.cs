using Ketting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;


namespace KettingTests;

[TestClass]
public class BlockTest
{

    [TestMethod]
    public void TestHashBlock()
    {
        Block block = new Block { PrevHash = "RandomString", Version = 1, Data = new List<BlockData>(){new TestData()}, Timestamp = DateTime.Now, PublicKey = "Key"};

        string hash = Block.HashBlock(block.PrevHash, block.Data, block.Timestamp);

        Assert.AreEqual(string.IsNullOrEmpty(hash) , false);
    }

    [TestMethod]
    public void TestVerify()
    {
        Block block = new Block { PrevHash = "RandomString", Version = 1, Data =  new List<BlockData>(){new TestData()}, Timestamp = DateTime.Now, PublicKey = "Key" };

        string hash = Block.HashBlock(block.PrevHash, block.Data, block.Timestamp);
        block.Signature = hash;

        Assert.IsTrue(Block.VerifyBlock(block));
    }
}