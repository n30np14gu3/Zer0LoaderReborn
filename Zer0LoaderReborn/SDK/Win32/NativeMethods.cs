using System;
using System.Runtime.InteropServices;

namespace Zer0LoaderReborn.SDK.Win32
{
    public class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        internal static extern bool FreeLibrary(IntPtr hModule);
    }
}