using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Ketting;

namespace KetKoin;

public class Transaction : BlockData
{
    public int TransactionNumber { get; set; }
    public float Amount { get; set; }
    public DateTime TimeStamp { get; set; }
    public Byte[] SenderKey { get; set; }
    public Byte[] RecieverKey { get; set; }
    public string Signature { get; set; }
    public Type Type { get; set; }
    
    
    public Transaction(float amount, Byte[] senderKey, Byte[] recieverKey,int transactionNumber,string signature,Type type)
    {
        TransactionNumber = transactionNumber;
        Amount = amount;
        SenderKey = senderKey;
        RecieverKey = recieverKey;
        TimeStamp = DateTime.Now;
        Signature = signature;
        Type = type;
    }
    public Transaction(float amount, Byte[] senderKey, Byte[] senderPrivateKey, Byte[] recieverKey,int transactionNumber,Type type)
    {
        TransactionNumber = transactionNumber;
        Amount = amount;
        SenderKey = senderKey;
        RecieverKey = recieverKey;
        TimeStamp = DateTime.Now;
        Type = type;
        string data = transactionNumber + "@" + Convert.ToBase64String(senderKey) + "@" + Convert.ToBase64String(recieverKey) + "@" + amount + "@" + TimeStamp.ToString("yyyy-MM-ddTHH:mm:ss");

        int keyLength = 2048;
        RSA sender = RSA.Create();
        sender.ImportRSAPublicKey(senderKey,out keyLength);
        sender.ImportRSAPrivateKey(senderPrivateKey, out keyLength);
        Signature = Convert.ToBase64String(sender.SignData(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(data))), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
    }
    
    public bool Verify()
    {
        bool correct = true;
        RSA rsaVerify = RSA.Create();
        rsaVerify.ImportRSAPublicKey(SenderKey,out _);
        string originalData = TransactionNumber + "@" + Convert.ToBase64String(SenderKey) + "@" + Convert.ToBase64String(RecieverKey) + "@" + Amount + "@" + TimeStamp.ToString("yyyy-MM-ddTHH:mm:ss");
        //Console.WriteLine(originalData);
        if(Type == Type.Transaction || Type == Type.Stake){
            if (!rsaVerify.VerifyData(Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(originalData))),
                    Convert.FromBase64String(Signature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1))
            {
                Console.WriteLine("Signature of transaction is not vallid.");
                correct = false;
            }
        }
        else if(Type == Type.Reward)
        {
            if (Stake.GetHighestStake() != RecieverKey || Amount != 1)
            {
                correct = false;
            }
        }

        List<Block> orderedBlockchain = KetKoinChain.BlockChain.OrderBy(b => b.Timestamp).ToList();

        foreach (Block block in orderedBlockchain)
        {
            foreach(Transaction transaction in block.Data)
            {
                if (transaction.SenderKey.SequenceEqual(SenderKey))
                {
                    if (transaction.TransactionNumber >= TransactionNumber)
                    {
                        Console.WriteLine("Transaction number already exists.");
                        correct = false;
                    }
                }
            }
        }
        return correct;
    }

    public override bool Equals(object? obj)
    {
        return obj is Transaction transaction &&
               TransactionNumber == transaction.TransactionNumber &&
               Signature == transaction.Signature;
    }
}
public enum Type
{
    Stake,
    Transaction,
    Reward
}
