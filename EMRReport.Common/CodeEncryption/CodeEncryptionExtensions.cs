using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EMRReport.Common.CodeEncryption
{
    public static class CodeEncryptionExtensions
    {
        private const int keysize = 256;
        private static readonly byte[] initScrubberVectorBytes = Encoding.ASCII.GetBytes("c!5v0r3!t3c5n01o");
        private static string EncryptString(string plainText, string passPhrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;//CBC
                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initScrubberVectorBytes))
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
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initScrubberVectorBytes))
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
        private static string GetPrivatePassword()
        {
            return "A0!ClDW#7K$CpwdLGR";
        }
        private static string EncryptString(string plainText)
        {
            string passPhrase = GetPrivatePassword();
            return EncryptString(plainText, passPhrase);
        }
        private static string LoopEncryptString(string loopplainText)
        {
            StringBuilder sb = new StringBuilder(10);
            string[] splitplainText = loopplainText.Split(',');
            string passPhrase = GetPrivatePassword();
            passPhrase = GetPrivatePassword();
            for (int i = 0; i < splitplainText.Length; i++)
            {
                string plainText = splitplainText[i];
                if (i == 0)
                    sb.Append(!string.IsNullOrEmpty(plainText) ? EncryptString(plainText) : plainText);
                else
                    sb.Append(",").Append(!string.IsNullOrEmpty(plainText) ? EncryptString(plainText) : plainText);
            }
            return sb.ToString();
        }
        private static string LoopDecryptString(string loopEncryptedText)
        {
            StringBuilder sb = new StringBuilder(10);
            string[] splitEncryptedText = loopEncryptedText.Split(',');
            string passPhrase = GetPrivatePassword();
            for (int i = 0; i < splitEncryptedText.Length; i++)
            {
                string encryptedText = splitEncryptedText[i];
                try
                {
                    if (i == 0)
                        sb.Append(encryptedText.Length > 12 ? DecryptString(encryptedText, passPhrase) : encryptedText);
                    else
                        sb.Append(",").Append(encryptedText.Length > 12 ? DecryptString(encryptedText, passPhrase) : encryptedText);
                }
                catch
                {
                    if (i == 0)
                        sb.Append(encryptedText);
                    else
                        sb.Append(",").Append(encryptedText);
                }
            }
            return sb.ToString();
        }

        private static string MessageDecryptScrubberString(string loopEncryptedText)
        {
            StringBuilder sb = new StringBuilder(10);
            string[] splitEncryptedText = loopEncryptedText.Split(new char[] { ':', ',' });
            string passPhrase = GetPrivatePassword();
            for (int i = 0; i < splitEncryptedText.Length; i++)
            {
                string encryptedText = splitEncryptedText[i];
                try
                {
                    if (i == 0)
                        sb.Append(DecryptString(encryptedText, passPhrase));
                    else
                        sb.Append(i == 1 ? " : " : " ").Append(DecryptString(encryptedText, passPhrase));
                }
                catch
                {
                    if (i == 0)
                        sb.Append(encryptedText);
                    else
                        sb.Append(encryptedText);
                }
            }
            return sb.ToString();
        }
        public static string EncryptCode(this string Code)
        {
            return !string.IsNullOrEmpty(Code) ? EncryptString(Code) : "";
        }
        public static string EncryptCommaSeperatedCodes(this string Codes)
        {
            return !string.IsNullOrEmpty(Codes) ? LoopEncryptString(Codes) : "";
        }
        public static string DecriptCommaSeperatedCodes(this string Codes)
        {
            return !string.IsNullOrEmpty(Codes) ? LoopDecryptString(Codes) : "";
        }
        public static string DecriptCommaSeperatedMessage(this string Codes)
        {
            return !string.IsNullOrEmpty(Codes) ? MessageDecryptScrubberString(Codes) : "";
        }
    }
}
