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

            #region create page
            Page overviewPage = new Page(activeProj, DocumentTypeManager.DocumentType.Overview, new PagePropertyList()
            {
                DESIGNATION_DOCTYPE = "EFA",
                DESIGNATION_PLANT = "P01",
                DESIGNATION_FUNCTIONALASSIGNMENT = "S1",
                PAGE_NAME = 1
            });

            // set page description property after page creation
            overviewPage.Properties.PAGE_NOMINATIOMN = "page description";
            #endregion

            // filter pages
            PagesFilter pagesFilter = new PagesFilter()
            {
                DocumentType = DocumentTypeManager.DocumentType.Circuit
            };

            pagesFilter.SetFilteredPropertyList(new PagePropertyList()
            {
                DESIGNATION_PLANT = "P01",
                DESIGNATION_FUNCTIONALASSIGNMENT = "S1",
            });

            Page[] filterPages = new DMObjectsFinder(activeProj).GetPages(pagesFilter);

            new Decider().Decide(
                EnumDecisionType.eOkDecision,
                $"count of all pages: {activeProj.Pages.Length}\n" +
                $"count of circuit pages in '==S1=P01' hierarchy: {filterPages.Length}\n",
                "PageInfo",
                EnumDecisionReturn.eOK,
                EnumDecisionReturn.eOK);

            foreach (Page page in filterPages)
            {
                // get page description by PAGE_NOMINATIOMN property
                var pageDescrProp = page.Properties.PAGE_NOMINATIOMN;

                // get page description by property int
                pageDescrProp = page.Properties[11011];

                FunctionsFilter functionsFilter = new FunctionsFilter()
                {
                    Page = page,
                    FunctionCategory = Eplan.EplApi.Base.Enums.FunctionCategory.PLCTerminal,
                    IsPlaced = true
                };

                Function[] filterFuncs = new DMObjectsFinder(activeProj)
                    .GetFunctions(functionsFilter);
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
