using System;
using System.Security.Principal;


namespace WallpaperApp.Utilities
{
    public static class UserHelper
    {


        private static string GetCurrentUserUpn()
        {
            try
            {

                // Attempt to get the UPN of the currently signed-in user
                string Username = Environment.UserName;
                Logger.Log("username:" + Username);
                return Environment.UserName;

            }
            catch
            {
                Logger.Log("Failed to retrieve current user's UPN.");
                return null;
            }
        }
        public static string GetDomainfromUPN()
        {
            string upn = GetCurrentUserUpn();
            try
            {
                var parts = upn.Split('@');
                if (parts.Length == 2)
                {
                    return parts[1];
                }
                else
                {
                    Logger.Log($"Invalid UPN format: {upn}");
                    return null;
                }
            }
            catch
            {
                Logger.Log("Failed to extract domain from UPN.");
                return null;
            }
        }    }
}