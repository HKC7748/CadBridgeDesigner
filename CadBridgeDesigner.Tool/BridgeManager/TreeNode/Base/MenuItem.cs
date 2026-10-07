using CadBridgeDesigner.Tool.Tool;

namespace CadBridgeDesigner.Tool.BridgeManager.TreeNode.Base
{
    public class MenuItem
    {
        public string Header { get; set; }
        public RelayCommand RelayCommand { get; set; }
        
        public MenuItem(string header, RelayCommand relayCommand)
        {
            Header = header;
            RelayCommand = relayCommand;
        }
    }
}
