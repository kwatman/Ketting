using KetKoin;
using Ketting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KettingTests;

[TestClass]
public class TransactionTest
{
    [TestMethod]
    public void TransactionCreateTest()
    {
        KeyPair sender = new KeyPair();
        KeyPair reciever = new KeyPair();

        Transaction transaction = new Transaction(10,sender.rsa.ExportRSAPublicKey(),sender.rsa.ExportRSAPrivateKey(),reciever.rsa.ExportRSAPublicKey(),1);
        Assert.IsTrue(transaction.Verify());
        
        Transaction fraudTransaction = new Transaction(10,sender.rsa.ExportRSAPublicKey(),reciever.rsa.ExportRSAPrivateKey(),reciever.rsa.ExportRSAPublicKey(),1);
        Assert.IsFalse(fraudTransaction.Verify());
    }
}