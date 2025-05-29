Upon executing, the program will detect screen resolution, approximate aspect ratio, and select a matching file named "wallpaper_X_Y.png" (ie. Pallpaper_16_9.png) from C:\ProgramData\Microsoft\WallpaperApp\Wallpapers (path configurable from Settings file).
If dark mode is enabled it will check if Wallpaper_16_9_Dark.png exist and apply that, or fallback to none-dark.

The goal is to allow enforcing a wallpaper that matches client screen resolution.

PNG format images is being used to avoid the TranscodedWallpaper workaround for getting around company enforced wallpapers that is applicable when using jpg.
