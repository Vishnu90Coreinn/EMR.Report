using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EMRReport.Common.PwdEncryption
{
    public static class PwdEncryptionExtensions
    {
        private const int keysize = 256;
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("v!5c0re!t3chn0l0");
        private static string EncryptString(string plainText, string passPhrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            byte[] cipherTextBytes = memoryStream.ToArray();
                            return Convert.ToBase64String(cipherTextBytes);
                        }
                    }
                }
            }

        }
        private static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
        }

        public static string EncryptUserString(this string plainText, string userName)
        {
            return EncryptString(plainText, userName);

        }
        public static string DecryptUserString(this string cipherText, string userName)
        {
            if (!string.IsNullOrEmpty(cipherText))
            {
                return DecryptString(cipherText, userName);
            }
            else
                return "";
        }
    }
}
