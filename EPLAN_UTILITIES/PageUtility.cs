using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using System.Linq;

namespace EplanUtilities
{
    public static class PageUtility
    {
        public static Page[] GetFilterPages(
            Project project,
            string doubleEqual,
            string singleEqual,
            string designDocType = "",
            DocumentTypeManager.DocumentType docType = DocumentTypeManager.DocumentType.Undefined)
        {
            try
            {
                PagesFilter efaFilter = new PagesFilter();

                var ppl = new PagePropertyList()
                {
                    DESIGNATION_PLANT = singleEqual,
                    DESIGNATION_FUNCTIONALASSIGNMENT = doubleEqual,
                };

                if (!string.IsNullOrEmpty(designDocType))
                {
                    ppl.DESIGNATION_DOCTYPE = designDocType;
                }

                if (docType != DocumentTypeManager.DocumentType.Undefined)
                {
                    efaFilter.DocumentType = docType;
                }

                new PagesFilter().SetFilteredPropertyList(ppl);

                return new DMObjectsFinder(project)
                    .GetPages(efaFilter).ToArray();
            }
            catch (System.Exception ex)
            {
                new Decider().Decide(
                    EnumDecisionType.eOkDecision,
                    $"{ex.Message}",
                    "Error",
                    EnumDecisionReturn.eOK,
                    EnumDecisionReturn.eOK);
                return null;
            }
        }
    }
}