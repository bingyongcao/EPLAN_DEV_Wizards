using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using System.Linq;

namespace EplanUtilities
{
    public static class FunctionUtility
    {
        public static bool CreateSubFunc(
            Function parentFunc, 
            Page targetPage, 
            SymbolVariant sv,
            PointD loc,
            string placeSchemaName = "")
        {
            try
            {
                var subFunc = new Function();
                subFunc.Create(targetPage, sv);
                subFunc.IsMainFunction = false;
                subFunc.Name = parentFunc.Name;
                subFunc.VisibleName = parentFunc.VisibleName;

                //set pins descriptions
                var pinCount = parentFunc.FunctionDefinition.ConnectionPoints.Length;
                for (int i = 0; i < pinCount; i++)
                {
                    subFunc.Properties.FUNC_CONNECTIONDESIGNATION[i + 1]
                        = parentFunc.Properties.FUNC_CONNECTIONDESIGNATION[i + 1];
                }

                // 铭牌文本
                subFunc.Properties[20025] = parentFunc.Properties[20025];
                // 技术参数
                subFunc.Properties[20027] = parentFunc.Properties[20027];

                //set location
                subFunc.Location = loc;

                //adjust representation type
                subFunc.ManualPlacementType = targetPage.PageType;

                if (!string.IsNullOrEmpty(placeSchemaName))
                {
                    subFunc.PropertyPlacementsSchemas.Selected =
                    subFunc.PropertyPlacementsSchemas.All.First(s => s.Name == placeSchemaName);
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
    }
}