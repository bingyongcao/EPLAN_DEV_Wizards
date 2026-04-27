using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using System.Linq;
using StorableObject = Eplan.EplApi.DataModel.StorableObject;

namespace EplanHelpers
{
    public class PropertyUtility
    {
        public static PropertyValue GetPropValueByInt<T>(T obj, int propInt) where T : StorableObject
        {
            var propId = obj.Properties.ExistingIds
             .Where(anyPropertyId => anyPropertyId.AsInt == propInt)
             .FirstOrDefault();

            if (propId == null) return null;

            return obj.Properties[propId];
        }

        public static string GetPropertyValueString(PropertyValue pValue)
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
    }
}