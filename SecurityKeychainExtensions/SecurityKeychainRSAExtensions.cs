using System;
using System.Security.Cryptography;
using BTBaseServices.Models;
using Microsoft.IdentityModel.Tokens;

namespace BTBaseServices
{

    #region RSA

    public static class SecurityKeychainRSAExtensions
    {
        public const string ALGORITHM_RSA = "RSA";
        public static void ResetNewRSAKeys(this SecurityKeychain keychain, int keySize = 2048)
        {
            var rsap = new RSACryptoServiceProvider(keySize);
            try
            {
                keychain.Algorithm = ALGORITHM_RSA;
                keychain.CreateDate = DateTime.Now;
                keychain.PublicKey = Convert.ToBase64String(rsap.ExportCspBlob(false));
                keychain.PrivateKey = Convert.ToBase64String(rsap.ExportCspBlob(true));
            }
            finally
            {
                rsap.PersistKeyInCsp = false;
            }
        }

        public static RSAParameters ReadRSAParameters(this SecurityKeychain keychain, bool readPrivateKey)
        {
            var rsap = new RSACryptoServiceProvider();
            var cspblob = Convert.FromBase64String(readPrivateKey ? keychain.PrivateKey : keychain.PublicKey);
            rsap.ImportCspBlob(cspblob);
            return rsap.ExportParameters(readPrivateKey);
        }
    }

    #endregion
}