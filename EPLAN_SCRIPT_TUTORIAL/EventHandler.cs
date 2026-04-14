#region should be included to avoid namespace conflict
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
# endregion

using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Scripting;

namespace EPLAN_SCRIPT_TUTORIAL
{
    /// <summary>
    /// no need to unregister event handler, 
    /// EPLAN will automatically remove all event handlers added by 
    /// the script when the script is reloaded or removed
    /// </summary>
    public class EventHandler
    {
        [DeclareEventHandler("Eplan.EplApi.OnPostOpenProject")]
        public void OnPostOpenProject(IEventParameter parameter)
        {
            MessageBox.Show(
                "Project named" + new EventParameterString(parameter).String + " was open!");
        }
    }
}
