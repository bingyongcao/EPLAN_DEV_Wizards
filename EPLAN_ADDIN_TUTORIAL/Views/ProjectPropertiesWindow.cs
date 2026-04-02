using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using EPLAN_API_TUTORIAL.ViewModels;

namespace EPLAN_API_TUTORIAL.Views
{
    public class ProjectPropertiesWindow : Window
    {
        private readonly ProjectPropertiesViewModel _viewModel;

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
            Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));

            var mainGrid = new Grid { Margin = new Thickness(10) };
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Header
            var headerPanel = new StackPanel { Margin = new Thickness(0, 0, 0, 15) };

            var titleText = new TextBlock
            {
                Text = "Project Properties",
                FontSize = 24,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(0, 120, 212))
            };
            headerPanel.Children.Add(titleText);

            var projectNameText = new TextBlock
            {
                FontSize = 14,
                Margin = new Thickness(0, 5, 0, 0),
                Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100))
            };
            projectNameText.SetBinding(TextBlock.TextProperty, new Binding("ProjectName"));
            headerPanel.Children.Add(projectNameText);

            Grid.SetRow(headerPanel, 0);
            mainGrid.Children.Add(headerPanel);

            // DataGrid
            var dataGrid = new DataGrid
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
                Margin = new Thickness(0, 0, 0, 10),
                BorderBrush = new SolidColorBrush(Color.FromRgb(220, 220, 220)),
                BorderThickness = new Thickness(1),
                Background = new SolidColorBrush(Colors.White),
                RowBackground = new SolidColorBrush(Colors.White),
                AlternatingRowBackground = new SolidColorBrush(Color.FromRgb(245, 245, 245))
            };

            dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "ID",
                Binding = new Binding("DisplayId"),
                Width = 80
            });
            dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Property Name",
                Binding = new Binding("PropertyName"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });
            dataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Value",
                Binding = new Binding("PropertyValue"),
                Width = new DataGridLength(2, DataGridLengthUnitType.Star)
            });

            dataGrid.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Properties"));

            Grid.SetRow(dataGrid, 1);
            mainGrid.Children.Add(dataGrid);

            // Footer
            var footerPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };

            var countText = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 20, 0),
                Foreground = new SolidColorBrush(Color.FromRgb(100, 100, 100))
            };
            countText.SetBinding(TextBlock.TextProperty, new Binding("Properties.Count")
            {
                StringFormat = "Total: {0} properties"
            });
            footerPanel.Children.Add(countText);

            var refreshButton = new Button
            {
                Content = "Refresh",
                Width = 100,
                Height = 32,
                Background = new SolidColorBrush(Color.FromRgb(0, 120, 212)),
                Foreground = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 212)),
                Cursor = System.Windows.Input.Cursors.Hand
            };
            refreshButton.SetBinding(Button.CommandProperty, new Binding("RefreshCommand"));
            footerPanel.Children.Add(refreshButton);

            Grid.SetRow(footerPanel, 2);
            mainGrid.Children.Add(footerPanel);

            Content = mainGrid;
        }
    }
}
