using System;
using System.Security.Cryptography;
using System.Text;

namespace Katarai.Utils
{
    public class RSAPasswordCrypter
    {
        private readonly RSA _rsa;

        ///<summary>
        /// Constructor
        ///</summary>
        ///<param name="rsa"></param>
        public RSAPasswordCrypter(RSA rsa)
        {
            _rsa = rsa;
        }

        /// <summary>
        /// Returns the given string without carrying out any changes.
        /// </summary>
        /// <param name="value">The string to decrypt</param>
        /// <returns>Returns the unaltered string provided</returns>
        public string DecryptString(string value)
        {

            var provider = new RSACryptoServiceProvider();
            provider.FromXmlString(_rsa.ToXmlString(true));
            var passwordBytes = new byte[value.Length / 2];
            for (var i = 0; i < passwordBytes.Length; i++)
            {
                passwordBytes[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
            }
            byte[] decryptedBytes = provider.Decrypt(passwordBytes, false);
            return Encoding.ASCII.GetString(decryptedBytes);
        }

        /// <summary>
        /// Returns the given string without carrying out any changes.
        /// </summary>
        /// <param name="value">The string to encrypt</param>
        /// <returns>Returns the unaltered string provided</returns>
        public string EncryptString(string value)
        {
            var provider = new RSACryptoServiceProvider();
            provider.FromXmlString(_rsa.ToXmlString(false));
            var passwordBytes = Encoding.ASCII.GetBytes(value);
            var encryptedByes = provider.Encrypt(passwordBytes, false);
            var text = "";
            foreach (byte bye in encryptedByes)
            {
                text += string.Format("{0:x2}", bye);
            }
            return text;
        }
    }
}