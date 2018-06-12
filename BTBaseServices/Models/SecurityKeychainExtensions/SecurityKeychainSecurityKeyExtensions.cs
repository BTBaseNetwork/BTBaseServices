using BTBaseServices.Models;
using Microsoft.IdentityModel.Tokens;

namespace BTBaseServices
{
    public static class SecurityKeychainSecurityKeyExtensions
    {
        public static SecurityKey GetSecurityKeys(this SecurityKeychain keychain, bool includePrivateKey)
        {
            switch (keychain.Algorithm)
            {
                case SecurityKeychainRSAExtensions.ALGORITHM_RSA: return new RsaSecurityKey(keychain.ReadRSAParameters(includePrivateKey));
                case SecurityKeychainSymmetricsExtensions.ALGORITHM_SYMMETRIC: return new SymmetricSecurityKey(keychain.ReadSymmetricsKeyBytes());
                default: return null;
            }
        }
    }

    public class SecurityKeychainProvider
    {
        public static SecurityKeychain Create(string name, string algorithm, string note = null)
        {
            var keychain = new SecurityKeychain()
            {
                Name = name,
                Note = note
            };

            switch (algorithm)
            {
                case SecurityKeychainRSAExtensions.ALGORITHM_RSA: keychain.ResetNewRSAKeys(); break;
                case SecurityKeychainSymmetricsExtensions.ALGORITHM_SYMMETRIC: keychain.ResetNewSymmetricKeys(); break;
                default: return null;
            }
            return keychain;
        }
    }
}