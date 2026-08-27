using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;

namespace EplanUtilities
{
    public static class PropertyUtility
    {
        public static string GetValueString(PropertyValue pValue)
        {
            if (pValue == null || pValue.IsEmpty) return null;

            try
            {
                switch (pValue.Definition.Type)
                {
                    case PropertyDefinition.PropertyType.Bool:
                        return pValue.ToBool() ? "YES" : "NO";
                    case PropertyDefinition.PropertyType.Long:
                        return pValue.ToString();
                    case PropertyDefinition.PropertyType.Double:
                        return pValue.ToDouble().ToString();
                    case PropertyDefinition.PropertyType.Coord:
                        return $"({pValue.ToPointD().X}, {pValue.ToPointD().Y})";
                    case PropertyDefinition.PropertyType.String:
                        return pValue.ToString();
                    case PropertyDefinition.PropertyType.Point:
                        return $"({pValue.ToPointD().X}, {pValue.ToPointD().Y})";
                    case PropertyDefinition.PropertyType.Time:
                        return pValue.ToTime().ToString();
                    case PropertyDefinition.PropertyType.MultilangString:
                        return pValue.ToMultiLangString().GetStringToDisplay(ISOCode.Language.L_zh_CN);
                    case PropertyDefinition.PropertyType.ValueWithUnit:
                        return pValue.ToString();
                    default:
                        return null;
                }
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
    }
}