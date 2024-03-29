﻿using System.Security.Cryptography;

namespace Ketting;

public class KeyPair
{
    public const int KEYSIZE = 2048;
    public RSA rsa { get; set; }

    public Byte[] PublicKey
    {
        get
        {
            return rsa.ExportRSAPublicKey();
        }
        set
        {
            rsa.ImportRSAPublicKey(value,out _);
        }
    }
    
    public Byte[] PrivateKey
    {
        get
        {
            return rsa.ExportRSAPrivateKey();
        }
        set
        {
            rsa.ImportRSAPrivateKey(value,out _);
        }
    }
    
    public KeyPair()
    {
        rsa = RSA.Create(KEYSIZE);
        Console.WriteLine("");
    }

    public KeyPair(string publicKey)
    {
        
    }

    public Byte[] Sign(Byte[] data)
    {
        return rsa.SignData(data,HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    public static bool Verify(RSAParameters key, byte[] dataToVerify,byte[] originalData)
    {
        RSA rsaVerify = RSA.Create();
        rsaVerify.ImportParameters(key);
        return rsaVerify.VerifyData(originalData,dataToVerify,HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}