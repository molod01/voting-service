using System.Security.Cryptography;
using System;
using System.Text;
namespace voterilka.Crypto
{
    static class Hasher
    {
        public static string GetHash(string password)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

    }
}
