csharp WallpaperApp\Utilities\RegistryHelper.cs
using System;
using Microsoft.Win32;

namespace WallpaperApp.Utilities
{
    public static class RegistryHelper
    {
        // Default subkey where settings are expected; adjust if your org uses a different path
        private const string DefaultSubKey = @"SOFTWARE\WallpaperApp";

        public static string GetString(string valueName, string subKey = DefaultSubKey)
        {
            try
            {
                // Try HKLM
                object v = Registry.GetValue($@"HKEY_LOCAL_MACHINE\{subKey}", valueName, null);
                if (v is string s && !string.IsNullOrEmpty(s)) return s;

                // Try Wow6432Node (common on 64-bit machines for 32-bit apps)
                v = Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\{subKey}", valueName, null);
                if (v is string s2 && !string.IsNullOrEmpty(s2)) return s2;

                // Try HKCU
                v = Registry.GetValue($@"HKEY_CURRENT_USER\{subKey}", valueName, null);
                if (v is string s3 && !string.IsNullOrEmpty(s3)) return s3;
            }
            catch
            {
                // swallow - registry read errors should not crash the app; caller will handle null
            }
            return null;
        }
    }
}