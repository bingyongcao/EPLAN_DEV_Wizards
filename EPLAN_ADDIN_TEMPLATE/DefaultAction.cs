using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;

namespace EPLAN_ADDIN_TEMPLATE
{
    public class DefaultAction : IEplAction
    {
        public static string ActionName = "DefaultAction";
        public bool Execute(ActionCallingContext ctx)
        {
            new Decider().Decide(
                EnumDecisionType.eOkDecision,
                "Default action was called!",
                "Tip",
                EnumDecisionReturn.eOK,
                EnumDecisionReturn.eOK);

            Project project = new ProjectManager().CurrentProject;
            
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