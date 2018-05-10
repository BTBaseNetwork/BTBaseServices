using System;
using System.Security.Cryptography;
using BTBaseServices.Models;
using Microsoft.IdentityModel.Tokens;

namespace BTBaseServices
{
    public class ServerAuthKeyUtils
    {
        public const string ALGORITHM_RSA = "RSA";

        public static T ConvertToKey<T>(BTWebServerAuthKey serverAuthKey) where T : SecurityKey
        {
            switch (serverAuthKey.Algorithm)
            {
                case ALGORITHM_RSA: return ConvertToRsaSecurityKey(serverAuthKey.Key) as T;
                default: return null;
            }
        }

        public static T CreateNewSecurityKey<T>(string algorithm) where T : SecurityKey
        {
            switch (algorithm)
            {
                case ALGORITHM_RSA: return CreateNewRsaSecurityKey() as T;
                default: return null;
            }
        }

        public static BTWebServerAuthKey GenerateAuthKey(string serverName, SecurityKey key)
        {
            var authKey = new BTWebServerAuthKey
            {
                ServerName = serverName
            };
            string algorithm = null;
            authKey.Key = GetAuthKeyString(key, out algorithm);
            if (algorithm != null)
            {
                authKey.Algorithm = algorithm;
                return authKey;
            }
            return null;
        }

        public static string GetAuthKeyString(SecurityKey key, out string algorithm)
        {
            if (key is RsaSecurityKey)
            {
                algorithm = "RSA";
                return GetAuthKeyStringOfRsaSecurityKey(key as RsaSecurityKey);
            }

            algorithm = null;
            return null;
        }

        private static RsaSecurityKey CreateNewRsaSecurityKey()
        {
            using (var rsap = new RSACryptoServiceProvider(2048))
            {
                return new RsaSecurityKey(rsap.ExportParameters(true));
            }
        }

        private static string GetAuthKeyStringOfRsaSecurityKey(RsaSecurityKey key)
        {
            return key.Rsa.ToXmlString(true);
        }

        private static RsaSecurityKey ConvertToRsaSecurityKey(string authKeyString)
        {
            var rsaKey = new RsaSecurityKey(RSA.Create());
            rsaKey.Rsa.FromXmlString(authKeyString);
            return rsaKey;
        }
    }
}