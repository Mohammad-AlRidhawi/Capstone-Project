/**
* \class Encryption.cs
* \brief A class that represents the encryption and decryption implementation.
* \author Johnathan Falbo
* \date 16/04/2015
*/
using System;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace AntiCorruptionSeachEngine.admin
{
    public class Encryption
    {
        //AES 256 bit key
        private static byte[] key = { 217, 140, 44, 231, 160, 206, 148, 202, 241, 54, 131, 233, 198, 23, 134, 253, 106, 200, 198, 145, 42, 168, 128, 156, 164, 218, 201, 254, 246, 218, 90, 68 };
        //AES 128 bit offset key
        private static byte[] vector = { 109, 8, 227, 218, 181, 177, 101, 207, 247, 66, 195, 11, 223, 222, 222, 45 };

        /**
* Name:         public static string GetSHA256Hash(string inputString)  
* Description:  Called to get the sha256 hash of a string.
* Arguments:    inputString: input string to hash.
*               e:      Any events being sent. Not currently used.
* Return:       String hashed input string.      
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        public static string GetSHA256Hash(string inputString)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] passwordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < passwordBytes.Length; i++)
            {
                stringBuilder.Append(passwordBytes[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        /**
* Name:         public static string Encrypt(string unencryptedInput)  
* Description:  Called returns encrypted version of input string.
* Arguments:    unencryptedInput: inputed string.
* Return:       string encrypted version of input string.      
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        public static string Encrypt(string unencryptedInput)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencryptedInput)));
        }

        /**
* Name:         public static string Decrypt(string encryptedInput)  
* Description:  Called to decrypt the inputted spring.
* Arguments:    encryptedInput: inputed string to decrypt.
* Return:       decrypted input string.      
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        public static string Decrypt(string encryptedInput)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetString(Decrypt(Convert.FromBase64String(encryptedInput)));
        }

        private static byte[] Encrypt(byte[] buffer)
        {
            AesManaged aes = new AesManaged();
            ICryptoTransform encryptor = aes.CreateEncryptor(key, vector);
            return Transform(buffer, encryptor);
        }

        private static byte[] Decrypt(byte[] buffer)
        {
            AesManaged aes = new AesManaged();
            ICryptoTransform decryptor = aes.CreateDecryptor(key, vector); 
            return Transform(buffer, decryptor);
        }

        private static byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}