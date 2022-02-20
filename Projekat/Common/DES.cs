using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DES
    {
        private static string keyLocation = "../../../SecretKey.txt";

        public static string KeyLocation { get => keyLocation; }

        public static byte[] EncryptFile(string tekst, string secretKey)
        {
            byte[] encrypted; // Ecnrypted data
            byte[] IV;        // Base vector for the first XOR iteration

            // Encrypting sent string
            using (DESCryptoServiceProvider des = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(secretKey); // SecretKey to bytes
                des.GenerateIV(); // Generates the base vector for the first XOR iteration
                IV = des.IV;

                des.Mode = CipherMode.CBC; // Enables CBC mode for DES

                var encryptor = des.CreateEncryptor(des.Key, des.IV); // Generates the encryptor

                // Stream used for encryption, mem is altered by crypto by encryptor value
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(tekst);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Makes a combined array of encrypted data + IV base vector for first XOR iteraton
            // Data has to have its IV vector in front as it is later used for decrypting
            var combinedIvCt = new byte[IV.Length + encrypted.Length];
            Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
            Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);

            return combinedIvCt;
        }

        public static string DecryptFile(byte[] inputData, string secretKey)
        {
            string tekst = string.Empty;

            // Encrypting sent string
            using (DESCryptoServiceProvider des = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create())
            {
                des.Key = Encoding.ASCII.GetBytes(secretKey); // SecretKey to bytes
                byte[] IV = new byte[des.BlockSize / 8]; // An array for storing the base IV vector used in the first XOR iteration
                byte[] cipherText = new byte[inputData.Length - IV.Length]; // An array for storing the data itself

                Array.Copy(inputData, IV, IV.Length); // 16bytes/128bits, thats where we stored our IV base vector for first XOR iteraton
                Array.Copy(inputData, IV.Length, cipherText, 0, cipherText.Length); // Taking the rest of bits which represent the data itself

                des.IV = IV;
                des.Mode = CipherMode.CBC; // Enables CBC mode for DES

                ICryptoTransform decryptor = des.CreateDecryptor(des.Key, des.IV); // Generates the decryptor

                // Stream used for decryption, mem is altered by crypto by decryptor value
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            tekst = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return tekst;
            }
        }
    }
}
