/*
==============================================================================
Copyright © Jason Drawdy 

All rights reserved.

The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

Except as contained in this notice, the name of the above copyright holder
shall not be used in advertising or otherwise to promote the sale, use or
other dealings in this Software without prior written authorization.
==============================================================================
*/

#region Imports

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

#endregion
namespace MemoryMapper
{
    /// <summary>
    /// Provides access to common methods found in security software.
    /// </summary>
    public static class Cryptography
    {
        #region Hashing

        /// <summary>
        /// Provides access to common cryptographic hashing algorithms.
        /// </summary>
        public class Hashing
        {
            /// <summary>
            /// Collection of common hashing algorithms.
            /// </summary>
            public enum HashType
            {
                MD5,
                RIPEMD160,
                SHA1,
                SHA256,
                SHA384,
                SHA512,
            }

            /// <summary>
            /// Transforms a string into a cryptographically strong hash.
            /// </summary>
            /// <param name="input">The string to be transformed.</param>
            /// <param name="hash">The algorithm to used for hashing.</param>
            private byte[] GetHash(string input, HashType hash)
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);

                switch (hash)
                {
                    case HashType.MD5:
                        return MD5.Create().ComputeHash(inputBytes);
                    case HashType.RIPEMD160:
                        return RIPEMD160.Create().ComputeHash(inputBytes);
                    case HashType.SHA1:
                        return SHA1.Create().ComputeHash(inputBytes);
                    case HashType.SHA256:
                        return SHA256.Create().ComputeHash(inputBytes);
                    case HashType.SHA384:
                        return SHA384.Create().ComputeHash(inputBytes);
                    case HashType.SHA512:
                        return SHA512.Create().ComputeHash(inputBytes);
                    default:
                        return inputBytes;
                }

            }

