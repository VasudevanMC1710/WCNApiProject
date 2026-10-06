using System.Security.Cryptography;
using System.Text;

namespace WCNApi.Infrastructure.Helpers
{
    public static class CryptoHelper
    {
        public static string SecretKey { get; set; }
        public static string ResponseSecretKey { get; set; }

        public static string EncryptData(string data, bool isURLData = true)
        {
            if (SecretKey.Length > 32)
            {
                SecretKey = SecretKey.Substring(0, 32);
            }
            else if (SecretKey.Length < 32)
            {
                SecretKey = SecretKey.PadRight(32, ' ');
            }
            byte[] keyBytes = Encoding.UTF8.GetBytes(SecretKey);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                    string encryptedStr = Convert.ToBase64String(encryptedBytes);

                    if (isURLData)
                    {
                        encryptedStr = encryptedStr.Replace("/", "-").Replace("=", "_").Replace("+", ".");
                    }

                    return encryptedStr;
                }
            }
        }

        public static string DecryptData(string encryptedData, bool isURLData = true)
        {
            if (SecretKey.Length > 32)
            {
                SecretKey = SecretKey.Substring(0, 32);
            }
            else if (SecretKey.Length < 32)
            {
                SecretKey = SecretKey.PadRight(32, ' ');
            }
            byte[] keyBytes = Encoding.UTF8.GetBytes(SecretKey);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    // Ensure the Base-64 encoded string is valid
                    if (isURLData)
                    {
                        encryptedData = encryptedData.Replace("-", "/").Replace("_", "=").Replace(".", "+");
                    }
                    if ((encryptedData.Length % 4) != 0)
                    {
                        encryptedData = encryptedData.PadRight(encryptedData.Length + (4 - (encryptedData.Length % 4)), '=');
                    }

                    byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        public static string EncryptResponse(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                if (key.Length > 32)
                {
                    key = key.Substring(0, 32);
                }
                else if (key.Length < 32)
                {
                    key = key.PadRight(32, ' ');
                }
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);

                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        using (var writer = new StreamWriter(cs))
                        {
                            writer.Write(plainText);
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static string GenerateSecretKey(string value)
        {
            var keyMaterial = $"{ResponseSecretKey}{value}";
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ResponseSecretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(keyMaterial));
                return Convert.ToBase64String(hash);
            }
        }

        public static string GenerateShortHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder result = new StringBuilder(7);
                foreach (byte b in hashBytes)
                {
                    result.Append(ConvertToBase36(b));
                    if (result.Length >= 7)
                    {
                        break;
                    }
                }
                return result.ToString().ToUpper();
            }
        }

        private static char ConvertToBase36(byte b)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return chars[b % 36];
        }
    }
}
