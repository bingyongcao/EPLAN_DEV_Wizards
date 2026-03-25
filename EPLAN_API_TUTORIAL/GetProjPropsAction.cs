using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using System.Windows.Forms;

namespace EPLAN_API_TUTORIAL
{
    public class GetProjPropsAction : IEplAction
    {
        public bool Execute(ActionCallingContext ctx)
        {
            new Decider().Decide(
                EnumDecisionType.eOkDecision, 
                "CSharpAction was called!",
                "Tip", 
                EnumDecisionReturn.eOK, 
                EnumDecisionReturn.eOK);
            
            Project project = new ProjectManager().CurrentProject;
            string propListStr = string.Empty;

            // Iterate over all project properties
            foreach (AnyPropertyId hPProp in project.Properties.ExistingIds)
            {
                var propValue = project.Properties[hPProp];
                if (propValue.Definition.IsInternal == false && !propValue.IsEmpty &&
                    propValue.Definition.Type == PropertyDefinition.PropertyType.String)
                {
                    propListStr += $"{propValue.Definition.Name}: {propValue.ToString()}\n";
                }
            }

            MessageBox.Show(propListStr, "Tip");

            return true;
        }
        public bool OnRegister(ref string Name, ref int Ordinal)
        {
            Name = "GetProjPropsAction";
            Ordinal = 20;
            return true;
        }
        public void GetActionProperties(ref ActionProperties actionProperties)
        {
        }
    }


}
