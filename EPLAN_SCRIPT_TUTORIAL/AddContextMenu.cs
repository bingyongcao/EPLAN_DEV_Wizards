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
    /// <summary>
    /// when EPLAN restart, the context menu will be removed, 
    /// you need to execute the 'AddContextMenuAction' again.
    /// </summary>
    public class AddContextMenu
    {
        // set refreshAfterChanges true to avoid flickering problem.
        RibbonBar myRibbonBar = new RibbonBar(true);

        public const string ActionName = "AddContextMenu";

        [DeclareAction(ActionName)]
        public void AddContextMenuAction()
        {
            Eplan.EplApi.Gui.ContextMenu ctm = new Eplan.EplApi.Gui.ContextMenu();
            ContextMenuLocation ctmLoc = new ContextMenuLocation();

            // ContextMenuName and DialogName are fixed based on diff dialogs
            // use 'ShowContextMenuInfo' script to get the correct values.
            ctmLoc.ContextMenuName = "Ged";
            ctmLoc.DialogName = "Editor";

            ctm.AddMenuItem(ctmLoc, "OpenProjectFolder", "OpenProjectFolder", true, false);
            ctm.AddMenuItem(ctmLoc, "OpenMACFolder", "OpenMACFolder", false, false);
        }

        [DeclareAction("OpenProjectFolder")]
        public void OpenProjectPathAction()
        {
            OpenFolder("$(PROJECTPATH)");
        }

        [DeclareAction("OpenMACFolder")]
        public void OpenProjectMACAction()
        {
            OpenFolder("$(MD_MACROS)");
        }

        public void OpenFolder(string folderName)
        {
            if (folderName != string.Empty)
            {
                if (folderName.StartsWith("$("))
                {
                    folderName = PathMap.SubstitutePath(folderName);
                }
                DirectoryInfo di = new DirectoryInfo(folderName);
                if (di.Exists)
                {
                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.FileName = "explorer.exe";
                    proc.Arguments = folderName;
                    Process.Start(proc);
                }
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
