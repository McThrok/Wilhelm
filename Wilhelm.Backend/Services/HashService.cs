using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Backend.Model;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Wilhelm.Backend.Services
{
    public class HashService : IHashService
    {
        private readonly HashAlgorithm _cryptoServiceProvider = new SHA256CryptoServiceProvider();
        private const int SaltValueSize = 16;

        public string HashPassword(string password)
        {
            return HashPassword(password, null);
        }
        public bool VerifyHashedPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(password) || hash.Length < SaltValueSize)
                return false;

            string saltValue = hash.Substring(0, SaltValueSize);

            string hashedPassword = HashPassword(password, saltValue);
            if (hash.Equals(hashedPassword, StringComparison.Ordinal))
                return true;

            return false;
        }

        private string HashPassword(string clearData, string saltValue)
        {
            if (saltValue == null)
                saltValue = GenerateSaltValue();
            byte[] hashValue = _cryptoServiceProvider.ComputeHash(PreparePasswordToHash(clearData, saltValue));

            string hashedPassword = saltValue;
            foreach (byte hexdigit in hashValue)
                hashedPassword += hexdigit.ToString("X2", CultureInfo.InvariantCulture.NumberFormat);

            return hashedPassword;
        }
        private string GenerateSaltValue()
        {
            UnicodeEncoding utf16 = new UnicodeEncoding();

            var salt = new byte[SaltValueSize * UnicodeEncoding.CharSize];
            using (var random = new RNGCryptoServiceProvider())
                random.GetNonZeroBytes(salt);

            return utf16.GetString(salt);
        }
        private byte[] PreparePasswordToHash(string clearData, string saltValue)
        {
            UnicodeEncoding utf16 = new UnicodeEncoding();

            byte[] binarySaltValue = utf16.GetBytes(saltValue);
            byte[] binaryPassword = utf16.GetBytes(clearData);
            byte[] valueToHash = new byte[binarySaltValue.Length + binaryPassword.Length];

            binaryPassword.CopyTo(valueToHash, binarySaltValue.Length);

            return valueToHash;

        }
    }
}
