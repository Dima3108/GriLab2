using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLibrary
{
    public class CryptoDecoder
    {
        ICryptoTransform encryptor;
        private byte[] key;
        public CryptoDecoder(byte[] key_)
        {
            //aes = Aes.Create();
            key = new byte[key_.Length];
            key_.CopyTo(key, 0);

        }
        public byte[] EncryptBlock(byte[] block)
        {
            //encryptor = Aes.Create().CreateDecryptor(key, new byte[12]);
            //return encryptor.TransformFinalBlock(block, 0, block.Length);
            return AES.AES_Decrypt(block, key);
        }
    }
}
