using Microsoft.Win32;

namespace EplanHelpers
{
    public enum WindowsTheme
    {
        Light,
        Dark
    }

    public static class WindowsUtility
    {
        public static WindowsTheme GetWindowsThemeFromRegistry()
        {
            try
            {
                int? registryValue = Registry.GetValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    "AppsUseLightTheme",
                    null) as int?;

                if (registryValue.HasValue)
                {
                    return registryValue.Value == 0 ? WindowsTheme.Dark : WindowsTheme.Light;
                }
            }
            catch
            {
            }

            return WindowsTheme.Light;
        }
    }
}