using System.ComponentModel;
using System.Runtime.CompilerServices;
using CadBridgeDesigner.Tool.Properties;

namespace CadBridgeDesigner.Tool.Bridge
{
    public class BridgeEditViewModel: INotifyPropertyChanged
    {
        private BridgeModel _bridgeModel;

        private BridgeModel BridgeModel
        {
            get => _bridgeModel;
            set
            {
                _bridgeModel = value;
                OnPropertyChanged();
            }
        }

        public BridgeEditViewModel(BridgeModel bridgeModel)
        {
            BridgeModel = bridgeModel;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


