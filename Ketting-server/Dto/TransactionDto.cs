﻿using KetKoin;
using Type = KetKoin.Type;

namespace Ketting_server.Dto;

public class TransactionDto
{
    public int TransactionNumber { get; set; }
    public float Amount { get; set; }
    public DateTime TimeStamp { get; set; }
    public string SenderKey { get; set; }
    public string RecieverKey { get; set; }
    public string Signature { get; set; }
    public int Type { get; set; }
    public void FromObject(Transaction transaction)
    {
        TransactionNumber = transaction.TransactionNumber;
        Amount = transaction.Amount;
        TimeStamp = transaction.TimeStamp;
        SenderKey = Convert.ToBase64String(transaction.SenderKey);
        RecieverKey =  Convert.ToBase64String(transaction.RecieverKey);
        Signature = transaction.Signature;
        Type = (int) transaction.Type;
    }

    public Transaction ToObject()
    {
        Transaction transaction = new Transaction(Amount,
            Convert.FromBase64String(SenderKey),
            Convert.FromBase64String(RecieverKey),
            TransactionNumber,
            Signature,
            (Type) Type
            );
        transaction.TimeStamp = TimeStamp;
        return transaction;
    }
}