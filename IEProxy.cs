using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
public partial class IEProxy
{
    public enum Options
    {
        INTERNET_PER_CONN_FLAGS = 1,
        INTERNET_PER_CONN_PROXY_SERVER = 2,
        INTERNET_PER_CONN_PROXY_BYPASS = 3,
        INTERNET_PER_CONN_AUTOCONFIG_URL = 4,
        INTERNET_PER_CONN_AUTODISCOVERY_FLAGS = 5,
        INTERNET_OPTION_REFRESH = 37,
        INTERNET_OPTION_PER_CONNECTION_OPTION = 75,
        INTERNET_OPTION_SETTINGS_CHANGED = 39,
        PROXY_TYPE_PROXY = 0x2,
        PROXY_TYPE_DIRECT = 0x1
    }
    [StructLayout(LayoutKind.Sequential)]
    private partial class FILETIME
    {
        public int dwLowDateTime;
        public int dwHighDateTime;
    }
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    private partial struct INTERNET_PER_CONN_OPTION
    {
        [FieldOffset(0)]
        public int dwOption;
        [FieldOffset(4)]
        public int dwValue;
        [FieldOffset(4)]
        public IntPtr pszValue;
        [FieldOffset(4)]
        public IntPtr ftValue;
        public byte[] GetBytes()
        {
            var b = new byte[13];
            BitConverter.GetBytes(dwOption).CopyTo(b, 0);
            switch (dwOption)
            {
                case (int)Options.INTERNET_PER_CONN_FLAGS:
                    {
                        BitConverter.GetBytes(dwValue).CopyTo(b, 4);
                        break;
                    }

                case (int)Options.INTERNET_PER_CONN_PROXY_BYPASS:
                    {
                        BitConverter.GetBytes(pszValue.ToInt32()).CopyTo(b, 4);
                        break;
                    }

                case (int)Options.INTERNET_PER_CONN_PROXY_SERVER:
                    {
                        BitConverter.GetBytes(pszValue.ToInt32()).CopyTo(b, 4);
                        break;
                    }
            }
            return b;
        }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private partial class INTERNET_PER_CONN_OPTION_LIST
    {
        public int dwSize;
        public string pszConnection;
        public int dwOptionCount;
        public int dwOptionError;
        public IntPtr pOptions;
    }
    [StructLayout(LayoutKind.Sequential)]
    private partial class INTERNET_PROXY_INFO
    {
        public int dwAccessType;
        public IntPtr lpszProxy;
        public IntPtr lpszProxyBypass;
    }
    private const int ERROR_INSUFFICIENT_BUFFER = 122;
    private const int INTERNET_OPTION_PROXY = 38;
    private const int INTERNET_OPEN_TYPE_DIRECT = 1;
    [DllImport("wininet.dll")]
    private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, INTERNET_PER_CONN_OPTION_LIST lpBuffer, int dwBufferLength);
    [DllImport("kernel32.dll")]
    private static extern int GetLastError();
    public static bool SetProxy(string proxy_full_addr)
    {
        bool bReturn;
        var list = new INTERNET_PER_CONN_OPTION_LIST();
        int dwBufSize = Marshal.SizeOf(list);
        var opts = new INTERNET_PER_CONN_OPTION[4];
        int opt_size = Marshal.SizeOf(opts[0]);
        list.dwSize = dwBufSize;
        list.pszConnection = Convert.ToString(ControlChars.NullChar);
        list.dwOptionCount = 3;
        opts[0].dwOption = (int)Options.INTERNET_PER_CONN_FLAGS;
        opts[0].dwValue = (int)(Options.PROXY_TYPE_DIRECT | Options.PROXY_TYPE_PROXY);
        opts[1].dwOption = (int)Options.INTERNET_PER_CONN_PROXY_SERVER;
        opts[1].pszValue = Marshal.StringToHGlobalAnsi(proxy_full_addr);
        opts[2].dwOption = (int)Options.INTERNET_PER_CONN_PROXY_BYPASS;
        opts[2].pszValue = Marshal.StringToHGlobalAnsi("local");
        var b = new byte[3 * opt_size + 1];
        opts[0].GetBytes().CopyTo(b, 0);
        opts[1].GetBytes().CopyTo(b, opt_size);
        opts[2].GetBytes().CopyTo(b, 2 * opt_size);
        var ptr = Marshal.AllocCoTaskMem(3 * opt_size);
        Marshal.Copy(b, 0, ptr, 3 * opt_size);
        list.pOptions = ptr;
        bReturn = InternetSetOption(IntPtr.Zero, (int)Options.INTERNET_OPTION_PER_CONNECTION_OPTION, list, dwBufSize);
        if (!bReturn)
        {
            Debug.WriteLine(GetLastError());
        }
        bReturn = InternetSetOption(IntPtr.Zero, (int)Options.INTERNET_OPTION_SETTINGS_CHANGED, null, 0);
        if (!bReturn)
        {
            Debug.WriteLine(GetLastError());
        }
        bReturn = InternetSetOption(IntPtr.Zero, (int)Options.INTERNET_OPTION_REFRESH, null, 0);
        if (!bReturn)
        {
            Debug.WriteLine(GetLastError());
        }
        Marshal.FreeHGlobal(opts[1].pszValue);
        Marshal.FreeHGlobal(opts[2].pszValue);
        Marshal.FreeCoTaskMem(ptr);
        return bReturn;
    }
    public static bool DisableProxy()
    {
        bool bReturn;
        var list = new INTERNET_PER_CONN_OPTION_LIST();
        int dwBufSize = Marshal.SizeOf(list);
        var opts = new INTERNET_PER_CONN_OPTION[1];
        int opt_size = Marshal.SizeOf(opts[0]);
        list.dwSize = dwBufSize;
        list.pszConnection = Convert.ToString(ControlChars.NullChar);
        list.dwOptionCount = 1;
        opts[0].dwOption = (int)Options.INTERNET_PER_CONN_FLAGS;
        opts[0].dwValue = (int)Options.PROXY_TYPE_DIRECT;
        var b = new byte[opt_size + 1];
        opts[0].GetBytes().CopyTo(b, 0);
        var ptr = Marshal.AllocCoTaskMem(opt_size);
        Marshal.Copy(b, 0, ptr, opt_size);
        list.pOptions = ptr;
        bReturn = InternetSetOption(IntPtr.Zero, (int)Options.INTERNET_OPTION_PER_CONNECTION_OPTION, list, dwBufSize);
        if (!bReturn)
        {
            Debug.WriteLine(GetLastError());
        }
        bReturn = InternetSetOption(IntPtr.Zero, (int)Options.INTERNET_OPTION_SETTINGS_CHANGED, null, 0);
        if (!bReturn)
        {
            Debug.WriteLine(GetLastError());
        }
        bReturn = InternetSetOption(IntPtr.Zero, (int)Options.INTERNET_OPTION_REFRESH, null, 0);
        if (!bReturn)
        {
            Debug.WriteLine(GetLastError());
        }
        Marshal.FreeCoTaskMem(ptr);
        return bReturn;
    }
}