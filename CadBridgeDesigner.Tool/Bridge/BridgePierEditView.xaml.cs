using System.Windows.Controls;

namespace CadBridgeDesigner.Tool.Bridge
{
    public partial class BridgePierEditView : UserControl
    {
        public BridgePierEditView(BridgeEditViewModel bridgeEditViewModel)
        {
            InitializeComponent();
            DataContext = bridgeEditViewModel;
        }
    }
}



