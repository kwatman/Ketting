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
    public void GetMaxTest()
    {
        KeyPair sender = new KeyPair();
        KeyPair reciever = new KeyPair();

        Transaction transaction = new Transaction(10, sender.rsa.ExportRSAPublicKey(), sender.rsa.ExportRSAPrivateKey(), reciever.rsa.ExportRSAPublicKey(), 1, KetKoin.Type.Stake);
        Transaction transaction2 = new Transaction(20, reciever.rsa.ExportRSAPublicKey(), reciever.rsa.ExportRSAPrivateKey(), sender.rsa.ExportRSAPublicKey(), 1, KetKoin.Type.Stake);
        Transaction transaction3 = new Transaction(15, sender.rsa.ExportRSAPublicKey(), sender.rsa.ExportRSAPrivateKey(), reciever.rsa.ExportRSAPublicKey(), 1, KetKoin.Type.Stake);

        Block block = new Block();
        block.Data.Add(transaction2);
        block.Data.Add(transaction);
        block.Data.Add(transaction3);

        Stake stake = new Stake();
        Byte[] result = stake.GetHighestTotalStakeFromBlock(block);
        Assert.IsTrue(reciever.rsa.ExportRSAPublicKey().SequenceEqual(result));
    }
}