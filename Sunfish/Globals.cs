using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sunfish
{
    public static class Globals
    {
        public static bool IsExternalResource(long address)
        { return (address & 0xC0000000) != 0; }

        public static string ReverseString(string type)
        {
            char[] chars = type.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }
        
        /// <summary>
        /// Status
        /// </summary>
        public static string Status { get { return status; } set { if (StatusChanged != null) { status = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + " - " + value; StatusChanged(status); } } }
        private static string status;

        public static void ClearStatus()
        {
            Status = "Ready";
        }

        public delegate void StatusChangeHandler(string Status);
        public static event StatusChangeHandler StatusChanged;
    }

    public static class Padding
    {
        public const int DefaultAlignment = 512;

        public static int Pad(long address)
        { return (int)(address + GetCount(address)); }

        public static int Pad(long address, int alignemnt)
        { return (int)(address + GetCount(address, alignemnt)); }

        public static int GetCount(long address)
        { return (int)(-address) & (DefaultAlignment - 1); }

        public static int GetCount(long address, int alignment)
        { return (int)(-address) & (alignment - 1); }

        public static byte[] GetBytes(long address)
        { return new byte[GetCount(address, DefaultAlignment)]; }

        public static byte[] GetBytes(long address, int alignment)
        { return new byte[GetCount(address, alignment)]; }

        public static int Pad(this Stream stream)
        {
            stream.Write(GetBytes(stream.Position), 0, GetCount(stream.Position));
            return (int)stream.Position;
        }

        public static int Pad(this Stream stream, int alignment)
        {
            stream.Write(GetBytes(stream.Position, alignment), 0, GetCount(stream.Position, alignment));
            return (int)stream.Position;
        }
    }
}
