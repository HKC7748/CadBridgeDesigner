using System.Windows.Controls;

namespace CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier.View
{
    public partial class ColumnPierEditView: UserControl
    {
        public ColumnPierEditView(ColumnPierEditViewModel columnPierEditViewModel)
        {
            InitializeComponent();
            DataContext = columnPierEditViewModel;
        }
    }
}