using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.Gui;
using EplanUtilities;
using System.Linq;

namespace EPLAN_ADDIN_TEMPLATE
{
    public class AddInRegister : IEplAddIn
    {
        public bool OnRegister(ref bool bLoadOnStart)
        {
            bLoadOnStart = true;
            GuiUtility.CleanCustomRibbonTab(m_newTabName);

            var ribbonBar = new RibbonBar();
            var newTab = ribbonBar.AddTab(m_newTabName);
            var cmdGroup = newTab.AddCommandGroup(m_commandGroupName, 0);
            RibbonCommandInfo ribbonCommandInfo = new RibbonCommandInfo(m_commandName, DefaultAction.ActionName)
            {
                Description = "",
                IndexButtonPosition = 0,
                Icon = new RibbonIcon(GuiUtility.ReplacePrimaryColor(Properties.Resources.airplay))
            };
            cmdGroup.AddCommand(ribbonCommandInfo);

            new Decider().Decide(
                EnumDecisionType.eOkDecision,
                $"<{m_newTabName}> addin registered successfully!",
                "Tip",
                EnumDecisionReturn.eOK,
                EnumDecisionReturn.eOK);

            return true;
        }
        public bool OnUnregister()
        {
            GuiUtility.CleanCustomRibbonTab(m_newTabName);

            new Decider().Decide(
                EnumDecisionType.eOkDecision,
                $"<{m_newTabName}> addin unregistered successfully!",
                "Tip",
                EnumDecisionReturn.eOK,
                EnumDecisionReturn.eOK);

            return true;
        }

        public bool OnInit()
        {
            return true;
        }
        public bool OnInitGui()
        {
            return true;
        }
        public bool OnExit()
        {
            return true;
        }

        public string m_newTabName = "EPLAN_ADDIN_TEMPLATE";
        public string m_commandGroupName = "Common";
        public string m_commandName = "DefaultCommand";
        public const string PRIMARY_COLOR = "currentColor";
    }
}