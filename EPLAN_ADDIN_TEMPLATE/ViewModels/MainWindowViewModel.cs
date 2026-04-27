using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eplan.EplApi.Base;
using Eplan.EplApi.HEServices;
using Eplan.EplApi.MasterData;
using System.Collections.ObjectModel;

namespace EPLAN_ADDIN_TEMPLATE.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private Eplan.EplApi.DataModel.Project activeProj;

        public IRelayCommand ConfirmCommand { get; }

        public MainWindowViewModel()
        {
            activeProj = new SelectionSet().GetCurrentProject(true);

            ConfirmCommand = new RelayCommand(Confirm, CanConfirm);
        }

        public ObservableCollection<MDPart> MDParts { get; } = new ObservableCollection<MDPart>();

        private MDPart _selectedMDPart;
        public MDPart SelectedMDPart
        {
            get => _selectedMDPart;
            set
            {
                if (SetProperty(ref _selectedMDPart, value))
                    ConfirmCommand.NotifyCanExecuteChanged();
            }
        }


        private void Confirm()
        {
            new Decider().Decide(
                EnumDecisionType.eOkDecision, 
                "Button was clicked", 
                "Tip", 
                EnumDecisionReturn.eOK, 
                EnumDecisionReturn.eOK);
        }

        private bool CanConfirm()
        {
            return true;
        }
    }
}
