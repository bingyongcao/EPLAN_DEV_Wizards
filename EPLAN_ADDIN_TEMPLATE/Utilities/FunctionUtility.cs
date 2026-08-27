using Eplan.EplApi.Base;
using Eplan.EplApi.Base.Enums;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using Eplan.EplApi.HEServices;
using System;
using System.Collections.Generic;
using System.Linq;
using SymbolVariant = Eplan.EplApi.DataModel.MasterData.SymbolVariant;
using TerminalStrip = Eplan.EplApi.DataModel.EObjects.TerminalStrip;

namespace EplanUtilities
{
    public static class FunctionUtility
    {
        /// <summary>
        /// get all main func by structure identifier
        /// </summary>
        /// <param name="project"></param>
        /// <param name="doubleEqual"></param>
        /// <param name="singleEqual"></param>
        /// <returns></returns>
        public static Function[] GetAllMainFuncs(
            Project project,
            string doubleEqual,
            string singleEqual)
        {
            FunctionsFilter funcFilter = new FunctionsFilter();
            var fpl = new FunctionPropertyList
            {
                FUNC_MAINFUNCTION = true,
                DESIGNATION_PLANT = singleEqual,
                DESIGNATION_FUNCTIONALASSIGNMENT = doubleEqual,
            };
            funcFilter.SetFilteredPropertyList(fpl);

            return new DMObjectsFinder(project).GetFunctions(funcFilter);
        }

