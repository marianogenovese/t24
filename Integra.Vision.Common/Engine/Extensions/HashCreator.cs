//-----------------------------------------------------------------------
// <copyright file="HashCreator.cs" company="Ingetra.Vision.Common">
//     Copyright (c) Ingetra.Vision.Common. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Extensions
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Extension for create hash codes from a string
    /// </summary>
    internal static class HashCreator
    {
        /// <summary>
        /// Get hash code from password
        /// </summary>
        /// <param name="md5Hash">Doc1 goes here</param>
        /// <param name="input">Doc2 goes here</param>
        /// <returns>Doc3 goes here</returns>
        public static string GetMd5Hash(this MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder stringBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return stringBuilder.ToString();
        }
    }
}
