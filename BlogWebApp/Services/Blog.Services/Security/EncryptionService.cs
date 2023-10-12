﻿// <copyright file="EncryptionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.EntityServices.Security;

using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Encryption service.
/// </summary>
public class EncryptionService : IEncryptionService
{
    /// <summary>
    /// Create salt key.
    /// </summary>
    /// <param name="size">Key size.</param>
    /// <returns>Salt key.</returns>
    public virtual string CreateSaltKey(int size)
    {
        // generate a cryptographic random number
        using (var provider = new RNGCryptoServiceProvider())
        {
            var buff = new byte[size];
            provider.GetBytes(buff);

            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(buff);
        }
    }

    /// <summary>
    /// Create a password hash.
    /// </summary>
    /// <param name="password">Password.</param>
    /// <param name="saltkey">Salk key.</param>
    /// <param name="passwordFormat">Password format (hash algorithm).</param>
    /// <returns>Password hash</returns>
    public virtual string CreatePasswordHash(string password, string saltkey, string passwordFormat)
        => this.CreateHash(Encoding.UTF8.GetBytes(string.Concat(password, saltkey)), passwordFormat);

    /// <summary>
    /// Create a data hash.
    /// </summary>
    /// <param name="data">The data for calculating the hash.</param>
    /// <param name="hashAlgorithm">Hash algorithm.</param>
    /// <returns>Data hash</returns>
    public virtual string CreateHash(byte[] data, string hashAlgorithm)
    {
        if (string.IsNullOrEmpty(hashAlgorithm))
        {
            throw new ArgumentNullException(nameof(hashAlgorithm));
        }

        var algorithm = (HashAlgorithm)CryptoConfig.CreateFromName(hashAlgorithm)
                        ?? throw new ArgumentException("Unrecognized hash name");
        var hashByteArray = algorithm.ComputeHash(data);

        return BitConverter.ToString(hashByteArray).Replace("-", string.Empty);
    }

    /// <summary>
    /// Encrypt text.
    /// </summary>
    /// <param name="plainText">Text to encrypt.</param>
    /// <param name="encryptionPrivateKey">Encryption private key.</param>
    /// <returns>
    /// Encrypted text.
    /// </returns>
    /// <exception cref="NotImplementedException">NotImplementedException.</exception>
    public string EncryptText(string plainText, string encryptionPrivateKey = "")
        => throw new NotImplementedException();

    /// <summary>
    /// Decrypt text.
    /// </summary>
    /// <param name="cipherText">Text to decrypt.</param>
    /// <param name="encryptionPrivateKey">Encryption private key.</param>
    /// <returns>
    /// Decrypted text.
    /// </returns>
    /// <exception cref="NotImplementedException">NotImplementedException.</exception>
    public string DecryptText(string cipherText, string encryptionPrivateKey = "")
        => throw new NotImplementedException();

    // public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
    // {
    //    if (string.IsNullOrEmpty(plainText))
    //        return plainText;
    //    if (string.IsNullOrEmpty(encryptionPrivateKey))
    //        encryptionPrivateKey = _securitySettings.EncryptionKey;
    //    using (var provider = new TripleDESCryptoServiceProvider())
    //    {
    //        provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
    //        provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));
    //        var encryptedBinary = EncryptTextToMemory(plainText, provider.Key, provider.IV);
    //        return Convert.ToBase64String(encryptedBinary);
    //    }
    // }

    // public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
    // {
    //    if (string.IsNullOrEmpty(cipherText))
    //        return cipherText;
    //    if (string.IsNullOrEmpty(encryptionPrivateKey))
    //        encryptionPrivateKey = _securitySettings.EncryptionKey;
    //    using (var provider = new TripleDESCryptoServiceProvider())
    //    {
    //        provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
    //        provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));
    //        var buffer = Convert.FromBase64String(cipherText);
    //        return DecryptTextFromMemory(buffer, provider.Key, provider.IV);
    //    }
    // }
    // private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
    // {
    //    using var ms = new MemoryStream();
    //    using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
    //    {
    //        var toEncrypt = Encoding.Unicode.GetBytes(data);
    //        cs.Write(toEncrypt, 0, toEncrypt.Length);
    //        cs.FlushFinalBlock();
    //    }
    //    return ms.ToArray();
    // }
    // private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
    // {
    //    using var ms = new MemoryStream(data);
    //    using var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read);
    //    using var sr = new StreamReader(cs, Encoding.Unicode);
    //    return sr.ReadToEnd();
    // }
}