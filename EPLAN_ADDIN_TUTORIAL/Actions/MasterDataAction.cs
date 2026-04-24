using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;
using Eplan.EplApi.MasterData;
using EPLAN_API_TUTORIAL.Views;
using System.Linq;

namespace EPLAN_API_TUTORIAL
{
    public class MasterDataAction : IEplAction
    {
        public static string ActionName = "MasterDataAction";

        public bool Execute(ActionCallingContext ctx)
        {
            Project firstOpenedProject = new ProjectManager().CurrentProject;

            Project activeProj = new SelectionSet().GetCurrentProject(true);

            using (MDPartsDatabase partsDatabase = new MDPartsManagement().OpenDatabase())
            {
                // Get all parts with part number beginning with "PSL" using filter
                MDObjectFilter mDObjectFilter = new MDObjectFilter();
                mDObjectFilter.AddPropertyCondition(22001, MDObjectFilter.CompareOperator.OperatorEqual, "PSL*");
                MDPart[] partsByFilter = partsDatabase.GetParts(mDObjectFilter);

                // Get all parts with part number beginning with "PSL" using linq
                MDPart[] partsByLinq = partsDatabase.Parts.Where(item => item.Properties.ARTICLE_PARTNR.ToString().StartsWith("PSL")).ToArray();

                new Decider().Decide(
                EnumDecisionType.eOkDecision,
                $"there are {partsByFilter.Length} parts in master data beginning with \"PSL\"",
                "Tip",
                EnumDecisionReturn.eOK,
                EnumDecisionReturn.eOK);
            }

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
