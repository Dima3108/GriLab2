using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hardware;
using LibraryValidator;
using System.Security.Cryptography;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string hash = UnsafeMethods.ComputeHash("hellow", "world!");
            Console.WriteLine(ValuesValidator.ValidHash("hellow", "world!", hash));
            return;
            //System.Environment.
            var hardinfo = new Hardware.Info.HardwareInfo();
            Console.WriteLine(hardinfo.ToString());
            hardinfo.RefreshCPUList();
            foreach(var i in hardinfo.CpuList)
            {
                Console.WriteLine(i.ToString());
            }
            hardinfo.RefreshDriveList();
            foreach(var d in hardinfo.DriveList)
            {
                Console.WriteLine(d.ToString());
                Console.WriteLine($"sn:{d.SerialNumber}");
               
            }
            hardinfo.RefreshNetworkAdapterList();
            Console.WriteLine("Net:\n\n\n");
            foreach(var n in hardinfo.NetworkAdapterList)
            {
                Console.WriteLine(n.ToString());
            }
            Console.WriteLine("\n\n\n\n");
            hardinfo.RefreshComputerSystemList();
            foreach(var cs in hardinfo.ComputerSystemList)
            {
                Console.WriteLine(cs.ToString());
            }
            Console.WriteLine("\n\n\n\n");
            hardinfo.RefreshMotherboardList();
            foreach(var moth in hardinfo.MonitorList)
            {
                Console.WriteLine(moth.ToString());
            }
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("\nMonitors:\n");
            //hardinfo.RefreshMonitorList();
            /*foreach(var monit in hardinfo.MonitorList)
            {
                Console.WriteLine(monit.ToString());
            }*/
            Console.WriteLine("GPU:\n");
            hardinfo.RefreshVideoControllerList();
            foreach(var gpu in hardinfo.VideoControllerList)
            {
                Console.WriteLine(gpu.ToString());
            }
            hardinfo.RefreshMemoryList();
            Console.WriteLine("\nmemory\n");
            foreach(var mem in hardinfo.MemoryList)
            {
                Console.WriteLine(mem.ToString());
                
            }
            hardinfo.RefreshMemoryStatus();
            Console.WriteLine("\nmemstat\n");
            Console.WriteLine(hardinfo.MemoryStatus.ToString());
            foreach (var drive_ in System.IO.DriveInfo.GetDrives())
            {
                Console.WriteLine(drive_.ToString())
;
                Console.WriteLine(drive_.Name);
                //Console.WriteLine(drive_.VolumeLabel);
                //Console.WriteLine(drive_.RootDirectory.ToString());
            }
        }
        static string GetMacAddr()
        {
            var hardinfo = new Hardware.Info.HardwareInfo();
            hardinfo.RefreshNetworkAdapterList();
            return hardinfo.NetworkAdapterList[0].MACAddress;
        }
        static string GetHardSerialNumber()
        {
            var hardinfo = new Hardware.Info.HardwareInfo();
            hardinfo.RefreshDriveList();
            return hardinfo.DriveList[0].SerialNumber;
        }
    }
}
