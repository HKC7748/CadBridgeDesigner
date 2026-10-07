using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CadBridgeDesigner.Tool.BridgeManager.TreeNode.Base
{
    public class BaseTreeNodeViewModel : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;
                _isSelected = value;
                OnPropertyChanged();
                if (_isSelected)
                {
                    OnNodeClick();
                }
                    
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(); }
        }

        protected virtual void OnNodeClick()
        {
        }
        
        private ObservableCollection<MenuItem> _menuItems;
        public ObservableCollection<MenuItem> MenuItems
        {
            get => _menuItems;
            private set { _menuItems = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BaseTreeNodeViewModel> _children;
        public ObservableCollection<BaseTreeNodeViewModel> Children
        {
            get => _children;
            set { _children = value; OnPropertyChanged(); }
        }
        
        public BaseTreeNodeViewModel()
        {
            Name = string.Empty;
            IsSelected = false;
            IsExpanded = true;
            MenuItems = new ObservableCollection<MenuItem>();
            Children = new ObservableCollection<BaseTreeNodeViewModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


