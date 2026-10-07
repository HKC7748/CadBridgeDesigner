using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode;
using CadBridgeDesigner.Tool.BridgeManager.TreeNode.Base;
using CadBridgeDesigner.Tool.Properties;

namespace CadBridgeDesigner.Tool.BridgeManager
{
    public class BridgeManagerViewModel: INotifyPropertyChanged
    {
        private BridgeManagerModel _bridgeManagerModel;
        public BridgeManagerModel BridgeManagerModel
        {
            get => _bridgeManagerModel;
            set
            {
                _bridgeManagerModel = value;
                OnPropertyChanged();
            }
        }
        
        private ObservableCollection<BaseTreeNodeViewModel> _treeNodes;
    
        public ObservableCollection<BaseTreeNodeViewModel> TreeNodes
        {
            get => _treeNodes;
            set
            {
                _treeNodes = value;
                OnPropertyChanged();
            }
        }
        
        public BridgeManagerViewModel(BridgeManagerModel bridgeManagerModel)
        {
            BridgeManagerTool.TreeNodesRefreshRequested += OnTreeNodesRefreshRequested;
            BridgeManagerModel = bridgeManagerModel;
        }

        private void OnTreeNodesRefreshRequested()
        {
            TreeNodes = BridgeManagerTool.GetTreeNodes();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


