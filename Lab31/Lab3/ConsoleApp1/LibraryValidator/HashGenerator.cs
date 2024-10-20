using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryValidator
{
    public class HashGenerator
    {
        public string Hash_ { get;private set; }
        public HashGenerator(string v1,string v2)
        {
            Hash_ = UnsafeMethods.ComputeHash(v1, v2);
        }
        public string HashBlock(byte[] data)
        {
            return UnsafeMethods.ComputeHash(data,UnicodeEncoding.Unicode.GetBytes(Hash_));
        }
    }
}
