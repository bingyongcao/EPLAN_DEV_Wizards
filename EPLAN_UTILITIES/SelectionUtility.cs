using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using System.Linq;
using Eplan.EplApi.HEServices;

namespace EplanUtilities
{
    public static class SelectionUtility
    {
        public static PropertyValue GetWorkingCubicle()
        {
            var sel = new SelectionSet();
            var openPages = sel.OpenedPages;

            if (openPages.Length > 0)
            {
                var activePage = openPages[0];
                return activePage.Properties.DESIGNATION_PLANT;
            }
            else
            {
                var selectedPages = sel.GetSelectedPages();
                if (selectedPages.Length == 1)
                {
                    return selectedPages[0].Properties.DESIGNATION_PLANT;
                }
                else return null;
            }
        }
    }
}