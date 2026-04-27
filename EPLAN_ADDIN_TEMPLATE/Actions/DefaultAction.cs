using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;
using EPLAN_ADDIN_TEMPLATE.Views;

namespace EPLAN_ADDIN_TEMPLATE
{
    public class DefaultAction : IEplAction
    {
        public static string ActionName = "DefaultAction";
        public bool Execute(ActionCallingContext ctx)
        {
            MainWindow mainWindow = new MainWindow();

            mainWindow.ShowDialog();

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