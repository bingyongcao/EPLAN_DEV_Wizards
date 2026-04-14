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
using Eplan.EplApi.Base;
using Eplan.EplApi.Gui;
using Eplan.EplApi.Scripting;

namespace EPLAN_SCRIPT_TUTORIAL
{
    public class CallScriptByCLI
    {
        /// <summary>
        /// The function with a 'Start' attribute is the entry point of the script. 
        /// It will be called by EPLAN when the script is executed.
        /// Call script by command line: 
        /// W3u.exe ExecuteScript /ScriptFile:"~\SimpleScriptWithParameters.cs" /Param1:"Hello" /Param2:"EPLAN"
        /// </summary>
        /// <param name="Param1"></param>
        /// <param name="Param2"></param>
        /// <returns></returns>
        [Start]
        public bool FunctionWithParameters(String Param1, String Param2)
        {
            new Decider().Decide(
                EnumDecisionType.eOkDecision, 
                Param1 + Param2, 
                "SimpleScriptWithParams", 
                EnumDecisionReturn.eOK, 
                EnumDecisionReturn.eOK);

            return true;
        }
    }
}
