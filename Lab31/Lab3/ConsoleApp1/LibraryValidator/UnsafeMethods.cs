using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LibraryValidator
{
    public class UnsafeMethods
    {
        private const string path = "Q:/github_repo/infosec/Lab3/ConsoleApp1/x64/Debug";
        [DllImport(dllName: path+"/CLIB.dll",CharSet =CharSet.Ansi,CallingConvention =CallingConvention.ThisCall)]
        private static extern unsafe byte* ComputeHash(byte* buffer1, byte* buffer2, int bufLen, int buf2Len, [Out] int* outLen);
        [DllImport(path+"/CLIB.dll")]
        private static extern unsafe void DeleteResource(byte* buffer);
        private static int Len  = 0;
        public static string ComputeHash(byte[] buf1, byte[] buf2)
        {
            unsafe
            {
                fixed (byte* b1 = buf1, b2 = buf2)
                {

                    //int L = 0;
                    fixed(int*oLen=&Len)                    
                    {
                        byte* c = ComputeHash(b1, b2, buf1.Length, buf2.Length, oLen);
                        string s = "";
                        for (int i = 0; i < (*oLen); i++)
                        {
                            char v_ = (char)(c[i]);
                            s += v_;
                        }
                        DeleteResource(c);
                        return s;
                    }
                }
            }
        }
        public static string ComputeHash(string v1, string v2) => ComputeHash(UnicodeEncoding.Unicode.GetBytes(v1),
            UnicodeEncoding.Unicode.GetBytes(v2));
    }
}
