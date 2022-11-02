using Ketting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;


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
        KeyPair keyPair = new KeyPair();
        Block block = new Block { PrevHash = "RandomString", Version = 1, Data =  new List<BlockData>(){new TestData()}, Timestamp = DateTime.Now, PublicKey = Convert.ToBase64String(keyPair.PublicKey) };
        block.Hash = Block.HashBlock(block.PrevHash, block.Data, block.Timestamp);;
        block.Signature = Convert.ToBase64String(keyPair.Sign(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(block.Hash)))));;

        Assert.IsTrue(Block.VerifyBlock(block));
        KeyPair keyPair2 = new KeyPair();
        block.Signature = Convert.ToBase64String(keyPair2.Sign(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(block.Hash)))));;;
        Assert.IsFalse(Block.VerifyBlock(block));
        
    }
}