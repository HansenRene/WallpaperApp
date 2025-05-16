using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics; // Add this namespace

// app.manifest content is important to disable DPI awarenes.


class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    private const int SPI_SETDESKWALLPAPER = 20;
    private const int SPIF_UPDATEINIFILE = 0x01;
    private const int SPIF_SENDCHANGE = 0x02;


    static void WriteLog(string _message)
    {
        // Get the path to the user's AppData\Local directory
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string appDirectory = Path.Combine(appDataPath, "WallpaperApp");

        // Create the WallpaperApp directory if it doesn't exist
        if (!Directory.Exists(appDirectory))
        {
            Directory.CreateDirectory(appDirectory);
        }

        // Generate the log file name based on today's date
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string logFilePath = Path.Combine(appDirectory, $"{today}.log");

        // Generate the log file name for yesterday
        string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        string yesterdayLogFilePath = Path.Combine(appDirectory, $"{yesterday}.log");

        // Delete yesterday's log file if it exists
        if (File.Exists(yesterdayLogFilePath))
        {
            File.Delete(yesterdayLogFilePath);
        }

        // Write to today's log file
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"Log entry for {DateTime.Now}");
            writer.WriteLine($"{DateTime.Now} - {_message}");
        }
    }

    static void Main()
    {

        var screen = Screen.PrimaryScreen;
        int width = screen.Bounds.Width;
        int height = screen.Bounds.Height;
        var resolution = $"{width}x{height}";

        var wallpaperPath = Path.Combine(WallpaperApp.Properties.Settings.Default.WallpaperPath, $"wallpaper_{resolution}.png");

        Debug.WriteLine($"Constructed wallpaper path: {wallpaperPath}");

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
}