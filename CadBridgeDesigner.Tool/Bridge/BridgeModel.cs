using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CadBridgeDesigner.Core.Properties;
using CadBridgeDesigner.Tool.Bridge.Pier.ColumnPier;

namespace CadBridgeDesigner.Tool.Bridge
{
    public sealed class BridgeModel: INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ColumnPierModel> _piers;
        public ObservableCollection<ColumnPierModel> Piers
        {
            get => _piers;
            set
            {
                _piers = value;
                OnPropertyChanged();
            }
        }
        public BridgeModel()
        {
            Name = "BridgeModel";
            Piers = new ObservableCollection<ColumnPierModel>();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


