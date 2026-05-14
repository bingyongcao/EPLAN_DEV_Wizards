using Eplan.EplApi.Gui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Eplan.EplApi.Gui.RibbonTab;

namespace EplanUtilities
{
    public enum EplanColor
    {
        [Description("#E9EAEA")]
        DarkPrimary,

        [Description("#464646")]
        LightPrimary,

        [Description("#0D9BE2")]
        Blue,

        [Description("#E2001A")]
        Red,

        [Description("#F7CC1B")]
        Yellow,

        [Description("#F7821B")]
        Orange,

        [Description("#62BA46")]
        Green
    }

    public static class GuiUtility
    {
        /// <summary>
        /// default color keyword in lucide icon.
        /// </summary>
        public const string PRIMARY_COLOR = "currentColor";

        public static string ReplacePrimaryColor(string svgContent)
        {
            return svgContent.Replace(PRIMARY_COLOR, GetPrimaryColorByTheme());
        }

        public static string GetPrimaryColorByTheme()
        {
            return SettingUtility.GetEplanColorTheme() == WindowsTheme.Dark ? 
                GetHexCode(EplanColor.DarkPrimary) : 
                GetHexCode(EplanColor.LightPrimary);
        }

        public static string GetHexCode(EplanColor color)
        {
            var field = color.GetType().GetField(color.ToString());
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;
            return attribute.Description;
        }

        public static RibbonTab GetBuiltInRibbonTab(DefaultRibbonTabs defaultRibbon)
        {
            return new RibbonBar().Tabs
                .FirstOrDefault(item => item.Identifier == defaultRibbon);
        }

        public static void CleanCustomRibbonTab(string tabName)
        {
            int maxValue = Enum.GetValues(typeof(DefaultRibbonTabs))
                   .Cast<DefaultRibbonTabs>()
                   .Max(tab => (int)tab);

            var newTab = new RibbonBar().Tabs
                .FirstOrDefault(item => (int)item.Identifier > maxValue && item.Name == tabName);
            if (newTab != null) newTab.Remove();
        }
    }
}
