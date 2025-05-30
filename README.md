Upon executing, the program will detect screen resolution, approximate aspect ratio, and select a matching file named "wallpaper_X_Y.png" (ie. Pallpaper_16_9.png) from C:\ProgramData\Microsoft\WallpaperApp\Wallpapers (path configurable from Settings file).
If dark mode is enabled it will check if Wallpaper_16_9_Dark.png exist and apply that, or fallback to none-dark.

The goal is to allow enforcing a wallpaper that matches client screen resolution.

PNG format images is being used to avoid the TranscodedWallpaper workaround for getting around company enforced wallpapers that is applicable when using jpg.

Deployment example:
Copying the WallpaperApp.exe + WallpaperApp.exe.config to client (C:\ProgramData\Microsoft\WallpaperApp), 
Create Schedule task to call the .exe with triggers for device unlock and account logon.

The Program produces a logfile in %LocalAppdata%\WallpaperApp
![image](https://github.com/user-attachments/assets/91fce1c5-9713-45c1-b23e-d5a3e14ebad3)
