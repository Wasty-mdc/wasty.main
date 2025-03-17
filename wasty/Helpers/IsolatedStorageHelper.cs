using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace wasty.Helpers
{
    public class IsolatedStorageHelper
    {
        private readonly byte[] _encryptionKey = Encoding.UTF8.GetBytes("XyZ1aBcDeFgHiJkLmNoPqRsTuVwXyZ1a");

        public void SaveData(string fileName, string data)
        {
            var encryptedData = EncryptData(data);

            using (         var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Create, isolatedStorage))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(encryptedData);
            }
        }

        public string LoadData(string fileName)
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Open, isolatedStorage))
            using (var reader = new StreamReader(stream))
            {
                var encryptedData = reader.ReadToEnd();
                return DecryptData(encryptedData);
            }
        }

        private string EncryptData(string data)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                aes.GenerateIV();
                var iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                using (var ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(data);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string DecryptData(string encryptedData)
        {
            var fullCipher = Convert.FromBase64String(encryptedData);

            using (var aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                var iv = new byte[aes.BlockSize / 8];
                var cipher = new byte[fullCipher.Length - iv.Length];

                Array.Copy(fullCipher, iv, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                using (var ms = new MemoryStream(cipher))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
