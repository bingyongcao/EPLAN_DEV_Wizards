using Eplan.EplApi.Base;
using Eplan.EplApi.Scripting;
using System.Linq;
using Eplan.EplApi.Gui;
using System.Windows;

namespace EPLAN_SCRIPT_TUTORIAL
{
    public class SimpleScriptForRegister
    {
        // set refreshAfterChanges true to avoid flickering problem.
        RibbonBar myRibbonBar = new RibbonBar(true);

        /// <summary>
        /// The function with a 'DeclareAction' attribute can be registered as an action in EPLAN.
        /// </summary>
        [DeclareAction("MyScriptAction")]
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
            CleanRibbonTab();

            var newTab = myRibbonBar.AddTab(m_newTabName);
            var cmdGroup = newTab.AddCommandGroup(m_commandGroupName, 0);
            RibbonCommandInfo ribbonCommandInfo = new RibbonCommandInfo(m_commandName, "MyScriptAction")
            {
                Description = "a description of action",
                IndexButtonPosition = 0,
                Icon = new RibbonIcon(CommandIcon.Circle_0)
            };
            var command = cmdGroup.AddCommand(ribbonCommandInfo);
        }

        [DeclareUnregister]
        public void UnRegisterRibbonItems()
        {
            CleanRibbonTab();
        }

        void CleanRibbonTab()
        {
            var newTab = myRibbonBar.Tabs
                .FirstOrDefault(item => item.Name == m_newTabName);
            if (newTab != null)
                newTab.Remove();
        }

        public string m_newTabName = "EPLAN_SCRIPT_TUTORIAL";
        public string m_commandGroupName = "Common";
        public string m_commandName = "Button";
        #endregion
    }
}
