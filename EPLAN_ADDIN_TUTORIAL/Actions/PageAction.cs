using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;
using EPLAN_API_TUTORIAL.Views;
using System.Linq;

namespace EPLAN_API_TUTORIAL
{
    public class PageAction : IEplAction
    {
        public static string ActionName = "PageAction";

        public bool Execute(ActionCallingContext ctx)
        {
            Project activeProj = new SelectionSet().GetCurrentProject(true);

            // filter pages
            PagesFilter pagesFilter = new PagesFilter()
            {
                    Name = @"==S1=P01",
                    DocumentType = DocumentTypeManager.DocumentType.Circuit
            };
            Page[] pages = new DMObjectsFinder(activeProj).GetPages(pagesFilter);

            new Decider().Decide(
                EnumDecisionType.eOkDecision,
                $"count of all pages: {activeProj.Pages.Length}\n" +
                $"count of circuit pages whose name starts with '==S1=P01': {pages.Length}\n",
                "PageInfo",
                EnumDecisionReturn.eOK,
                EnumDecisionReturn.eOK);

            FunctionsFilter functionsFilter = new FunctionsFilter()
            {
                Page = pages[0],
                FunctionCategory = Eplan.EplApi.Base.Enums.FunctionCategory.Terminal,
                IsPlaced = true,
            };

            Function[] funcs = new DMObjectsFinder(activeProj)
                .GetFunctions(functionsFilter);


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
