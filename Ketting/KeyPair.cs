using System.Security.Cryptography;

namespace Ketting;

public class KeyPair
{
    public RSACryptoServiceProvider rsa { get; set; }
    public KeyPair()
    {
        rsa = new RSACryptoServiceProvider(2048);
        RSAParameters publicKey = rsa.ExportParameters(false);
        RSAParameters privateKey = rsa.ExportParameters(true);
    }

    public Byte[] Sign(Byte[] data)
    {
        return rsa.SignData(data,SHA256.Create());
    }

    public static bool Verify(RSAParameters key, byte[] dataToVerify,byte[] originalData)
    {
        RSACryptoServiceProvider rsaVerify = new RSACryptoServiceProvider();
        rsaVerify.ImportParameters(key);
        return rsaVerify.VerifyData(originalData,SHA256.Create() ,dataToVerify);
    }
}