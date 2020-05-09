namespace DosCommand
{
    #region

    using Microsoft.VisualBasic.Devices;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using JVersion;
    using System.Windows.Forms;

    #endregion

    public class SystemTools
    {
        public static int VerLength;
        /// <summary>
        ///     从注册表中查找可能的javaw.exe位置
        /// </summary>
        /// <returns>JAVA地址列表</returns>
        public static string GetJavaPath()
        {
            string Backinfo =  FindJava().ToString();
            return Backinfo;
        }
        public static IEnumerable<string> FindJava()
        {
            try
            {
                var rootReg = Registry.LocalMachine.OpenSubKey("SOFTWARE");
                return rootReg == null
                    ? new string[0]
                    : FindJavaInternal(rootReg).Union(FindJavaInternal(rootReg.OpenSubKey("Wow6432Node")));
            }
            catch
            {
                return new string[0];
            }
        }

        public static IEnumerable<string> FindJavaInternal(RegistryKey registry)
        {
            try
            {
                var registryKey = registry.OpenSubKey("JavaSoft");
                if ((registryKey == null) || ((registry = registryKey.OpenSubKey("Java Runtime Environment")) == null)) return new string[0];
                return (from ver in registry.GetSubKeyNames()
                        select registry.OpenSubKey(ver)
                    into command
                        where command != null
                        select command.GetValue("JavaHome")
                    into javaHomes
                        where javaHomes != null
                        select javaHomes.ToString()
                    into str
                        where !String.IsNullOrWhiteSpace(str)
                        select str + @"\bin\javaw.exe");
            }
            catch
            {
                return new string[0];
            }
        }

        /// <summary>
        ///     取物理内存
        /// </summary>
        /// <returns>物理内存</returns>
        public static ulong GetTotalMemory()
        {
            return new Computer().Info.TotalPhysicalMemory;
        }

        /// <summary>
        ///     获取x86 or x64
        /// </summary>
        /// <returns>32 or 64</returns>
        public static string GetArch()
        {
            return Environment.Is64BitOperatingSystem ? "64" : "32";
        }

        /// <summary>
        /// 获取系统剩余内存
        /// </summary>
        /// <returns>剩余内存</returns>
        public static ulong GetRunmemory()
        {
            ComputerInfo ComputerMemory = new Microsoft.VisualBasic.Devices.ComputerInfo();
            return ComputerMemory.AvailablePhysicalMemory / 1048576;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetProcessWorkingSetSize(IntPtr hProcess, IntPtr dwMinimumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);

        /// <summary>
        ///     清理内存
        /// </summary>
        public void ClearRAM()
        {
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, GetIntPtrFromInt(-1), GetIntPtrFromInt(-1));
        }

        private static IntPtr GetIntPtrFromInt(int i)
        {
            IntPtr ptr2;
            if (GetArch() == "64")
            {
                ptr2 = Marshal.AllocHGlobal(8);
            }
            else
            {
                ptr2 = Marshal.AllocHGlobal(4);
            }
            Marshal.WriteInt32(ptr2, i);
            return ptr2;
        }
        public static Guid GetPlayerUuid(string playerName)
        {
            MD5CryptoServiceProvider md5csp = new MD5CryptoServiceProvider();
            byte[] uuidBytes = md5csp.ComputeHash(System.Text.Encoding.UTF8.GetBytes("OfflinePlayer:" + playerName));
            uuidBytes[6] &= 0x0f;
            uuidBytes[6] |= 0x30;
            uuidBytes[8] &= 0x3f;
            uuidBytes[8] |= 0x80;
            Guid uuid = new Guid(toUuidString(uuidBytes));
            return uuid;
        }
        static string toUuidString(byte[] data)
        {
            long msb = 0;
            long lsb = 0;
            for (int i = 0; i < 8; i++)
                msb = (msb << 8) | (data[i] & 0xff);
            for (int i = 8; i < 16; i++)
                lsb = (lsb << 8) | (data[i] & 0xff);
            return (digits(msb >> 32, 8) + "-" +
                digits(msb >> 16, 4) + "-" +
                digits(msb, 4) + "-" +
                digits(lsb >> 48, 4) + "-" +
                digits(lsb, 12));
        }
        static string digits(long val, int digits)
        {
            long hi = 1L << (digits * 4);
            return (hi | (val & (hi - 1))).ToString("X").Substring(1);
        }
        /// <summary>
        ///     获取系统版本
        /// </summary>
        /// <returns>System Version</returns>
        public static string GetSystemVersion()
        {
            switch (Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor)
            {
                case "10.0":
                    return "10";
                case "6.3":
                    return "8.1";
                case "6.2":
                    return "8";
                case "6.1":
                    return "7";
                case "6.0":
                    return "2008";
                case "5.2":
                    return "2003";
                case "5.1":
                    return "XP";
                default:
                    return "unknow";
            }
        }
    }
}