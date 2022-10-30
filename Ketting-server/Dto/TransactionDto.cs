using KetKoin;

namespace Ketting_server.Dto;

public class TransactionDto
{
    public int TransactionNumber { get; set; }
    public int Amount { get; set; }
    public DateTime TimeStamp { get; set; }
    public string SenderKey { get; set; }
    public string RecieverKey { get; set; }
    public string Signature { get; set; }

    public void FromObject(Transaction transaction)
    {
        TransactionNumber = transaction.TransactionNumber;
        Amount = transaction.Amount;
        TimeStamp = transaction.TimeStamp;
        SenderKey = Convert.ToBase64String(transaction.SenderKey);
        RecieverKey =  Convert.ToBase64String(transaction.RecieverKey);
        Signature = transaction.Signature;
    }

    public Transaction ToObject()
    {
        Byte[] test = Convert.FromBase64String(SenderKey);
        Byte[] test2 = Convert.FromBase64String(RecieverKey);
        Transaction transaction = new Transaction(Amount,
            Convert.FromBase64String(SenderKey),
            Convert.FromBase64String(RecieverKey),
            TransactionNumber,Signature);
        return transaction;
    }
}