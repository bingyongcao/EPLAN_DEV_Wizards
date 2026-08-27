using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.MasterData;
using System.Linq;
using Eplan.EplApi.HEServices;

namespace EplanUtilities
{
    public static class SelectionUtility
    {
        public static Page GetWorkingPage()
        {
            var sel = new SelectionSet();
            var openPages = sel.OpenedPages;

            if (openPages.Length > 0)
            {
                return openPages[0];
            }
            else
            {
                var selectedPages = sel.GetSelectedPages();
                if (selectedPages.Length == 1)
                {
                    return selectedPages[0];
                }
                else return null;
            }
        }
    }
}