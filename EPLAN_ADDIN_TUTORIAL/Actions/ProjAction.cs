using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;
using EPLAN_API_TUTORIAL.Views;

namespace EPLAN_API_TUTORIAL
{
    public class ProjAction : IEplAction
    {
        public static string ActionName = "ProjAction";

        public bool Execute(ActionCallingContext ctx)
        {
            Project firstOpenedProject = new ProjectManager().CurrentProject;

            Project activeProj = new SelectionSet().GetCurrentProject(true);

            new Decider().Decide(
                EnumDecisionType.eOkCancelDecision,
                $"The first opened project: {firstOpenedProject.ProjectName}\n" +
                $"The active project: {activeProj.ProjectName}",
                "Project",
                EnumDecisionReturn.eOK,
                EnumDecisionReturn.eOK);

            var window = new ProjectPropertiesWindow();
            window.ShowDialog();

            return true;
        }

        public bool OnRegister(ref string Name, ref int Ordinal)
        {
            Name = ActionName;
            Ordinal = 20;
            return true;
        }
        public void GetActionProperties(ref ActionProperties actionProperties)
        {
        }
    }
}
