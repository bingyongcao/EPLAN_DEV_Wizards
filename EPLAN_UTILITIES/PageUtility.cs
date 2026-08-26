using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using System.Collections.Generic;
using System.Linq;

namespace EplanUtilities
{
    public static class PageUtility
    {
        /// <summary>
        /// get pages by structure identifier
        /// </summary>
        /// <param name="project"></param>
        /// <param name="doubleEqual"></param>
        /// <param name="singleEqual"></param>
        /// <param name="designDocType"></param>
        /// <param name="docType"></param>
        /// <returns></returns>
        public static Page[] GetPages(
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

                efaFilter.SetFilteredPropertyList(ppl);

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

        /// <summary>
        /// get page by name
        /// </summary>
        /// <param name="project"></param>
        /// <param name="exactName"></param>
        /// <returns></returns>
        public static Page GetPage(
            Project project,
            string exactName)
        {
            try
            {
                PagesFilter filter = new PagesFilter()
                {
                    Name = exactName,
                    ExactNameMatching = true
                };

                return new DMObjectsFinder(project)
                    .GetPages(filter).FirstOrDefault();
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

        /// <summary>
        /// get project structure by page
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetProjectStructureByPage(
            Project project)
        {
            Dictionary<string, List<string>> pageStructTags = new Dictionary<string, List<string>>();

            foreach (var page in project.Pages)
            {
                var singleEqual = PropertyUtility.GetValueString(page.Properties.DESIGNATION_PLANT);
                var doubleEqual = PropertyUtility.GetValueString(page.Properties.DESIGNATION_FUNCTIONALASSIGNMENT);

                if (string.IsNullOrEmpty(singleEqual)) continue;

                if (!pageStructTags.ContainsKey(doubleEqual))
                {
                    pageStructTags[doubleEqual] = new List<string>() { singleEqual };
                }
                else if (!pageStructTags[doubleEqual].Contains(singleEqual))
                {
                    pageStructTags[doubleEqual].Add(singleEqual);
                }
            }

            return pageStructTags;
        }
    }
}