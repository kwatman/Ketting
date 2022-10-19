using System;
using System.ComponentModel;
using Ketting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Text;

namespace KettingTests;

[TestClass]
public class KeyPairTest
{
    [TestMethod]
    public void KeyPairSignTest()
    {
        KeyPair keyPair1 = new KeyPair();
        byte[] data = Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes("test data")));
        byte[] signedData = keyPair1.Sign(data);

        KeyPair keyPair2 = new KeyPair();
        Byte[] data2 = Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes("data 2")));
        byte[] signedData2 = keyPair2.Sign(data2);
        
        Assert.IsTrue(KeyPair.Verify(keyPair1.rsa.ExportParameters(false),signedData,data));
        Assert.IsTrue(KeyPair.Verify(keyPair2.rsa.ExportParameters(false),signedData2,data2));
        
        Assert.IsFalse(KeyPair.Verify(keyPair2.rsa.ExportParameters(false),signedData,data));
        Assert.IsFalse(KeyPair.Verify(keyPair1.rsa.ExportParameters(false),signedData2,data2));
        
        Assert.IsFalse(KeyPair.Verify(keyPair1.rsa.ExportParameters(false),signedData,data2));
        Assert.IsFalse(KeyPair.Verify(keyPair2.rsa.ExportParameters(false),signedData2,data));
        
        Assert.IsFalse(KeyPair.Verify(keyPair1.rsa.ExportParameters(false),signedData2,data));
        Assert.IsFalse(KeyPair.Verify(keyPair2.rsa.ExportParameters(false),signedData,data2));
    }
}