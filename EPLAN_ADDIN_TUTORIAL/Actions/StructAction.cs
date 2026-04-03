using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Base;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;
using EPLAN_API_TUTORIAL.Views;
using System.Linq;

namespace EPLAN_API_TUTORIAL
{
    public class StructAction : IEplAction
    {
        public static string ActionName = "StructAction";

        public bool Execute(ActionCallingContext ctx)
        {
            Project activeProj = new SelectionSet().GetCurrentProject(true);

            // get higher-level function objects
            var locObjs = activeProj.GetLocationObjects(Project.Hierarchy.Plant);

            string message = "Properties of plant structure identifier\n";

            for ( int i = 0; i < locObjs.Count(); i++)
            {
                var locObj = locObjs[i];
                message += $"-----------------row{i}-----------------\n";

                // get location description field
                //var descProp = locObj.Properties.LOCATION_DESCRIPTION;

                // get supplementary field, which is an indexed property
                //var supplementaryProp = locObj.Properties.LOCATION_DESCRIPTION_SUPPLEMENTARYFIELD;

                foreach (var propValue in locObj.Properties.ExistingValues)
                {
                    var propDef = propValue.Definition;

                    if (propDef.IsInternal)
                    {
                        continue;
                    }

                    if (propDef.IsIndexed)
                    {
                        for (int j = 1; j < propDef.MaxIndex + 1; j++)
                        {
                            var indexProp = propValue[j];
                            if (!indexProp.IsEmpty)
                            {
                                message += $"<{indexProp.Id.AsInt} {j}> " +
                                    $"{indexProp.Definition.Name}: " +
                                    $"{indexProp.ToString() ?? string.Empty}\n";
                            }
                        }
                    }
                    else
                    {
                        if (!propValue.IsEmpty)
                        {
                            message += $"<{propValue.Id.AsInt}> " +
                                    $"{propValue.Definition.Name}: " +
                                    $"{propValue.ToString() ?? string.Empty}\n";
                        }
                    }
                }
            }

            new Decider().Decide(
                EnumDecisionType.eOkCancelDecision,
                message,
                "Structure Identifier-Plant",
                EnumDecisionReturn.eOK,
                EnumDecisionReturn.eOK);

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
