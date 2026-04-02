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
            Project project = new ProjectManager().CurrentProject;

            Project selectedProj = new SelectionSet().GetCurrentProject(true);

            new Decider().Decide(
                EnumDecisionType.eOkCancelDecision,
                $"The first opened project: {project.ProjectName}\n" +
                $"The active project: {selectedProj.ProjectName}",
                "Project",
                EnumDecisionReturn.eOK,
                EnumDecisionReturn.eOK);

            var window = new ProjectPropertiesWindow();
            window.ShowDialog();

            return true;
        }


        private string GetAllProjProps(Project project)
        {
            string propListStr = string.Empty;

            foreach (PropertyValue propValue in project.Properties.ExistingValues)
            {
                var propDef = propValue.Definition;

                if (propDef.IsInternal) continue;

                if (propDef.IsIndexed)
                {
                    for (int i = 1; i < propDef.MaxIndex + 1; i++)
                    {
                        var indexProp = propValue[i];
                        if (!indexProp.IsEmpty)
                        {
                            propListStr += $"<{indexProp.Id.AsInt} {i}>{indexProp.Definition.Name}: {indexProp.ToString()}\n";
                        }
                    }
                }
                else
                {
                    if (!propValue.IsEmpty)
                    {
                        propListStr += $"<{propValue.Id.AsInt}>{propValue.Definition.Name}: {propValue.ToString()}\n";
                    }
                }
            }

            return propListStr;
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
