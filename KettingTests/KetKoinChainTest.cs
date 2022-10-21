using System;
using System.Collections.Generic;
using KetKoin;
using Ketting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KettingTests;

[TestClass]
public class KetCoinChainTest
{
    private static KetKoinChain ketCoin;
    private static KeyPair keyPair; 
    
    [ClassInitialize]
    public static void Setup(TestContext context)
    {
        ketCoin = new KetKoinChain();
        keyPair = new KeyPair();
        ketCoin.SetKeys(keyPair.rsa.ExportRSAPrivateKey(),keyPair.rsa.ExportRSAPublicKey());
    }
    
    [TestMethod]
    public void AddTransactionToPoolTest()
    {
        KeyPair sender = new KeyPair();
        KeyPair reciever = new KeyPair();

        Transaction transaction = new Transaction(10,sender.rsa.ExportRSAPublicKey(),sender.rsa.ExportRSAPrivateKey(),reciever.rsa.ExportRSAPublicKey());
        Assert.IsTrue(ketCoin.AddTransactionToPool(transaction));
        Transaction transaction2 = new Transaction(10,sender.rsa.ExportRSAPublicKey(),reciever.rsa.ExportRSAPrivateKey(),reciever.rsa.ExportRSAPublicKey());
        Assert.IsFalse(ketCoin.AddTransactionToPool(transaction2));
    }
}

