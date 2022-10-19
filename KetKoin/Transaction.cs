using System.Security.Cryptography;
using System.Text;
using Ketting;

namespace KetKoin;

public class Transaction : BlockData
{
    public int Amount { get; set; }

    public DateTime TimeStamp { get; set; }
    public Byte[] SenderKey { get; set; }
    public Byte[] RecieverKey { get; set; }
    public string Signature { get; set; }


    public Transaction(int amount, Byte[] senderKey, Byte[] senderPrivateKey, Byte[] recieverKey)
    {
        Amount = amount;
        SenderKey = senderKey;
        RecieverKey = recieverKey;
        TimeStamp = DateTime.Now;
        string data = senderKey + "@" + recieverKey + "@" + amount + "@" + TimeStamp;

        int keyLength = 2048;
        RSA sender = RSA.Create();
        sender.ImportRSAPublicKey(senderKey,out keyLength);
        sender.ImportRSAPrivateKey(senderPrivateKey, out keyLength);
        Signature = Convert.ToBase64String(sender.SignData(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(data))), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
    }
    
    public bool Verify()
    {
        int keyLength = 2048;
        RSA rsaVerify = RSA.Create();
        rsaVerify.ImportRSAPublicKey(SenderKey,out keyLength);
        string originalData = SenderKey + "@" + RecieverKey + "@" + Amount + "@" + TimeStamp;;
        return rsaVerify.VerifyData(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(originalData))),Convert.FromBase64String(Signature),HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}
