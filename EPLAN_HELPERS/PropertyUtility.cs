using Eplan.EplApi.DataModel;
using System.Linq;

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
    }
}