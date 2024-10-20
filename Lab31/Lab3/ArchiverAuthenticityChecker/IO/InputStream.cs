using CryptoLibrary;
using Hardware.Info;
using LibraryValidator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArchiverAuthenticityChecker.IO
{
    public class InputStream : Stream
    {
        private Stream iStream;
        private int pos;
        private byte[] buffer;
        private CryptoDecoder cryptoDecoder;
        private ValuesValidator valuesValidator;
        public InputStream(Stream iStream, CryptoDecoder cryptoDecoder, ValuesValidator valuesValidator)
        {
            this.iStream = iStream;
            this.cryptoDecoder = cryptoDecoder;
            this.valuesValidator = valuesValidator;
            /*
             * byte[] _h = ASCIIEncoding.ASCII.GetBytes(hashGenerator.Hash_);
               WriteUintToStream((uint)_h.Length);
               Write(_h, 0, _h.Length);
             */
            int hash_len = (int)ReadUIntInStream();
            byte[] h_ = new byte[hash_len];
            int l = iStream.Read(h_, 0, h_.Length);


            string hash_ = UnicodeEncoding.Unicode.GetString(h_, 0, h_.Length);
            if (!valuesValidator.ValidHash(hash_))
                throw new Exceptions.ComputerMismatchException();
            buffer = new byte[0];
        }
        /*
         * private void WriteUshortToStream(ushort val)
        {
            oStream.WriteByte((byte)(val/256));
            oStream.WriteByte((byte)(val % 256));
        }
        private void WriteUintToStream(uint val)
        {
            WriteUshortToStream((ushort)(val/(256*256)));
            WriteUshortToStream((ushort)(val % (256 * 256)));
        }
         */
        private ushort ReadUshortInStream()
        {
            return (ushort)((ushort)iStream.ReadByte() * 256 + (ushort)iStream.ReadByte());
        }
        private uint ReadUIntInStream() => (uint)(ReadUshortInStream() * 256 * 256 + ReadUshortInStream());
        private void Decode()
        {
            if (iStream.CanRead)
            {
                /*
                 *  byte[] nr = new byte[pos];
                    for(int i=0;i<nr.Length; i++)
                    {
                        nr[i] = cesh[i];
                    }
                    string hash_ = hashGenerator.HashBlock(nr);
                    byte[] encr = cryptoEncoder.EncryptBlock(nr);
                    byte[] hval_ = ASCIIEncoding.ASCII.GetBytes(hash_);
                    WriteUintToStream((uint)hval_.Length);
                    oStream.Write(hval_, 0, hval_.Length);
                    WriteUintToStream((uint)encr.Length);
                    oStream.Write(encr, 0, encr.Length);
                 */
                int hval_ = (int)ReadUIntInStream(); 
                byte[] hash_ = new byte[hval_];
                iStream.Read(hash_, 0, hval_);
                string _hash_ = UnicodeEncoding.Unicode.GetString(hash_);
                int arval=(int)ReadUIntInStream();
                byte[] encar = new byte[arval];
                iStream.Read(encar, 0, encar.Length);
                buffer=cryptoDecoder.EncryptBlock(encar);
                if (!valuesValidator.ValidBlock(buffer, _hash_))
                    throw new Exceptions.CorruptedSourceData();
                pos = 0;
            }
            else
            {
                
            }
        }
        public override int ReadByte()
        {
            if (pos < buffer.Length)
            {
                return buffer[pos++];
            }
            else
            {
                Decode();
                if (pos != 0)
                    return -1;
                return ReadByte();
            }
            // return base.ReadByte();
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                int val_ = ReadByte();
                if (val_ >= 0)
                {
                    buffer[i + offset] = (byte)val_;
                }
                else
                {
                    return i;
                }
            }
            return count;
        }
        public override bool CanSeek => iStream.CanSeek;
        public override bool CanRead => iStream.CanRead;
        public override bool CanWrite => false;
        public override long Length => iStream.Length;
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }
        public override void Flush()
        {
            iStream.Flush();
        }
    }
}
