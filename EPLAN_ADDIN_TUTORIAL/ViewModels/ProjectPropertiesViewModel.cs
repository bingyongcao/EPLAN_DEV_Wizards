using System.ComponentModel;
using System.Runtime.CompilerServices;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;
using System.Collections.ObjectModel;

namespace EPLAN_API_TUTORIAL.ViewModels
{
    public class ProjectPropertiesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Models.ProjectPropertyModel> _properties;
        private string _projectName;
        private bool _isLoading;

        public event PropertyChangedEventHandler PropertyChanged;

        public ProjectPropertiesViewModel()
        {
            _properties = new ObservableCollection<Models.ProjectPropertyModel>();
            LoadProjectProperties();
        }

        public ObservableCollection<Models.ProjectPropertyModel> Properties
        {
            get => _properties;
            set
            {
                _properties = value;
                OnPropertyChanged();
            }
        }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadProjectProperties()
        {
            IsLoading = true;

            try
            {
                Project selectedProj = new SelectionSet().GetCurrentProject(true);
                ProjectName = selectedProj?.ProjectName ?? "No Project";

                if (selectedProj == null)
                {
                    IsLoading = false;
                    return;
                }

                foreach (PropertyValue propValue in selectedProj.Properties.ExistingValues)
                {
                    var propDef = propValue.Definition;

                    if (propDef.IsInternal) continue;

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
                                    PropertyValue = indexProp.ToString()
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
                                PropertyValue = propValue.ToString()
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

        public void Refresh()
        {
            Properties.Clear();
            LoadProjectProperties();
        }
    }
}
