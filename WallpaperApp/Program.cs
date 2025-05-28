using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
// app.manifest content is important to disable DPI awarenes.

class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    private const int SPI_SETDESKWALLPAPER = 20;
    private const int SPIF_UPDATEINIFILE = 0x01;
    private const int SPIF_SENDCHANGE = 0x02;
    private static void SetWallpaper(string wallpaperPath, string style)
    {
        // Purpose: Applies wallpaper based on path, and wallpaper style.
        var styles = new Dictionary<string, string>
        {
            { "Fill", "10" }, { "Fit", "6" }, { "Stretch", "2" },
            { "Tile", "0" }, { "Center", "0" }, { "Span", "22" }
        };

        if (!styles.ContainsKey(style))
        {
            Console.WriteLine("Invalid wallpaper style.");
            return;
        }

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
        {
            if (key != null)
            {
                string currentStyle = key.GetValue("WallpaperStyle")?.ToString();
                string tileWallpaper = key.GetValue("TileWallpaper")?.ToString();

                string newStyle = styles[style];
                string newTile;
                if (style == "Tile")
                {
                    newTile = "1";
                }
                else
                {
                    newTile = "0";
                }
                    // Only update if the style is different
                if (currentStyle != newStyle || tileWallpaper != newTile)
                {
                    key.SetValue("WallpaperStyle", newStyle);
                    key.SetValue("TileWallpaper", newTile);
                    WriteLog("Updated WallpaperStyle");
                }
            }
        }
        if (File.Exists(wallpaperPath))
        {
            bool result = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, wallpaperPath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            if (!result)
            {
                int error = Marshal.GetLastWin32Error();
                Debug.WriteLine($"Failed to set wallpaper. Error code: {error}");
                WriteLog($"Failed to set wallpaper. Error code: {error}");
            }
            else
            {
                Debug.WriteLine($"Wallpaper set to {wallpaperPath}");
                WriteLog($"Wallpaper set to {wallpaperPath}");
            }
        }
        else
        {
            Debug.WriteLine($"Wallpaper file {wallpaperPath} does not exist.");
            WriteLog($"Wallpaper file {wallpaperPath} does not exist.");
        }
    }

    private static string GetClosestAspectRatio(int width, int height, float tolerance = 0.1f)
    {
        // Purpose: approximate aspect ratio from resolution
        // If diffeeence larger than tolerance, fallback to Wallpaper_Default.png
        float aspectRatio = (float)width / height;
        var standardRatios = new Dictionary<string, float>
        {
            { "16_10", 1.60f },
            { "16_9", 1.77f },
            { "21_9", 2.33f },
            { "32_9", 3.56f }
        };

        var closestMatch = standardRatios.OrderBy(r => Math.Abs(r.Value - aspectRatio)).First();

        // Check if it's within the tolerance range
        if (Math.Abs(closestMatch.Value - aspectRatio) <= tolerance)
        {
            return closestMatch.Key; // Return standard aspect ratio
        }
        else
        {
            return "Default"; // Indicates it's outside expected range
        }

        return standardRatios.OrderBy(r => Math.Abs(r.Value - aspectRatio)).First().Key;
    }

    private static void WriteLog(string _message)
    {
        // Purpose: delete .logfiles in directory older than today; then write to todays log
        // get AppData\Local
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        // append foldername
        string appDirectory = Path.Combine(appDataPath, "WallpaperApp");

        if (!Directory.Exists(appDirectory))
        {
            Directory.CreateDirectory(appDirectory);
        }

        DateTime today = DateTime.Now.Date;
        foreach (string file in Directory.GetFiles(appDirectory, "*.log"))
        {
            string fileName = Path.GetFileNameWithoutExtension(file);

            if (DateTime.TryParse(fileName, out DateTime fileDate) && fileDate < today)
            {
                File.Delete(file);
            }
        }
        string logFilePath = Path.Combine(appDirectory, $"{today:yyyy-MM-dd}.log");
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"Log entry for {DateTime.Now}");
            writer.WriteLine($"{DateTime.Now} - {_message}");
        }
    }


    private static bool IsDarkModeEnabled()
    {
        // Purpose: detect darkmode in order to append _dark tp wallpaper path if file exist.
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
        {
            if (key != null)
            {
                object value = key.GetValue("AppsUseLightTheme");
                if (value != null)
                {
                    if ((int)value == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        return false;
    }
    private static void Main()
    {
        // Purpose: Find screen resolution, detect dark mode, approximate aspect ratio, apply wallpaper
        string aspectRatio = GetClosestAspectRatio(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        string wallpaperPath = Path.Combine(WallpaperApp.Properties.Settings.Default.WallpaperPath, $"wallpaper_{aspectRatio}.png");
        Debug.WriteLine($"Constructed wallpaper path: {wallpaperPath}");

        if (IsDarkModeEnabled())
        {
            string darkmodeWallpaperpath = Path.Combine(WallpaperApp.Properties.Settings.Default.WallpaperPath, $"wallpaper_{aspectRatio}_Dark.png");
            WriteLog("Darkmode detected");
            Debug.WriteLine($"Constructed wallpaper path: {darkmodeWallpaperpath}");
            if (File.Exists(darkmodeWallpaperpath)){
                Debug.WriteLine($"Dark wallpaperpath found: {darkmodeWallpaperpath}");
                WriteLog("Darkmode detected");
                SetWallpaper(darkmodeWallpaperpath, "Stretch");
            }
            else
            {
                Debug.WriteLine($"Dark wallpaperpath NOT found: {darkmodeWallpaperpath}");
                SetWallpaper(wallpaperPath, "Stretch");
            }
        }
        else
        {
            SetWallpaper(wallpaperPath, "Stretch");
        }
    }
}