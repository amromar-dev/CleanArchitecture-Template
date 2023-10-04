using System.Security.Cryptography;
using System.Text;

namespace CleanArchitectureTemplate.Application.Common.Encryptions
{
    public static class EncryptionHelper
    {
        private static string encryptionKeyString = "YjBY9pvprSngaqMoZ8q2wvJ2a7DmM9mZ";
        private static byte[] fixedIV = Encoding.UTF8.GetBytes("0123456789ABCDEF");

        public static string Encrypte(string originalData)
        {
            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(encryptionKeyString);
            aes.IV = fixedIV;

            // Create an encryptor to perform the encryption
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            // Convert the original data to bytes
            byte[] originalBytes = Encoding.UTF8.GetBytes(originalData);

            // Create a MemoryStream to hold the encrypted data
            using var encryptedStream = new System.IO.MemoryStream();
            // Create a CryptoStream to perform the encryption
            using var cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write);
            // Write the encrypted data to the CryptoStream
            cryptoStream.Write(originalBytes, 0, originalBytes.Length);
            cryptoStream.FlushFinalBlock();

            // Get the encrypted data as bytes
            byte[] encryptedBytes = encryptedStream.ToArray();

            // Convert the encrypted bytes to a Base64 string
            string encryptedData = Convert.ToBase64String(encryptedBytes);

            return encryptedData;
        }

        public static string Decrypte(string encryptedData)
        {
            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(encryptionKeyString);
            aes.IV = fixedIV;

            // Convert the encrypted data from Base64 to bytes
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            // Create a decryptor to perform the decryption
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            // Create a MemoryStream to hold the decrypted data
            using var decryptedStream = new System.IO.MemoryStream();
            // Create a CryptoStream to perform the decryption
            using var cryptoStream = new CryptoStream(decryptedStream, decryptor, CryptoStreamMode.Write);
            // Write the encrypted data to the CryptoStream
            cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            cryptoStream.FlushFinalBlock();

            // Get the decrypted data as bytes
            byte[] decryptedBytes = decryptedStream.ToArray();

            // Convert the decrypted bytes to a string
            string decryptedData = Encoding.UTF8.GetString(decryptedBytes);

            return decryptedData;
        }
    }
}
