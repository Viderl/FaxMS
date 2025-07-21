using System;
using System.Security.Cryptography;
using System.Text;

namespace FaxMS.Models
{
    public static class SecurityUtil
    {
        // SHA256 雜湊加密
        public static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}