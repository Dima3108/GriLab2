using System;
using System.Security.Cryptography;
namespace CryptoLibrary
{
    public class CryptoEncoder
    {
        ICryptoTransform encryptor;
        private byte[] key;
        public CryptoEncoder(byte[]key_)
        {
            //aes = Aes.Create();
           key=new byte[key_.Length];
            key_.CopyTo(key, 0);
           
        }
        public byte[] EncryptBlock(byte[] block)
        {
            /*encryptor = Aes.Create().CreateEncryptor(key, new byte[12]);
           return encryptor.TransformFinalBlock(block,0,block.Length);*/
            return AES.AES_Encrypt(block, key);
        }
    }
}
