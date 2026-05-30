using System;
using System.Diagnostics;
using System.IO;

namespace WallpaperApp.Utilities
{
    public static class Logger
    {
        public static void Log(string message)
        {
            Debug.WriteLine(message);
            try

            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string appDirectory = Path.Combine(appDataPath, "WallpaperApp");
                if (!Directory.Exists(appDirectory))
                    Directory.CreateDirectory(appDirectory);

                string today = DateTime.Now.ToString("yyyy-MM-dd");
                string logFilePath = Path.Combine(appDirectory, $"{today}.log");
                string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                string yesterdayLogFilePath = Path.Combine(appDirectory, $"{yesterday}.log");

                if (File.Exists(yesterdayLogFilePath))
                {
                    File.Delete(yesterdayLogFilePath);
                }

                using (var writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:O} - {message}");
                }
            }
            catch
            {
                // Prevent logging errors from breaking the application
            }
        }
    }
}