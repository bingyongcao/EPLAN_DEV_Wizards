using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Gui;
using System.Linq;

namespace EPLAN_API_TUTORIAL
{
    public class AddInRegister : IEplAddIn
    {
        public bool OnRegister(ref bool bLoadOnStart)
        {
            bLoadOnStart = true;
            CleanRibbonTab(m_newTabName);

            var ribbonBar = new RibbonBar();
            var newTab = ribbonBar.AddTab(m_newTabName);
            var cmdGroup = newTab.AddCommandGroup(m_commandGroupName, 0);
            RibbonCommandInfo ribbonCommandInfo = new RibbonCommandInfo(m_commandName, ProjAction.ActionName) 
            {
                Description = "",
                IndexButtonPosition = 0,
                Icon = new RibbonIcon(CommandIcon.Octagon_0)
            };
            cmdGroup.AddCommand(ribbonCommandInfo);
            return true;
        }
        public bool OnUnregister()
        {
            CleanRibbonTab(m_newTabName);
            return true;
        }

        void CleanRibbonTab(string tabName)
        {
            var newTab = new RibbonBar().Tabs
                .FirstOrDefault(item => item.Name == tabName);
            if (newTab != null)
                newTab.Remove();
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

        public string m_newTabName = "EPLAN_API_TUTORIAL";
        public string m_commandGroupName = "Common";
        public string m_commandName = "Project";
    }
}
