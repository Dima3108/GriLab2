using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryValidator
{
    public class ValuesValidator
    {
        /*private string V1 {  get; set; }    
        private string V2 { get; set; }*/
        private string V1V2_HASH {  get; set; } 
        public ValuesValidator(string v1,string v2) 
        {
            V1V2_HASH = UnsafeMethods.ComputeHash(v1, v2);
        }
        public bool ValidBlock(byte[] data,string hashdata)
        {
            return UnsafeMethods.ComputeHash(data, UnicodeEncoding.Unicode.GetBytes(V1V2_HASH)) == hashdata;
        }
        public bool ValidHash(string hash_)
        {
#if DEBUG
            Console.WriteLine($"{hash_}::{V1V2_HASH}");
#endif
            return hash_ == V1V2_HASH;
        }
        public static bool ValidHash(string v1, string v2,string hash)
        {
            return UnsafeMethods.ComputeHash(v1,v2)==hash;
        }
    }
}
