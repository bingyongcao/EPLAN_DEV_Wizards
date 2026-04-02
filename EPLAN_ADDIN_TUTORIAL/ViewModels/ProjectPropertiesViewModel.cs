using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;
using System.Collections.ObjectModel;

namespace EPLAN_API_TUTORIAL.ViewModels
{
    public partial class ProjectPropertiesViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Models.ProjectPropertyModel> _properties = new();

        [ObservableProperty]
        private string _projectName = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        public ProjectPropertiesViewModel()
        {
            LoadProjectProperties();
        }

        private void LoadProjectProperties()
        {
            IsLoading = true;

            try
            {
                Project? selectedProj = new SelectionSet().GetCurrentProject(true);
                ProjectName = selectedProj?.ProjectName ?? "No Project";

                if (selectedProj == null)
                {
                    return;
                }

                foreach (PropertyValue propValue in selectedProj.Properties.ExistingValues)
                {
                    var propDef = propValue.Definition;

                    if (propDef.IsInternal)
                    {
                        continue;
                    }

                    if (propDef.IsIndexed)
                    {
                        for (int i = 1; i < propDef.MaxIndex + 1; i++)
                        {
                            var indexProp = propValue[i];
                            if (!indexProp.IsEmpty)
                            {
                                Properties.Add(new Models.ProjectPropertyModel
                                {
                                    PropertyId = indexProp.Id.AsInt,
                                    Index = i,
                                    PropertyName = indexProp.Definition.Name,
                                    PropertyValue = indexProp.ToString() ?? string.Empty
                                });
                            }
                        }
                    }
                    else
                    {
                        if (!propValue.IsEmpty)
                        {
                            Properties.Add(new Models.ProjectPropertyModel
                            {
                                PropertyId = propValue.Id.AsInt,
                                Index = null,
                                PropertyName = propValue.Definition.Name,
                                PropertyValue = propValue.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void Refresh()
        {
            Properties.Clear();
            LoadProjectProperties();
        }
    }
}
