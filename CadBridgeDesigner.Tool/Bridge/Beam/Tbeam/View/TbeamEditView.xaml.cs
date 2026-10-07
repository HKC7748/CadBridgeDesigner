using System.Windows.Controls;

namespace CadBridgeDesigner.Tool.Bridge.Beam.Tbeam.View
{
    public partial class TbeamEditView : UserControl
    {
        public TbeamEditView(TbeamEditViewModel tbeamEditViewModel)
        {
            InitializeComponent();
            DataContext = tbeamEditViewModel;
        }
    }
}

