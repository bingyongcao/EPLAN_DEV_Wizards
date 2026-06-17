using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eplan.EplApi.Base;
using Eplan.EplApi.HEServices;
using Eplan.EplApi.MasterData;
using System.Collections.ObjectModel;

namespace EPLAN_ADDIN_TEMPLATE.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly Eplan.EplApi.DataModel.Project activeProj = new SelectionSet().GetCurrentProject(true);

        public ObservableCollection<MDPart> MDParts { get; } = new ObservableCollection<MDPart>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor("ConfirmCommand")]
        private MDPart selectedMDPart;

        [RelayCommand]
        private void Confirm() => new Decider().Decide(
            EnumDecisionType.eOkDecision,
            "Button was clicked",
            "Tip",
            EnumDecisionReturn.eOK,
            EnumDecisionReturn.eOK);
    }
}
