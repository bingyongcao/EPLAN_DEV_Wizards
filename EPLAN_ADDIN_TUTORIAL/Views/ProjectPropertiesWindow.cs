using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data;
using EPLAN_API_TUTORIAL.ViewModels;

namespace EPLAN_API_TUTORIAL.Views
{
    public class ProjectPropertiesWindow : System.Windows.Window
    {
        private readonly ProjectPropertiesViewModel _viewModel;
        private DataGrid _dataGrid;
        private TextBlock _countTextBlock;
        private TextBlock _projectNameTextBlock;

        public ProjectPropertiesWindow()
        {
            _viewModel = new ProjectPropertiesViewModel();
            BuildUI();
            DataContext = _viewModel;
        }

        private void BuildUI()
        {
            Title = "Project Properties";
            Height = 600;
            Width = 900;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));

            var mainGrid = new Grid { Margin = new Thickness(10) };
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Header
            var headerPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 10) };

            var titleText = new TextBlock
            {
                Text = "Project Properties",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(0, 120, 212))
            };
            headerPanel.Children.Add(titleText);

            _projectNameTextBlock = new TextBlock
            {
                FontSize = 14,
                Margin = new Thickness(0, 5, 0, 0),
                Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100))
            };
            _projectNameTextBlock.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("ProjectName"));
            headerPanel.Children.Add(_projectNameTextBlock);

            Grid.SetRow(headerPanel, 0);
            mainGrid.Children.Add(headerPanel);

            // DataGrid
            _dataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                IsReadOnly = true,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                SelectionMode = DataGridSelectionMode.Extended,
                GridLinesVisibility = DataGridGridLinesVisibility.Horizontal,
                HeadersVisibility = DataGridHeadersVisibility.Column,
                AlternationCount = 2,
                RowHeaderWidth = 0,
                Margin = new Thickness(0, 0, 0, 10)
            };

            _dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "ID",
                Binding = new System.Windows.Data.Binding("DisplayId"),
                Width = 80
            });
            _dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Property Name",
                Binding = new System.Windows.Data.Binding("PropertyName"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });
            _dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Value",
                Binding = new System.Windows.Data.Binding("PropertyValue"),
                Width = new DataGridLength(2, DataGridLengthUnitType.Star)
            });

            _dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new System.Windows.Data.Binding("Properties"));

            Grid.SetRow(_dataGrid, 1);
            mainGrid.Children.Add(_dataGrid);

            // Footer
            var footerPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            _countTextBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 20, 0),
                Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100))
            };
            _countTextBlock.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("Properties.Count")
            {
                StringFormat = "Total: {0} properties"
            });
            footerPanel.Children.Add(_countTextBlock);

            var refreshButton = new Button
            {
                Content = "Refresh",
                Width = 80,
                Padding = new Thickness(5, 3, 5, 3)
            };
            refreshButton.Click += RefreshButton_Click;
            footerPanel.Children.Add(refreshButton);

            Grid.SetRow(footerPanel, 2);
            mainGrid.Children.Add(footerPanel);

            Content = mainGrid;

            Loaded += (s, e) => { };
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Refresh();
        }
    }
}
