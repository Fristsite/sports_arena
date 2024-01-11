using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;



namespace Play.Controllers
{
#nullable disable
    public class DESEncrption
    {
        public string Encrypt(string plainText, string key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);

                // Convert the key string to bytes with a valid size (8 bytes for DES)
                byte[] keyBytes = new byte[8];
                byte[] keyData = Encoding.UTF8.GetBytes(key);
                Array.Copy(keyData, keyBytes, Math.Min(keyData.Length, keyBytes.Length));

                des.Key = keyBytes;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    byte[] encryptedBytes = ms.ToArray();
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }


        public string Decrypt(string encryptedText, string key)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);

                des.Key = keyBytes;
                des.Mode = CipherMode.ECB; // Use ECB mode (must match the encryption mode)
                des.Padding = PaddingMode.PKCS7;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                    cs.FlushFinalBlock();
                    byte[] decryptedBytes = ms.ToArray();
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

    }
}