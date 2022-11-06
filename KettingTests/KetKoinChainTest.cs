using System;
using System.Collections.Generic;
using KetKoin;
using Ketting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Type = KetKoin.Type;

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
        ketCoin.SetKeys(keyPair.rsa.ExportRSAPrivateKey());
        Console.WriteLine("");
    }
    
    [TestMethod]
    public void AddTransactionToPoolTest()
    {
        KeyPair sender = new KeyPair();
        KeyPair reciever = new KeyPair();

        Transaction transaction = new Transaction(10,sender.rsa.ExportRSAPublicKey(),sender.rsa.ExportRSAPrivateKey(),reciever.rsa.ExportRSAPublicKey(),1, Type.Transaction);
        Assert.IsTrue(ketCoin.AddTransactionToPool(transaction));
        Transaction transaction2 = new Transaction(10,sender.rsa.ExportRSAPublicKey(),reciever.rsa.ExportRSAPrivateKey(),reciever.rsa.ExportRSAPublicKey(),1, Type.Transaction);
        Assert.IsFalse(ketCoin.AddTransactionToPool(transaction2));
    }

    [TestMethod]
    public void GetBalanceTest()
    {
        KeyPair wallet1 = new KeyPair();
        KeyPair wallet2 = new KeyPair();
        KeyPair wallet3 = new KeyPair();
        KeyPair wallet4 = new KeyPair();

        KetKoinChain.TransactionPool.Add(new Transaction(10, wallet1.PublicKey,wallet1.PrivateKey,wallet2.PublicKey,1, Type.Transaction));
        KetKoinChain.TransactionPool.Add(new Transaction(50, wallet3.PublicKey,wallet3.PrivateKey,wallet4.PublicKey,1, Type.Transaction));
        Block block = ketCoin.MintBlock();
        KetKoinChain.BlockChain.Add(block);
        Assert.AreEqual(10,ketCoin.GetBalance(wallet2.PublicKey));
        Assert.AreEqual(50,ketCoin.GetBalance(wallet4.PublicKey));
    }
}

