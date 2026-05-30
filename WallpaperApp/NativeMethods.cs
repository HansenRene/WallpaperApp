using System.Runtime.InteropServices;
using System;

#if NET472
// System.DirectoryServices.AccountManagement is available on .NET Framework 4.7.2,
// but ensure the project references the assembly in Visual Studio if you get compilation errors.
using System.DirectoryServices.AccountManagement;
#endif

namespace WallpaperApp
{
    internal static class NativeMethods
    {
        internal const int SPI_SETDESKWALLPAPER = 20;
        internal const int SPIF_UPDATEINIFILE = 0x01;
        internal const int SPIF_SENDCHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}