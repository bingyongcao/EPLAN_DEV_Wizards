#region should be included to avoid namespace conflict
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
# endregion

using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.Gui;
using Eplan.EplApi.Scripting;

namespace EPLAN_SCRIPT_TUTORIAL
{
    public class Settings
    {
        // set refreshAfterChanges true to avoid flickering problem.
        RibbonBar myRibbonBar = new RibbonBar(true);

        public const string ActionName = "Settings";

        [DeclareAction(ActionName)]
        public void SettingsAction()
        {
            var o_Settings = new Eplan.EplApi.Base.Settings();

            // str setting path can be found in EPLAN settings export file(xml)
            string language = o_Settings.GetStringSetting("USER.SYSTEM.GUI.LANGUAGE", 0);

            // EPLAN script does not support
            //MessageBox.Show($"current language is {language}"); 
            MessageBox.Show("current language is " + language);

            string backgroundColor = o_Settings.GetStringSetting("USER.GedViewer.ColorSchema.Current", 0);

            MessageBox.Show("current background color is " + backgroundColor);

            if (backgroundColor == "White")
            {
                o_Settings.SetStringSetting("USER.GedViewer.ColorSchema.Current", "Black", 0);
            }
            else
            {
                o_Settings.SetStringSetting("USER.GedViewer.ColorSchema.Current", "White", 0);
            }
        }

        #region extend UI
        [DeclareRegister]
        public void RegisterRibbonItems()
        {
            var newTab = myRibbonBar.Tabs.FirstOrDefault(item => item.Name == m_newTabName);
            if (newTab == null) newTab = myRibbonBar.AddTab(m_newTabName);

            var cmdGroup = newTab.CommandGroups.FirstOrDefault(item => item.Name == m_commandGroupName);
            if (cmdGroup == null) cmdGroup = newTab.AddCommandGroup(m_commandGroupName, 0);

            var command = cmdGroup.AddCommand(m_commandName, ActionName);
        }

        [DeclareUnregister]
        public void UnRegisterRibbonItems()
        {
            CleanCommand(ActionName);
        }

        void CleanCommand(string actionCommandLine)
        {
            var newTab = myRibbonBar.Tabs
                .FirstOrDefault(item => item.Name == m_newTabName);
            if (newTab == null) return;

            var cmdGroup = newTab.CommandGroups.FirstOrDefault(item => item.Name == m_commandGroupName);
            if (cmdGroup == null) return;

            var command = cmdGroup.Commands.FirstOrDefault(item => item.Value.ActionCommandLine == ActionName).Value;
            if (command == null) return;

            command.Remove();
        }

        public string m_newTabName = "EPLAN_SCRIPT_TUTORIAL";
        public string m_commandGroupName = "Common";
        public string m_commandName = ActionName;
        #endregion
    }
}
