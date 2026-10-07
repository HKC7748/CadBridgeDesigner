using System.ComponentModel;
using System.Windows;

namespace CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier.View
{
    public partial class ColumnPierAddView: Window
    {
        public ColumnPierAddView(ColumnPierAddViewModel columnPierAddViewModel)
        {
            InitializeComponent();
            DataContext = columnPierAddViewModel;
            columnPierAddViewModel.PropertyChanged += OnViewModelResultChanged;
        }

        private void OnViewModelResultChanged(object sender, PropertyChangedEventArgs e)
        {
            DialogResult = ((ColumnPierAddViewModel)DataContext).Result;
        }
    }
}