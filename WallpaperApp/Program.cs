using System;
using System.Windows.Forms;
using WallpaperApp.Services;

namespace WallpaperApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            WallpaperService.SetWallpaperForPrimaryScreen();
        }
    }
}