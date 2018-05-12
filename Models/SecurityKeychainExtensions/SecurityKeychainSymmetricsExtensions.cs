using System;
using System.Security.Cryptography;
using BTBaseServices.Models;
using Microsoft.IdentityModel.Tokens;

namespace BTBaseServices
{
    public static class SecurityKeychainSymmetricsExtensions
    {
        public const string ALGORITHM_SYMMETRIC = "Symmetric";
        public static void ResetNewSymmetricKeys(this SecurityKeychain keychain)
        {
            keychain.Algorithm = ALGORITHM_SYMMETRIC;
            keychain.CreateDate = DateTime.Now;
            var keyString = BahamutCommon.Encryption.SHA.ComputeSHA256Hash(Guid.NewGuid().ToString() + new Random().Next());
            keychain.PrivateKey = BahamutCommon.Encryption.Base64.EncodeString(keyString);
        }

        public static byte[] ReadSymmetricsKeyBytes(this SecurityKeychain keychain)
        {
            if (keychain.Algorithm == ALGORITHM_SYMMETRIC)
            {
                return Convert.FromBase64String(keychain.PrivateKey);
            }
            else
            {
                throw new System.InvalidOperationException("Not A Symmetrics Security Keychain");
            }
        }
    }
}