            /// <summary>
            /// Transforms a file's bytes into an array of cryptographically strong data.
            /// </summary>
            /// <param name="path">The location of the file to hash.</param>
            /// <param name="hash">The algorithm to be used for hashing.</param>
            private byte[] GetFileHash(string path, HashType hash)
            {
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, (1024 * 1024)))
                {
                    switch (hash)
                    {
                        case HashType.MD5:
                            return MD5.Create().ComputeHash(file);
                        case HashType.RIPEMD160:
                            return RIPEMD160.Create().ComputeHash(file);
                        case HashType.SHA1:
                            return SHA1.Create().ComputeHash(file);
                        case HashType.SHA256:
                            return SHA256.Create().ComputeHash(file);
                        case HashType.SHA384:
                            return SHA384.Create().ComputeHash(file);
                        case HashType.SHA512:
                            return SHA512.Create().ComputeHash(file);
                        default:
                            return null;
                    }
                }
            }

            /// <summary>
            /// Transforms a message into a cryptographically strong hash.
            /// </summary>
            /// <param name="input">The message to be hashed.</param>
            /// <param name="selection">The algorithm to be used for hashing.</param>
            /// <returns></returns>
            public string ComputeMessageHash(string input, HashType selection)
            {
                try
                {
                    byte[] hash = GetHash(input, selection);
                    StringBuilder result = new StringBuilder();

                    for (int i = 0; i <= hash.Length - 1; i++)
                    {
                        result.Append(hash[i].ToString("x2"));
                    }
                    return result.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// Transforms a file's bytes into a cryptographically strong hash.
            /// </summary>
            /// <param name="path">The location of the file to hash.</param>
            /// <param name="hash">The algorithm to be used for hashing.</param>
            public string ComputeFileHash(string input, HashType selection)
            {
                try
                {
                    byte[] hash = GetFileHash(input, selection);
                    StringBuilder result = new StringBuilder();

                    for (int i = 0; i <= hash.Length - 1; i++)
                    {
                        result.Append(hash[i].ToString("x2"));
                    }

                    return result.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        #endregion
        #region Other

        /// <summary>
        /// Provides access to homebrew encryption algorithms.
        /// </summary>
        public class Other
        {
            private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
            {
                byte[] encryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }

                return encryptedBytes;
            }

            private byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
            {
                byte[] decryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }
            /// <summary>
            /// Transforms plaintext into cryptographically strong ciphertext using AES.
            /// </summary>
            /// <param name="input">The plaintext to transform.</param>
            /// <param name="password">The password to use during the cryptographic transformation.</param>
            public string EncryptText(string input, string password)
            {
                // Get the bytes of the string
                byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                string result = Convert.ToBase64String(bytesEncrypted);

                return result;
            }
            /// <summary>
            /// Transforms ciphertext into its original plaintext form using AES.
            /// </summary>
            /// <param name="input">The ciphertext to be transformed.</param>
            /// <param name="password">The password to use during the transformation.</param>
            public string DecryptText(string input, string password)
            {
                // Get the bytes of the string
                byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                string result = Encoding.UTF8.GetString(bytesDecrypted);

                return result;
            }
            /// <summary>
            /// Encrypts a file into its cryptographically strong equivalent using AES.
            /// </summary>
            /// <param name="originalFile">The location of the original file to encrypt.</param>
            /// <param name="destinationFile">The location the encrypted file should be saved to.</param>
            /// <param name="passKey">The password to use for encrypting.</param>
            public void EncryptFile(string originalFile, string destinationFile, string passKey)
            {
                string file = originalFile;
                string password = passKey;

                byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                string fileEncrypted = destinationFile;

                File.WriteAllBytes(fileEncrypted, bytesEncrypted);
            }
            /// <summary>
            /// Decrypts a file into its original plainbyte form using AES.
            /// </summary>
            /// <param name="originalFile">The location of the original file to decrypt.</param>
            /// <param name="destinationFile">The location the decrypted file should be saved to.</param>
            /// <param name="passKey">The password to use for decrypting.</param>
            public void DecryptFile(string originalFile, string destinationFile, string passKey)
            {
                string fileEncrypted = originalFile;
                string password = passKey;

                byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                string file = destinationFile;
                File.WriteAllBytes(file, bytesDecrypted);
            }
            /// <summary>
            /// Encrypts plaintext into cryptographically strong ciphertext using AES.
            /// </summary>
            /// <param name="text">The plaintext to encrypt.</param>
            /// <param name="pwd">The password to use for encryption.</param>
            /// <returns></returns>
            public string Encrypt(string text, string pwd)
            {
                byte[] originalBytes = Encoding.UTF8.GetBytes(text);
                byte[] encryptedBytes = null;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                // Generating salt bytes
                byte[] saltBytes = new SecureRandom().GetBytes(8);//GetRandomBytes();

                // Appending salt bytes to original bytes
                byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
                for (int i = 0; i < saltBytes.Length; i++)
                    bytesToBeEncrypted[i] = saltBytes[i];

                for (int i = 0; i < originalBytes.Length; i++)
                    bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];

                encryptedBytes = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                return Convert.ToBase64String(encryptedBytes);
            }
            /// <summary>
            /// Decrypts ciphertext into plaintext using AES.
            /// </summary>
            /// <param name="decryptedText">The ciphertext to decrypt.</param>
            /// <param name="pwd">The password to use for decryption.</param>
            /// <returns></returns>
            public string Decrypt(string decryptedText, string pwd)
            {
                byte[] bytesToBeDecrypted = Convert.FromBase64String(decryptedText);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(pwd);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                // Getting the size of salt
                int _saltSize = 4;

                // Removing salt bytes, retrieving original bytes
                byte[] originalBytes = new byte[decryptedBytes.Length - _saltSize];
                for (int i = _saltSize; i < decryptedBytes.Length; i++)
                {
                    originalBytes[i - _saltSize] = decryptedBytes[i];
                }

                return Encoding.UTF8.GetString(originalBytes);
            }

            //private byte[] GetRandomBytes()
            //{
            //    int _saltSize = 4;
            //    byte[] ba = new byte[_saltSize];
            //    RNGCryptoServiceProvider.Create().GetBytes(ba);
            //    return ba;
            //}
        }

        #endregion
    }
}
