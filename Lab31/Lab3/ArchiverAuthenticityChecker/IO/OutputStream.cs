using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CryptoLibrary;
using LibraryValidator;
namespace ArchiverAuthenticityChecker.IO
{
    public class OutputStream:Stream
    {
        private byte[] cesh;
        private int pos;
        private Stream oStream;
        private CryptoEncoder cryptoEncoder;
        private HashGenerator hashGenerator;
        public OutputStream(Stream oStream,CryptoEncoder cryptoEncoder,HashGenerator hashGenerator,int block_size=1024)
        {
            this.cryptoEncoder = cryptoEncoder;
            this.oStream = oStream;
            this.hashGenerator = hashGenerator;
            this.pos = 0;
            cesh=new byte[block_size];
            byte[] _h = UnicodeEncoding.Unicode.GetBytes(hashGenerator.Hash_);
            WriteUintToStream((uint)_h.Length);
            oStream.Write(_h, 0, _h.Length);
        }
        private void WriteUshortToStream(ushort val)
        {
            oStream.WriteByte((byte)(val/256));
            oStream.WriteByte((byte)(val % 256));
        }
        private void WriteUintToStream(uint val)
        {
            WriteUshortToStream((ushort)(val/(256*256)));
            WriteUshortToStream((ushort)(val % (256 * 256)));
        }
        private void Encode()
        {
            if (pos > 0)
            {
                if (pos == cesh.Length)
                {
                    string hash_ = hashGenerator.HashBlock(cesh);
                    byte[] encr = cryptoEncoder.EncryptBlock(cesh);
                    byte[] hval_ = UnicodeEncoding.Unicode.GetBytes(hash_);
                    WriteUintToStream((uint)hval_.Length);
                    oStream.Write(hval_, 0, hval_.Length);
                    WriteUintToStream((uint)encr.Length);
                    oStream.Write(encr, 0, encr.Length);
                }
                else
                {
                    byte[] nr = new byte[pos];
                    for(int i=0;i<nr.Length; i++)
                    {
                        nr[i] = cesh[i];
                    }
                    string hash_ = hashGenerator.HashBlock(nr);
                    byte[] encr = cryptoEncoder.EncryptBlock(nr);
                    byte[] hval_ =UnicodeEncoding.Unicode.GetBytes(hash_);
                    WriteUintToStream((uint)hval_.Length);
                    oStream.Write(hval_, 0, hval_.Length);
                    WriteUintToStream((uint)encr.Length);
                    oStream.Write(encr, 0, encr.Length);
                }
            }
            pos = 0;
        }
        public override void WriteByte(byte value)
        {
            cesh[pos++] = value;
            if (pos == cesh.Length)
            {
                Encode();
            }
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            for(int i=0;i< count;i++)
                WriteByte(buffer[i+offset]);
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        public override void Flush()
        {
            if (pos > 0)
                Encode();
            oStream.Flush();
        }
        public override bool CanSeek => oStream.CanSeek;
        public override bool CanRead => false;
        public override bool CanWrite => oStream.CanWrite;
        public override long Length => oStream.Length + pos;
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }
        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }
    }
}