        /// <summary>
        /// place a non-main func on page
        /// </summary>
        /// <param name="mainFunc"></param>
        /// <param name="page"></param>
        /// <param name="sv"></param>
        /// <param name="loc"></param>
        /// <param name="placeSchemaName"></param>
        /// <returns></returns>
        public static Function CreateSubFunc(
            Function mainFunc,
            Page page,
            SymbolVariant sv,
            PointD loc,
            string placeSchemaName = "")
        {
            try
            {
                var subFunc = new Function();
                subFunc.Create(page, sv);
                subFunc.IsMainFunction = false;
                subFunc.Name = mainFunc.Name;
                subFunc.VisibleName = mainFunc.Properties[20008];

                //set pins descriptions
                var pinCount = mainFunc.FunctionDefinition.ConnectionPoints.Length;
                for (int i = 0; i < pinCount; i++)
                {
                    subFunc.Properties.FUNC_CONNECTIONDESIGNATION[i + 1]
                        = mainFunc.Properties.FUNC_CONNECTIONDESIGNATION[i + 1];
                }

                // set engraving text
                subFunc.Properties[20025] = mainFunc.Properties[20025];
                // set technical characteristics
                subFunc.Properties[20027] = mainFunc.Properties[20027];

                //set location
                subFunc.Location = loc;

                //adjust representation type
                subFunc.ManualPlacementType = page.PageType;

                if (!string.IsNullOrEmpty(placeSchemaName))
                {
                    subFunc.PropertyPlacementsSchemas.Selected =
                    subFunc.PropertyPlacementsSchemas.All.First(s => s.Name == placeSchemaName);
                }

                return subFunc;
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
        /// place a device on page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="strPartNr"></param>
        /// <param name="visibleName"></param>
        /// <param name="sv"></param>
        /// <param name="loc"></param>
        /// <param name="placeSchemaName"></param>
        /// <returns></returns>
        public static Function CreateDevice(
            Page page,
            string strPartNr,
            string visibleName,
            SymbolVariant sv,
            PointD loc,
            string placeSchemaName = "")
        {
            try
            {
                DeviceService deviceService = new DeviceService();

                var funcs = deviceService.CreateDevice(strPartNr, "1", page, loc);

                Function createdFunc = funcs[0];

                // set name before SymbolVariant
                createdFunc.Name = $"=={page.Properties.DESIGNATION_FUNCTIONALASSIGNMENT}={page.Properties.DESIGNATION_PLANT}-{visibleName}";
                createdFunc.VisibleName = visibleName;
                createdFunc.SymbolVariant = sv;

                if (!string.IsNullOrEmpty(placeSchemaName))
                {
                    createdFunc.PropertyPlacementsSchemas.Selected =
                    createdFunc.PropertyPlacementsSchemas.All.First(s => s.Name == placeSchemaName);
                }

                return createdFunc;
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

        public static Function[] GetFuncs(
            Page page,
            FunctionCategory funcCategory)
        {
            try
            {
                FunctionsFilter filter = new FunctionsFilter()
                {
                    IsPlaced = true,
                    Page = page,
                    FunctionCategory = funcCategory
                };
                return new DMObjectsFinder(page.Project)
                    .GetFunctions(filter);
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

        public static Function GetFunc(
            Page page,
            string exactName)
        {
            try
            {
                FunctionsFilter filter = new FunctionsFilter()
                {
                    ExactNameMatching = true,
                    IsPlaced = true,
                    Page = page,
                    Name = exactName
                };
                return new DMObjectsFinder(page.Project)
                    .GetFunctions(filter).FirstOrDefault();
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
        /// get all terminal strips by structure identifier
        /// </summary>
        /// <param name="project"></param>
        /// <param name="doubleEqual"></param>
        /// <param name="singleEqual"></param>
        /// <returns></returns>
        public static TerminalStrip[] GetTerminalStrips(
            Project project,
            string doubleEqual,
            string singleEqual)
        {
            try
            {
                FunctionsFilter funcFilter = new FunctionsFilter();

                var fpl = new FunctionPropertyList()
                {
                    DESIGNATION_PLANT = singleEqual,
                    DESIGNATION_FUNCTIONALASSIGNMENT = doubleEqual,
                };

                funcFilter.SetFilteredPropertyList(fpl);

                return new DMObjectsFinder(project)
                    .GetTerminalStrips(funcFilter)
                    .ToArray();
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
        /// get and create all terminal strips by structure identifier
        /// </summary>
        /// <param name="project"></param>
        /// <param name="doubleEqual"></param>
        /// <param name="singleEqual"></param>
        /// <returns></returns>
        public static TerminalStrip[] GetOrCreateTerminalStrips(
            Project project,
            string doubleEqual,
            string singleEqual)
        {
            List<TerminalStrip> terminalStrips = new List<TerminalStrip>();

            try
            {
                FunctionDefinitionLibrary fdl = new FunctionDefinitionLibrary(project);
                var funcDef = new FunctionDefinition(
                    fdl,
                    FunctionCategory.TerminalDefText,
                    1,
                    1);

                FunctionsFilter funcFilter = new FunctionsFilter()
                {
                    FunctionCategory = FunctionCategory.Terminal,
                };

                var fpl = new FunctionPropertyList()
                {
                    DESIGNATION_PLANT = singleEqual,
                    DESIGNATION_FUNCTIONALASSIGNMENT = doubleEqual,
                };

                funcFilter.SetFilteredPropertyList(fpl);

                var terminalGroups = new DMObjectsFinder(project)
                    .GetTerminals(funcFilter)
                    .GroupBy(t => t.Properties[20008].ToString());

                foreach (var group in terminalGroups)
                {
                    if (string.IsNullOrEmpty(group.Key)) continue;

                    string fullName = $"=={doubleEqual}={singleEqual}-{group.Key}";

                    FunctionsFilter filter = new FunctionsFilter()
                    {
                        ExactNameMatching = true,
                        Name = fullName
                    };

                    var foundTerminalStrip = new DMObjectsFinder(project)
                        .GetTerminalStrips(filter).FirstOrDefault();
                    if (foundTerminalStrip == null)
                        foundTerminalStrip = CreateTerminalStrip(project, funcDef, fullName);

                    terminalStrips.Add(foundTerminalStrip);
                }

                return terminalStrips.ToArray();
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
        /// create terminal strp by name
        /// </summary>
        /// <param name="project"></param>
        /// <param name="funcDef"></param>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static TerminalStrip CreateTerminalStrip(
            Project project,
            FunctionDefinition funcDef,
            string fullName)
        {
            try
            {
                var tsd = new TerminalStrip();
                tsd.Create(project, funcDef);
                tsd.ManualPlacementType = DocumentTypeManager.DocumentType.Overview;
                tsd.Name = fullName;

                tsd.SymbolVariant = new SymbolVariant(
                    new Symbol(new SymbolLibrary(project,
                        "SPECIAL"), 6),
                    0);

                return tsd;
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
        /// remove all terminal strip placements on page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static bool RemoveAllTerminalStripPlacement(
            Page page)
        {
            try
            {
                foreach (Placement item in page.AllPlacements)
                {
                    if (item is TerminalStrip ts)
                    {
                        ts.RemoveFromPage();
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                new Decider().Decide(
                                    EnumDecisionType.eOkDecision,
                                    $"{ex.Message}",
                                    "Error",
                                    EnumDecisionReturn.eOK,
                                    EnumDecisionReturn.eOK);
                return false;
            }
        }

        /// <summary>
        /// set function article reference at index
        /// </summary>
        /// <param name="func"></param>
        /// <param name="strPartNr"></param>
        /// <param name="strVariant"></param>
        /// <param name="count"></param>
        /// <param name="index"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void SetArticleRefAtIndex(
            Function func,
            string strPartNr,
            string strVariant,
            uint count,
            int index) // start from 0
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            ArticleReference[] allRefs = func.ArticleReferences;
            if (index >= allRefs.Length)
            {
                func.AddArticleReference(strPartNr, strVariant, count);
                return;
            }

            var allData = allRefs
                .Select(r => (PartNr: r.PartNr, VariantNr: r.VariantNr, Count: (uint)r.Count))
                .ToList();

            allData[index] = (strPartNr, strVariant, count);

            foreach (var item in allRefs)
                func.RemoveArticleReference(item);

            foreach (var d in allData)
                func.AddArticleReference(d.PartNr, d.VariantNr, d.Count);
        }

        /// <summary>
        /// reset all function article references
        /// </summary>
        /// <param name="func"></param>
        /// <param name="strPartNrList"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ResetArticleRef(
            Function func,
            List<string> strPartNrList)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            ArticleReference[] allRefs = func.ArticleReferences;

            foreach (var item in allRefs)
                func.RemoveArticleReference(item);

            foreach (var partNr in strPartNrList)
                func.AddArticleReference(partNr);
        }

        /// <summary>
        /// set the property of specific article reference of function
        /// </summary>
        /// <param name="func"></param>
        /// <param name="articleRefIndex"></param>
        /// <param name="propertyId"></param>
        /// <param name="valueStr"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SetArticleRefProperty(
            Function func,
            int articleRefIndex,
            int propertyId,
            string valueStr)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (articleRefIndex < 0 || articleRefIndex >= func.ArticleReferences.Length)
                throw new ArgumentOutOfRangeException(nameof(articleRefIndex));

            func.ArticleReferences[articleRefIndex]
                .ParentObject
                .Properties[propertyId, articleRefIndex + 1] = valueStr;
        }

        /// <summary>
        /// set the property of specific article reference of function
        /// </summary>
        /// <param name="func"></param>
        /// <param name="articleRefIndex"></param>
        /// <param name="userDefinedPropertyName"></param>
        /// <param name="valueStr"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void SetArticleRefProperty(
            Function func,
            int articleRefIndex,
            string userDefinedPropertyName,
            string valueStr)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (articleRefIndex < 0 || articleRefIndex >= func.ArticleReferences.Length)
                throw new ArgumentOutOfRangeException(nameof(articleRefIndex));

            func.ArticleReferences[articleRefIndex]
                .ParentObject
                .Properties[userDefinedPropertyName][articleRefIndex + 1] = valueStr;
        }
    }
}