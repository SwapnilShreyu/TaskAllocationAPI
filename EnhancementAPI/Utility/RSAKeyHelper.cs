using System.Security.Cryptography;

namespace EnhancementAPI.Utility
{
    public class RSAKeyHelper
    {
        public static RSAParameters GenerateKey()
        {
            using (var key = new RSACryptoServiceProvider(2048))
            {
                var keydata = key.ExportParameters(true);
                return keydata;
            }
        }
    }
}
