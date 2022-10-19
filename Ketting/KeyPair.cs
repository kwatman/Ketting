using System.Security.Cryptography;

namespace Ketting;

public class KeyPair
{
    public RSA rsa { get; set; }
    public KeyPair()
    {
        rsa = RSA.Create(2048);
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