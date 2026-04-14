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
    public class AddCustomUI
    {
        // set refreshAfterChanges true to avoid flickering problem.
        RibbonBar myRibbonBar = new RibbonBar(true);

        public const string ActionName = "MyScriptAction";

        /// <summary>
        /// The function with a 'DeclareAction' attribute can be registered as an action in EPLAN.
        /// </summary>
        [DeclareAction(ActionName)]
        public void MyScriptAction()
        {
            new Decider().Decide(
                EnumDecisionType.eOkDecision,
                "MyScriptAction was called!", 
                "RegisterScriptAction", 
                EnumDecisionReturn.eOK, 
                EnumDecisionReturn.eOK);

            return;
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
        public string m_commandName = "Button";
        #endregion
    }
}
