using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WCNApi.Globals
{
    public static class Utils
    {
        /// <summary>
        /// Encrypt Password
        /// </summary>
        /// <param name="pstrText"></param>
        /// <returns></returns>
        public static string EncryptPassword(string pstrText)
        {
            string pstrEncrKey = "1239;[pewGKG)NisarFidesTech";
            byte[] byKey = System.Text.Encoding.UTF8.GetBytes(pstrEncrKey.Substring(0, 8));
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };

            using (DES des = DES.Create()) // <-- Use factory method
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pstrText);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypt Password
        /// </summary>
        /// <param name="pstrText"></param>
        /// <returns></returns>
        public static string DecryptPassword(string pstrText)
        {
            pstrText = pstrText.Replace(" ", "+");
            string pstrDecrKey = "1239;[pewGKG)NisarFidesTech";
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] byKey = System.Text.Encoding.UTF8.GetBytes(pstrDecrKey.Substring(0, 8));

            byte[] inputByteArray = Convert.FromBase64String(pstrText);

            using (DES des = DES.Create())
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write))
            {
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
        }


        public static string ConvertToTitleCase(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                data = textInfo.ToTitleCase(data);
            }
            return data;
        }

        public static bool ContainsHtml(string input)
        {
            Regex htmlRegex = new Regex(@"<[^>]+>");
            return htmlRegex.IsMatch(input);
        }

        public static string GetFileExtension(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            return Path.GetExtension(file.FileName).ToLower();
        }

        public static bool IsValidEmail(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(input);
        }

        public static bool IsValidUri(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            Regex uriRegex = new Regex(@"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$");
            return uriRegex.IsMatch(input);
        }

        public static string RemoveHtmlContent(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            string plainText = Regex.Replace(input, "<.*?>", string.Empty);
            plainText = WebUtility.HtmlDecode(plainText);
            plainText = Regex.Replace(plainText, @"\s+", " ").Trim();

            return plainText;
        }
    }
}
