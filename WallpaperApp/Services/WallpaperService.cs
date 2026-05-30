using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WallpaperApp.Utilities;
using WallpaperApp;

namespace WallpaperApp.Services
{
    public static class WallpaperService
    {
        public static void SetWallpaperForPrimaryScreen()
        {
            var screen = Screen.PrimaryScreen;
            int width = screen.Bounds.Width;
            int height = screen.Bounds.Height;
            var resolution = $"{width}x{height}";
            // Todo : Determine wether to use UPN or manual

            var wallpaperPath = Path.Combine(Properties.Settings.Default.WallpaperPath, $"wallpaper_{resolution}.png");
            Debug.WriteLine($"Constructed wallpaper path: {wallpaperPath}");

            if (File.Exists(wallpaperPath))
            {
                bool result = NativeMethods.SystemParametersInfo(NativeMethods.SPI_SETDESKWALLPAPER, 0, wallpaperPath, NativeMethods.SPIF_UPDATEINIFILE | NativeMethods.SPIF_SENDCHANGE);
                if (!result)
                {
                    int error = Marshal.GetLastWin32Error();
                    Logger.Log($"Failed to set wallpaper. Error code: {error}");
                }
                else
                {
                    Logger.Log($"Wallpaper set to {wallpaperPath}");
                }
            }
            else
            {
                Logger.Log($"Wallpaper file {wallpaperPath} does not exist.");
            }
        }
    }
}