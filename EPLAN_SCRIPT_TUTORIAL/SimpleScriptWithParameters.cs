using Eplan.EplApi.Base;
using Eplan.EplApi.Scripting;
using System;

namespace EPLAN_SCRIPT_TUTORIAL
{
    public class SimpleScriptWithParameters
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
