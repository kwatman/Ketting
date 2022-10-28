using System;
using System.ComponentModel;
using System.Linq;
using Ketting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Text;
using KetKoin;

namespace KettingTests;

[TestClass]
public class StakeTest
{
    [TestMethod]
    public void GetHighestStakeTest()
    {
        KeyPair sender = new KeyPair();
        KeyPair reciever = new KeyPair();

        Transaction transaction = new Transaction(10, sender.rsa.ExportRSAPublicKey(), sender.rsa.ExportRSAPrivateKey(), reciever.rsa.ExportRSAPublicKey(), 1, KetKoin.Type.Stake);
        Transaction transaction2 = new Transaction(20, reciever.rsa.ExportRSAPublicKey(), reciever.rsa.ExportRSAPrivateKey(), sender.rsa.ExportRSAPublicKey(), 2, KetKoin.Type.Stake);
        Transaction transaction3 = new Transaction(15, sender.rsa.ExportRSAPublicKey(), sender.rsa.ExportRSAPrivateKey(), reciever.rsa.ExportRSAPublicKey(), 3, KetKoin.Type.Stake);

        Block block1 = new Block();
        Block block2 = new Block();
        block2.Data.Add(transaction2);
        block1.Data.Add(transaction);
        block2.Data.Add(transaction3);
        block1.AmountOfStakers = 4;

        KetKoinChain.BlockChain.Add(block1);
        KetKoinChain.BlockChain.Add(block2);

        Stake stake = new Stake();
        Byte[] result = stake.GetHighestStake();
        Assert.IsTrue(reciever.rsa.ExportRSAPublicKey().SequenceEqual(result));
    }

    [TestMethod]
    public void AddStakeTest()
    {
        KeyPair sender = new KeyPair();
        Stake.AddStake((float) 0.05, sender.rsa.ExportRSAPublicKey(), sender.rsa.ExportRSAPrivateKey(), 1);
        Assert.IsTrue(KetKoinChain.TransactionPool.Count == 1);
    }
}