Upon executing, the program will detect screen resolution, and select a matching file named "wallpaper_{width}x{height}.png" from C:\Wallpapers (path configurable from Settings file)

Default path is subject to change, and settings will probably chance location as well, to a more enteprise friendly location, (like the registry).

The goal is to allow enforcing a wallpaper that matches client screen resolution.

png format images is being used to avoid the TranscodedWallpaper workaround for getting around company enforced wallpapers that is applicable when using jpg.

I intend to also add support for detecting if darkmode ui is enabled, and have the app check for a matching wallpaper_{width}x{height}_dark.png image.
Currently the app looks for specific screen resolutions, rather than aspect ratio, which would be preferably.
