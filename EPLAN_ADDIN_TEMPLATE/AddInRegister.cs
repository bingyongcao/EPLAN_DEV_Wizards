using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.Gui;
using Eplan.EplApi.Starter;
using EplanUtilities;
using System.Linq;

namespace EPLAN_ADDIN_TEMPLATE
{
    public class AddInRegister : IEplAddIn
    {
        static AddInRegister()
        {
            // EPLAN loads the add-in into its own process; the CLR's default
            // probing path doesn't include the add-in's directory, so hook the
            // AppDomain.AssemblyResolve event as early as possible to redirect
            // dependency lookups (EPLAN_UTILITIES, third-party libs under DLLs\)
            // to the add-in's own location.
            AssemblyResolver.Register();
        }

        public bool OnRegister(ref bool bLoadOnStart)
        {
            bLoadOnStart = true;
            GuiUtility.CleanCustomRibbonTab(m_newTabName);

            var ribbonBar = new RibbonBar();
            var newTab = ribbonBar.AddTab(m_newTabName);
            var cmdGroup = newTab.AddCommandGroup(m_commandGroupName, 0);
            cmdGroup.AddCommand(new RibbonCommandInfo(m_commandName, DefaultAction.ActionName)
            {
                Description = "",
                IndexButtonPosition = 0,
                Icon = new RibbonIcon(GuiUtility.ReplacePrimaryColor(Properties.Resources.airplay))
            });

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
    }
}