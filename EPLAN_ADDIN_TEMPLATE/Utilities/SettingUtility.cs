namespace EplanUtilities
{
    public static class SettingUtility
    {
        public static WindowsTheme GetEplanColorTheme()
        {
            var o_Settings = new Eplan.EplApi.Base.Settings();

            try
            {
                int colorScheme = o_Settings.GetNumericSetting("USER.MF.GuiColorScheme", 0);

                switch (colorScheme)
                {
                    case 0:
                        return WindowsUtility.GetWindowsThemeFromRegistry();
                    case 1:
                        return WindowsTheme.Dark;
                    case 2:
                        return WindowsTheme.Light;
                    default:
                        return WindowsTheme.Light;
                }
            }
            catch
            {
            }

            return WindowsTheme.Light;
        }
    }
}