﻿using System.Security.Cryptography;

namespace FullStack.API.Helper


{
    public class PasswordHasher
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 10000;

        public static string HashPassword(string password)
        {
            byte[] salt ;  
            rng.GetBytes(salt =new byte[SaltSize]);
            var key = new Rfc2898DeriveBytes(password,salt, Iterations);
            var hash = key.GetBytes(HashSize);
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt,0,hashBytes, 0,SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize,HashSize);
            var base64Hash = Convert.ToBase64String(hashBytes);
                    
            return base64Hash;


        }
        public static bool VerifyPassword(string password, string Base64Hash)
        {

            var hashbytes = Convert.FromBase64String(Base64Hash);
            var salt = new byte[SaltSize];
            Array.Copy(hashbytes, 0, salt, 0, SaltSize);
            var Key = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = Key.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashbytes[i + SaltSize] != hash[i])
                {
                    return false;


                }

            }
            return true;

        }

    }
}
 