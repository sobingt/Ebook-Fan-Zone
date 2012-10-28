using System;
using System.Security.Cryptography;

namespace EbookZone.Utils.Helpers
{
    public static class EncryptionHelper
    {
        public static string ComputeHash(string text)
        {
            SHA1 sha1 = SHA1.Create();

            byte[] input = System.Text.Encoding.UTF8.GetBytes(text);

            byte[] hash = sha1.ComputeHash(input);

            return Convert.ToBase64String(hash);
        }

        private static readonly byte[] iv = new byte[] { 7, 136, 185, 34, 65, 124, 226, 149, 53, 215, 163, 230, 97, 72, 23, 163 };

        public static string Encrypt(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                using (Rijndael cryptor = Rijndael.Create())
                {
                    ICryptoTransform transformer = cryptor.CreateEncryptor(
                        EncryptionHelper.GetBytes(EncryptionHelper.CreateKey(key)), iv);
                    byte[] source = System.Text.Encoding.UTF8.GetBytes(value);
                    byte[] encrypted = transformer.TransformFinalBlock(source, 0, source.Length);

                    return Convert.ToBase64String(encrypted);
                }
            }
            return string.Empty;
        }

        public static string Decrypt(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                using (Rijndael cryptor = Rijndael.Create())
                {
                    ICryptoTransform transformer = cryptor.CreateDecryptor(
                        EncryptionHelper.GetBytes(CreateKey(key)), iv);
                    byte[] source = Convert.FromBase64String(value);
                    byte[] decrypted = transformer.TransformFinalBlock(source, 0, source.Length);
                    return System.Text.Encoding.UTF8.GetString(decrypted);
                }
            }
            return string.Empty;
        }

        private static byte[] GetBytes(string value)
        {
            return System.Text.Encoding.UTF8.GetBytes(value);
        }

        private static string CreateKey(string key)
        {
            using (Rijndael cryptor = Rijndael.Create())
            {
                int minLength = cryptor.LegalKeySizes[0].MinSize / 8;
                if (key.Length < minLength)
                {
                    string result = key;
                    while (result.Length < minLength)
                    {
                        result += "0";
                    }

                    return result;
                }
                else
                {
                    if (key.Length > minLength)
                    {
                        return key.Substring(0, minLength);
                    }
                    else
                    {
                        return key;
                    }
                }
            }
        }
    }
}